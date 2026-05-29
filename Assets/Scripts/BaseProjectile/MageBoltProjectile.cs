using UnityEngine;

public class MageBoltProjectile : BaseProjectile
{
    [Header("Animation Frames")]
    [SerializeField] private string animName = "magebolt_";
    [SerializeField] private int startFlyFrame = 1;
    [SerializeField] private int endFlyFrame = 2;
    [SerializeField] private int startHitFrame = 3;
    [SerializeField] private int endHitFrame = 10;

    public override void Launch(Transform enemy, float projectileSpeed)
    {
        base.Launch(enemy, projectileSpeed);

        // Vừa bay ra là lập tức lặp lại (Loop) hoạt ảnh đạn bay (Frame 1 -> 2)
        SpriteSheetAnimator.Instance.PlayAnimation(gameObject, animName, startFlyFrame, endFlyFrame);
    }

    protected override void MoveLogic()
    {
        // Đạn pháp sư bay thẳng trực diện hướng vào tâm của Quái vật
        Vector3 direction = (targetEnemy.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Xoay viên đạn hướng theo mục tiêu (Nếu sprite đạn của bạn có hướng đuôi)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Kiểm tra va chạm bằng khoảng cách nhỏ (Thay vì dùng Collider phức tạp)
        if (Vector3.Distance(transform.position, targetEnemy.position) <= 0.1f)
        {
            OnHitTarget(); // Đã chạm mục tiêu
        }
    }

    protected override void OnHitTarget()
    {
        isFlying = false; // Dừng di chuyển

        // Diễn hoạt ảnh nổ và chỉ định rõ ràng callback onComplete
        SpriteSheetAnimator.Instance.PlayAnimation(
            target: gameObject,
            animPrefix: animName,
            startFrame: startHitFrame,
            endFrame: endHitFrame,
            frameRate: 0.05f, // Cho tốc độ nổ nhanh hơn một chút nhìn sẽ phê hơn (0.05s mỗi frame)
            onComplete: () =>
            {
                // Chắc chắn sẽ chạy vào đây khi kết thúc frame 10
                Debug.Log("Đã nổ xong -> Ẩn đạn");
                gameObject.SetActive(false);
            }
        );
    }
}