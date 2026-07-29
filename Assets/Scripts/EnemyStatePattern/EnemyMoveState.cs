using UnityEngine;
public class EnemyMoveState : IEnemyState
{
    public void EnterState(EnemyController enemy)
    {
        enemy.RecalculateNextWaypoint();
        // THÊM DÒNG NÀY: Ép cập nhật lại hoạt ảnh di chuyển ở frame kế tiếp
        enemy.ResetAnimDirection();
    }

    public void UpdateState(EnemyController enemy)
    {
        if (enemy.isDead || enemy.isFrozen) return;

        if (enemy.target != null && enemy.target.TryGetComponent(out BaseUnitStateMachine baseUnitStateMachine)
            && baseUnitStateMachine.isRunToFlag == true)
        {
            enemy.ResetTarget();
        }

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
                bool soldierTargetsMe = enemy.target.TryGetComponent(out BaseUnitStateMachine soldierRef)
                                         && soldierRef.IsTargetingEnemy(enemy);

                if (enemy.ShouldMoveBackToTarget())
                {
                    // Soldier còn ở phía trước (hoặc ngang hàng) -> cứ tiến tới đánh
                    MoveTowardsTarget(enemy, distance);
                }
                else if (soldierTargetsMe)
                {
                    // Soldier ĐANG thật sự target mình nhưng đã ở phía sau -> đứng chờ, không quay đầu
                    enemy.TransitionToState(enemy.IdleState);
                }
                else
                {
                    // Soldier không target mình, đã ở phía sau -> nó sẽ không bao giờ tự tới -> bỏ qua, đi tiếp
                    enemy.ResetTarget();
                    enemy.HandleMovement();
                }
                return;
            }
        }

        // 2. Không có ai chặn đường -> hành quân theo Waypoints bình thường
        enemy.HandleMovement();
    }

    //public void UpdateState(EnemyController enemy)
    //{
    //    if (enemy.isDead || enemy.isFrozen) return;

    //    // 0. Nếu target hiện tại đang chạy về cờ -> Hủy target
    //    if (enemy.target != null && enemy.target.TryGetComponent(out BaseUnitStateMachine baseUnitStateMachine)
    //        && baseUnitStateMachine.isRunToFlag == true)
    //    {
    //        enemy.target = null;
    //        enemy.ResetTarget();
    //    }

    //    // --- BỔ SUNG ĐOẠN NÀY ---
    //    // Nếu target đang null nhưng trong list vẫn còn mục tiêu khác -> Lấy target mới ngay
    //    if (enemy.target == null && enemy.targetList.Count > 0)
    //    {
    //        enemy.ResetTarget();
    //    }
    //    // ------------------------

    //    // 1. Nếu đang có mục tiêu -> ưu tiên xử lý mục tiêu
    //    if (enemy.target != null)
    //    {
    //        float distance = Vector2.Distance(enemy.transform.position, enemy.target.position);
    //        bool inAttackRange = distance <= enemy.unitData.attackRange;

    //        if (inAttackRange)
    //        {
    //            if (!enemy.IsAlignedWithTarget(0.1f))
    //            {
    //                MoveTowardsTargetY(enemy);
    //                return;
    //            }
    //            else
    //            {
    //                enemy.TransitionToState(enemy.AttackState);
    //                return;
    //            }
    //        }
    //        else
    //        {
    //            bool soldierTargetsMe = enemy.target.TryGetComponent(out BaseUnitStateMachine soldierRef)
    //                                     && soldierRef.IsTargetingEnemy(enemy);

    //            if (enemy.ShouldMoveBackToTarget())
    //            {
    //                MoveTowardsTarget(enemy, distance);
    //            }
    //            else if (soldierTargetsMe)
    //            {
    //                enemy.TransitionToState(enemy.IdleState);
    //            }
    //            else
    //            {
    //                // Bỏ target cũ ở phía sau, thử ResetTarget để tìm xem trong list có ai khác ở phía trước không
    //                enemy.ResetTarget();

    //                // Nếu sau khi ResetTarget vẫn null (không còn ai hợp lệ) -> Mới chạy tiếp
    //                if (enemy.target == null)
    //                {
    //                    enemy.HandleMovement();
    //                }
    //            }
    //            return;
    //        }
    //    }

    //    // 2. Không có ai chặn đường -> hành quân theo Waypoints
    //    enemy.HandleMovement();
    //}

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