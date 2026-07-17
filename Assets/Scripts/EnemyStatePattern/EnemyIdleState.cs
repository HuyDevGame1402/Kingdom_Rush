using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    public void EnterState(EnemyController enemy)
    {
        // Phát animation Idle
        if (EnemySpriteAnimator.Instance != null)
        {
            string id = enemy.unitData.unitName;
            string prefix = enemy.unitData.animations.animPrefix;
            float frameRate = enemy.unitData.animations.frameRate;

            EnemySpriteAnimator.Instance.PlayAnimationByRange(
                enemy.gameObject,
                id,
                prefix,
                enemy.unitData.animations.idle,
                frameRate
            );
        }

        // Reset hướng animation
        enemy.ResetAnimDirection();
    }

    public void UpdateState(EnemyController enemy)
    {
        if (enemy.isDead || enemy.isFrozen)
            return;

        // Không còn mục tiêu -> tiếp tục hành quân
        if (enemy.target == null)
        {
            enemy.TransitionToState(enemy.MoveState);
            return;
        }

        float distance = Vector2.Distance(
            enemy.transform.position,
            enemy.target.position);

        // Nếu đã vào tầm đánh
        if (distance <= enemy.unitData.attackRange)
        {
            if (enemy.IsAlignedWithTarget(0.1f))
            {
                enemy.TransitionToState(enemy.AttackState);
            }
            else
            {
                // Chưa thẳng hàng Y thì sang MoveState để chỉnh vị trí
                enemy.TransitionToState(enemy.MoveState);
            }

            return;
        }

        // Hero đã chạy đủ gần để enemy chủ động quay lại
        if (enemy.ShouldMoveBackToTarget())
        {
            enemy.TransitionToState(enemy.MoveState);
        }

        // Nếu chưa thì tiếp tục đứng chờ.
    }

    public void ExitState(EnemyController enemy)
    {

    }
}