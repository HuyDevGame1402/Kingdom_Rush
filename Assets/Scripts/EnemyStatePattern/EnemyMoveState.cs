using UnityEngine;

public class EnemyMoveState : IEnemyState
{
    public void EnterState(EnemyController enemy)
    {
        // Debug.Log($"[{enemy.name}] Bắt đầu di chuyển dọc hành lang.");
    }

    public void UpdateState(EnemyController enemy)
    {
        // 1. Kiểm tra xem có mục tiêu chặn đường không
        if (enemy.target != null && enemy.IsTargetInAttackRange())
        {
            enemy.TransitionToState(enemy.AttackState);
            return;
        }

        // 2. Nếu không có ai chặn đường, tiếp tục hành quân theo Waypoints
        enemy.HandleMovement();
    }

    public void ExitState(EnemyController enemy)
    {
        // Rời trạng thái di chuyển
    }
}