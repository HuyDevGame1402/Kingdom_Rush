//using UnityEngine;

//public class EnemyMoveState : IEnemyState
//{
//    public void EnterState(EnemyController enemy)
//    {
//        // Khởi tạo trạng thái di chuyển
//    }

//    public void UpdateState(EnemyController enemy)
//    {
//        // 1. Kiểm tra xem có mục tiêu (Player) chặn đường không
//        if (enemy.target != null)
//        {
//            float distance = Vector2.Distance(enemy.transform.position, enemy.target.position);
//            bool inAttackRange = distance <= enemy.unitData.attackRange;

//            if (inAttackRange)
//            {
//                // Nếu lọt vào tầm đánh nhưng TRỤC Y CHƯA THẲNG HÀNG (< 0.05f)
//                if (!enemy.IsAlignedWithTarget(0.15f))
//                {
//                    // Ép quái di chuyển tịnh tiến theo trục Y để áp sát Player
//                    Vector2 currentPos = enemy.transform.position;
//                    Vector2 alignPos = new Vector2(enemy.transform.position.x, enemy.target.position.y);
//                    Vector2 dirY = (alignPos - currentPos).normalized;

//                    float distanceThisFrame = enemy.unitData.moveSpeed * Time.deltaTime;

//                    // Di chuyển quái tịnh tiến Y
//                    enemy.transform.position += (Vector3)(dirY * distanceThisFrame);

//                    // Cập nhật hướng xoay mặt Sprite (Trái/Phải) dựa vào vị trí của Player
//                    Vector3 directionToPlayer = enemy.target.position - enemy.transform.position;
//                    if (directionToPlayer.x > 0.1f) enemy.transform.localScale = enemy.unitData.localScaleRight;
//                    else if (directionToPlayer.x < -0.1f) enemy.transform.localScale = enemy.unitData.localScaleLeft;

//                    return; // Ngắt hàm ở đây, không cho chạy tiếp xuống Waypoint
//                }
//                else
//                {
//                    // Đã vào tầm đánh VÀ đã thẳng hàng trục Y -> Đánh luôn
//                    enemy.TransitionToState(enemy.AttackState);
//                    return;
//                }
//            }
//        }

//        // 2. Nếu không có ai chặn đường (hoặc chưa vào tầm đánh), tiếp tục hành quân theo Waypoints bình thường
//        enemy.HandleMovement();
//    }

//    public void ExitState(EnemyController enemy)
//    {
//        // Rời trạng thái di chuyển
//    }
//}

using UnityEngine;
public class EnemyMoveState : IEnemyState
{
    public void EnterState(EnemyController enemy)
    {
        // Khởi tạo trạng thái di chuyển
    }

    public void UpdateState(EnemyController enemy)
    {
        // 1. Nếu đang có mục tiêu (Player) chặn đường -> ưu tiên xử lý mục tiêu, KHÔNG chạy waypoint nữa
        if (enemy.target != null)
        {
            float distance = Vector2.Distance(enemy.transform.position, enemy.target.position);
            bool inAttackRange = distance <= enemy.unitData.attackRange;

            if (inAttackRange)
            {
                // Nếu lọt vào tầm đánh nhưng TRỤC Y CHƯA THẲNG HÀNG (< 0.15f)
                if (!enemy.IsAlignedWithTarget(0.15f))
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