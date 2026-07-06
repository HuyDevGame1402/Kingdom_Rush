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

    [SerializeField] private Transform brokenArrow;


    private Vector3 pointA;

    private float elapsed = 0f;

    private float flightTime = 1f;   // tổng thời gian bay ước tính khi launch

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
        if (targetEnemy.parent.TryGetComponent(out EnemyController enemyCtr)
            && enemyCtr.isDead == false)
        {
            Debug.LogWarning("Attack Enemy: " + damage);
            enemyCtr.TakeDamage(damage, textSO);
            base.OnHitTarget();
            // Sound
            if(SoundGameAttackManager.Instance != null)
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

        Vector3 B = targetEnemy.position;
        Vector3 M = Vector3.Lerp(pointA, B, 0.5f) + Vector3.up * arcHeight;

        // Xấp xỉ độ dài cung Bezier bằng cách chia nhỏ và cộng dồn
        float arcLength = 0f;
        Vector3 prev = pointA;
        int samples = 20;
        for (int i = 1; i <= samples; i++)
        {
            float ti = i / (float)samples;
            float inv = 1f - ti;
            Vector3 pt = inv * inv * pointA + 2f * inv * ti * M + ti * ti * B;
            arcLength += Vector3.Distance(prev, pt);
            prev = pt;
        }

        flightTime = Mathf.Max(arcLength / arrowSpeed, 0.05f);
    }



    // ── Mỗi frame ────────────────────────────────────────────────────────────

    protected override void MoveLogic()

    {

        Vector3 B = targetEnemy.position;



        // Trúng đích nếu đủ gần

        if (Vector3.Distance(transform.position, B) <= snapDistance || elapsed >= flightTime)

        {

            transform.position = B;

            OnHitTarget();

            return;

        }



        // elapsed tăng theo thời gian thực, chia flightTime → t luôn 0→1 đúng tốc độ

        elapsed += Time.deltaTime;

        float t = Mathf.Clamp01(elapsed / flightTime);

        float inv = 1f - t;



        // Điểm đỉnh M cập nhật theo enemy

        Vector3 M = Vector3.Lerp(pointA, B, 0.5f) + Vector3.up * arcHeight;



        // Vị trí Quadratic Bezier

        Vector3 newPos = inv * inv * pointA

                       + 2f * inv * t * M

                       + t * t * B;



        // Tangent B'(t) = 2[(1-t)(M-A) + t(B-M)]

        Vector3 tangent = 2f * (inv * (M - pointA) + t * (B - M));

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

        if (!Application.isPlaying || !isFlying || targetEnemy == null) return;

        Vector3 B = targetEnemy.position;

        Vector3 M = Vector3.Lerp(pointA, B, 0.5f) + Vector3.up * arcHeight;

        Gizmos.color = Color.yellow;

        Vector3 prev = pointA;

        for (int i = 1; i <= 30; i++)

        {

            float ti = i / 30f;

            float inv = 1f - ti;

            Vector3 pt = inv * inv * pointA + 2f * inv * ti * M + ti * ti * B;

            Gizmos.DrawLine(prev, pt);

            prev = pt;

        }

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(B, snapDistance);

    }

#endif

}