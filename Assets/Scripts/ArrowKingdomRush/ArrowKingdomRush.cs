using System.Collections;
using UnityEngine;

public class ArrowKingdomRush : BaseProjectile
{
    [Header("Arc")]
    [SerializeField] private float arcHeight = 2f;

    [Header("Speed")]
    [SerializeField] private float arrowSpeed = 20f;

    [Header("Rotation")]
    [SerializeField] private float rotSpeed = 1440f;

    [Header("Hit Detection")]
    [SerializeField] private float snapDistance = 0.25f;
    [SerializeField] private float missTolerance = 0.6f; // enemy lệch quá xa điểm dự đoán → coi như trượt
    [SerializeField] private Transform brokenArrow;

    private Vector3 pointA;
    private Vector3 pointM;
    private Vector3 pointB;      // điểm đích CỐ ĐỊNH (đã dự đoán), không đổi trong lúc bay
    private float elapsed = 0f;
    private float flightTime = 1f;

    private EnemyController cachedEnemyCtr;

    // ── API ──────────────────────────────────────────────────────────────────

    public void LaunchWithArc(Transform enemy, float shootSpeed, float height,
        int damage = 1)
    {
        arrowSpeed = shootSpeed;
        arcHeight = height;
        base.Launch(enemy, shootSpeed, damage);
    }

    protected override void OnHitTarget()
    {
        // Kiểm tra enemy thực tế còn sống và còn đủ gần điểm dự đoán hay không
        bool valid = cachedEnemyCtr != null && cachedEnemyCtr.isDead == false;

        if (valid)
        {
            float actualDist = Vector3.Distance(targetEnemy.position, pointB);
            if (actualDist > missTolerance)
                valid = false; // enemy đã đi lệch quá xa dự đoán (đổi hướng đột ngột, v.v.)
        }

        if (valid)
        {
            Debug.LogWarning("Attack Enemy: " + damage);
            cachedEnemyCtr.TakeDamage(damage, textSO);
            base.OnHitTarget();

            if (SoundGameAttackManager.Instance != null)
            {
                SoundGameAttackManager.Instance.PlayAudioArrowHit();
            }
        }
        else
        {
            isFlying = false;
            ShowBrokenArrow();
        }
    }

    private void ShowBrokenArrow()
    {
        brokenArrow.gameObject.SetActive(true);
        transform.GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(CoroutineHideBrokenArrow());
    }

    private IEnumerator CoroutineHideBrokenArrow()
    {
        yield return new WaitForSeconds(1f);
        HideBrokenArrow();
    }

    private void HideBrokenArrow()
    {
        brokenArrow.gameObject.SetActive(false);
        transform.GetComponent<SpriteRenderer>().enabled = true;
        transform.gameObject.SetActive(false);
    }

    // ── Init ─────────────────────────────────────────────────────────────────

    protected override void OnLaunched()
    {
        elapsed = 0f;
        pointA = transform.position;
        if (speed > 0f) arrowSpeed = speed;

        cachedEnemyCtr = null;
        if (targetEnemy != null && targetEnemy.parent != null)
            targetEnemy.parent.TryGetComponent(out cachedEnemyCtr);

        // Bước 1: ước lượng thô flightTime dựa trên vị trí hiện tại của enemy
        Vector3 currentEnemyPos = targetEnemy.position;
        float estFlightTime = Vector3.Distance(pointA, currentEnemyPos) / Mathf.Max(arrowSpeed, 0.01f);
        estFlightTime = Mathf.Max(estFlightTime, 0.05f);

        // Bước 2: lặp vài vòng để hội tụ điểm dự đoán + flightTime + arcLength
        Vector3 predictedB = currentEnemyPos;
        const int iterations = 3;
        for (int iter = 0; iter < iterations; iter++)
        {
            predictedB = cachedEnemyCtr != null
                ? cachedEnemyCtr.GetFuturePosition(estFlightTime)
                : currentEnemyPos;

            Vector3 M = Vector3.Lerp(pointA, predictedB, 0.5f) + Vector3.up * arcHeight;

            float arcLength = 0f;
            Vector3 prev = pointA;
            int samples = 20;
            for (int i = 1; i <= samples; i++)
            {
                float ti = i / (float)samples;
                float inv = 1f - ti;
                Vector3 pt = inv * inv * pointA + 2f * inv * ti * M + ti * ti * predictedB;
                arcLength += Vector3.Distance(prev, pt);
                prev = pt;
            }

            estFlightTime = Mathf.Max(arcLength / arrowSpeed, 0.05f);
        }

        // Chốt lại quỹ đạo CỐ ĐỊNH: A, M, B không đổi trong suốt quá trình bay
        pointB = predictedB;
        pointM = Vector3.Lerp(pointA, pointB, 0.5f) + Vector3.up * arcHeight;
        flightTime = estFlightTime;
    }

    // ── Mỗi frame ────────────────────────────────────────────────────────────

    protected override void MoveLogic()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / flightTime);
        float inv = 1f - t;

        // Quỹ đạo parabol CỐ ĐỊNH (không đuổi theo enemy nữa)
        Vector3 newPos = inv * inv * pointA
                       + 2f * inv * t * pointM
                       + t * t * pointB;

        if (t >= 1f)
        {
            transform.position = pointB;
            OnHitTarget();
            return;
        }

        // Tangent B'(t) = 2[(1-t)(M-A) + t(B-M)]
        Vector3 tangent = 2f * (inv * (pointM - pointA) + t * (pointB - pointM));
        if (tangent.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = rotSpeed <= 0f
                ? targetRot
                : Quaternion.RotateTowards(transform.rotation, targetRot,
                                           rotSpeed * Time.deltaTime);
        }

        transform.position = newPos;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || !isFlying) return;

        Gizmos.color = Color.yellow;
        Vector3 prev = pointA;
        for (int i = 1; i <= 30; i++)
        {
            float ti = i / 30f;
            float inv = 1f - ti;
            Vector3 pt = inv * inv * pointA + 2f * inv * ti * pointM + ti * ti * pointB;
            Gizmos.DrawLine(prev, pt);
            prev = pt;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointB, snapDistance);

        Gizmos.color = Color.cyan;
        if (targetEnemy != null)
            Gizmos.DrawWireSphere(targetEnemy.position, missTolerance);
    }
#endif
}