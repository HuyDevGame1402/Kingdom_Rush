using UnityEngine;
public class EnemyMoveState : IEnemyState
{
    public void EnterState(EnemyController enemy)
    {
        // Khi bắt đầu vào trạng thái di chuyển(hoặc từ trạng thái Đánh quay về lại)
        // Tính toán lại waypoint tối ưu nhất dựa trên vị trí hiện tại của quái
        enemy.RecalculateNextWaypoint();
        // THÊM DÒNG NÀY: Ép cập nhật lại hoạt ảnh di chuyển ở frame kế tiếp
        enemy.ResetAnimDirection();
    }

    public void UpdateState(EnemyController enemy)
    {
        if (enemy.isDead || enemy.isFrozen) return;
        // 1. Nếu đang có mục tiêu (Player) chặn đường -> ưu tiên xử lý mục tiêu, KHÔNG chạy waypoint nữa
        if (enemy.target != null)
        {
            float distance = Vector2.Distance(enemy.transform.position, enemy.target.position);
            bool inAttackRange = distance <= enemy.unitData.attackRange;

            if (inAttackRange)
            {
                // Nếu lọt vào tầm đánh nhưng TRỤC Y CHƯA THẲNG HÀNG (< 0.15f)
                if (!enemy.IsAlignedWithTarget(0.1f))
                {
                    MoveTowardsTargetY(enemy);
                    return;
                }
                else
                {
                    // Đã vào tầm đánh VÀ đã thẳng hàng trục Y -> Đánh luôn
                    enemy.TransitionToState(enemy.AttackState);
                    return;
                }
            }
            else
            {
                // Chưa vào tầm đánh -> chủ động di chuyển thẳng về phía target
                // thay vì tiếp tục chạy theo waypoint
                MoveTowardsTarget(enemy, distance);
                return;
            }
        }

        // 2. Không có ai chặn đường -> hành quân theo Waypoints bình thường
        enemy.HandleMovement();
    }

    // Di chuyển tịnh tiến trục Y để áp sát mục tiêu khi đã trong tầm đánh
    private void MoveTowardsTargetY(EnemyController enemy)
    {
        Vector2 currentPos = enemy.transform.position;
        Vector2 alignPos = new Vector2(enemy.transform.position.x, enemy.target.position.y);
        Vector2 dirY = (alignPos - currentPos).normalized;
        float distanceThisFrame = enemy.unitData.moveSpeed * Time.deltaTime;

        enemy.transform.position += (Vector3)(dirY * distanceThisFrame);

        UpdateFacing(enemy, enemy.target.position - enemy.transform.position);
    }

    // Di chuyển thẳng về phía target khi chưa vào tầm đánh
    private void MoveTowardsTarget(EnemyController enemy, float distance)
    {
        Vector3 direction = (enemy.target.position - enemy.transform.position).normalized;
        float distanceThisFrame = enemy.unitData.moveSpeed * Time.deltaTime;

        // Tránh overshoot qua target nếu bước di chuyển frame này dài hơn khoảng cách còn lại
        if (distanceThisFrame >= distance)
        {
            enemy.transform.position = enemy.target.position;
        }
        else
        {
            enemy.transform.Translate(direction * distanceThisFrame, Space.World);
        }

        UpdateFacing(enemy, direction);

        // Nếu bạn muốn quái chạy animation di chuyển bình thường khi đuổi target,
        // có thể gọi lại logic animation hướng đi tương tự UpdateMoveAnimation trong EnemyController.
        // Ví dụ (cần chuyển UpdateMoveAnimation thành public trong EnemyController rồi gọi ở đây):
        // enemy.UpdateMoveAnimation(direction);
    }

    // Lật mặt sprite trái/phải dựa theo hướng di chuyển
    private void UpdateFacing(EnemyController enemy, Vector3 direction)
    {
        if (direction.x > 0.1f) enemy.transform.localScale = enemy.unitData.localScaleRight;
        else if (direction.x < -0.1f) enemy.transform.localScale = enemy.unitData.localScaleLeft;
    }

    public void ExitState(EnemyController enemy)
    {
        // Rời trạng thái di chuyển
    }
}