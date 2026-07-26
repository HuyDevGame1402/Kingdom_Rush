using UnityEngine;

public class UnitAttackState : UnitBaseState
{
    private bool isAttacking;
    public bool IsAttacking => isAttacking;

    public UnitAttackState(BaseUnitStateMachine unit) : base(unit) { }

    public override void Enter()
    {
        isAttacking = true;
        unit.lastAttackTime = Time.time;
        unit.baseUnitAnimationHandler.PlayAttackAnimation(
            unit.unitData.animations,
            unit.spriteObject,
            onEventTrigger: () => {
                // Sát thương khi chạm eventFrame (nếu cần xử lý tại frame này)
            },
            onComplete: () => {
                if (unit.currentTarget != null && unit.currentTarget.TryGetComponent(out EnemyController enemy))
                {
                    enemy.TakeDamage(
                        DamageStatic.GetDamageBase((int)unit.unitData.minDamage, (int)unit.unitData.maxDamage),
                        unit.textSO
                    );
                }
                isAttacking = false;
            }
        );

        if (SoundGameAttackManager.Instance != null)
        {
            SoundGameAttackManager.Instance.PlayAudioSoliderAttack();
        }
    }

    public override void Update()
    {
        // 🚨 ƯU TIÊN CAO NHẤT: Bấm Cờ -> Hủy đánh lập tức và chạy tới Cờ
        if (unit.isRunToFlag)
        {
            isAttacking = false;
            unit.TransitionToState(unit.RunState);
            return;
        }

        // Nếu đang vung kiếm chưa xong -> Không làm gì khác
        if (isAttacking) return;

        // Nếu mục tiêu biến mất hoặc chết -> Reset target và về Idle
        if (unit.currentTarget == null || !unit.currentTarget.gameObject.activeSelf ||
            (unit.currentTarget.TryGetComponent(out EnemyController enemy) && enemy.isDead))
        {
            unit.ResetTarget();
            unit.TransitionToState(unit.IdleState);
            return;
        }

        // Đánh giá lại tầm đánh
        if (unit.IsTargetInAttackRange() && unit.IsAlignedWithTarget())
        {
            if (unit.CanAttack())
                Enter();
            else
                unit.TransitionToState(unit.IdleState);
        }
        else
        {
            unit.TransitionToState(unit.IdleState);
        }
    }

    public override void Exit()
    {
        isAttacking = false;
    }
}