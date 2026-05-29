using UnityEngine;

public class BombProjectile : BaseProjectile
{
    [Header("Components Setup")]
    [SerializeField] private GameObject bombSpriteObj;
    [SerializeField] private GameObject explosionObj;

    [Header("Explosion Animation Setup")]
    [SerializeField] private string explosionAnimName;
    [SerializeField] private int explosionStartFrame;
    [SerializeField] private int explosionEndFrame;

    [Header("Ballistic Settings")]
    [SerializeField] private float flightDuration = 1.2f;
    [SerializeField] private float arcHeightBonus = 3f;

    [Header("Rotation Settings")]
    [SerializeField] private float spinSpeed = 300f;      // Độ/giây — tăng/giảm tùy ý
    [SerializeField] private bool spinClockwise = true;   // Chiều quay

    private Vector3 fixedTargetPos;
    private float elapsedTime;
    private float currentAngle;   // Góc quay tích lũy
    private bool hasLanded = false;

    public void LaunchWithArc(Transform enemy, float bombSpeed, float arcHeight)
    {
        fixedTargetPos = (enemy != null) ? enemy.position : transform.position;
        base.Launch(enemy, bombSpeed);

        hasLanded = false;
        elapsedTime = 0f;
        currentAngle = 0f;

        arcHeightBonus = arcHeight + arcHeightBonus;

        if (bombSpriteObj != null) bombSpriteObj.SetActive(true);
        if (explosionObj != null) explosionObj.SetActive(false);

        transform.rotation = Quaternion.identity;
        if (bombSpriteObj != null)
            bombSpriteObj.transform.rotation = Quaternion.identity;
    }

    protected override void MoveLogic()
    {
        if (hasLanded) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= flightDuration)
        {
            transform.position = fixedTargetPos;
            OnHitTarget();
            return;
        }

        // ── t mượt hơn: dùng SmoothStep thay Lerp thẳng
        // SmoothStep làm bom "ease in" lúc bắn và "ease out" lúc rơi xuống đích
        float tRaw = elapsedTime / flightDuration;
        float tSmooth = Mathf.SmoothStep(0f, 1f, tRaw);

        // Nội suy vị trí ngang với t đã smooth
        Vector3 pos = Vector3.Lerp(startPosition, fixedTargetPos, tSmooth);

        // Parabol dùng tRaw để sin đối xứng đúng (không bị lệch bởi smooth)
        pos.y += Mathf.Sin(tRaw * Mathf.PI) * arcHeightBonus;

        transform.position = pos;

        // ── Quay liên tục tích lũy, không reset mỗi frame
        float direction = spinClockwise ? -1f : 1f;
        currentAngle += direction * spinSpeed * Time.deltaTime;

        if (bombSpriteObj != null)
            bombSpriteObj.transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
    }

    protected override void OnHitTarget()
    {
        hasLanded = true;
        isFlying = false;

        if (bombSpriteObj != null) bombSpriteObj.SetActive(false);

        if (explosionObj != null)
        {
            explosionObj.SetActive(true);
            SpriteSheetAnimator.Instance.PlayAnimation(
                target: explosionObj,
                animPrefix: explosionAnimName,
                startFrame: explosionStartFrame,
                endFrame: explosionEndFrame,
                frameRate: -1,
                eventFrame: -1,
                onEventTrigger: null,
                onComplete: () =>
                {
                    explosionObj.SetActive(false);
                    gameObject.SetActive(false);
                }
            );
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}