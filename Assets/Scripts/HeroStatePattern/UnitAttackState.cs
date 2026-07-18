using UnityEngine;

public class UnitAttackState : UnitBaseState
{
    private bool isAttacking;
    public bool IsAttacking => isAttacking;

    public UnitAttackState(BaseUnitStateMachine unit) : base(unit) { }

    public override void Enter()
    {
        isAttacking = true;
        unit.lastAttackTime = Time.time; // Ghi nhận thời gian ra đòn

        var config = unit.unitData.animations.attack;

        SpriteSheetAnimator.Instance.PlayAnimation(
        target: unit.spriteObject,
        animPrefix: unit.unitData.animations.animPrefix,
        startFrame: config.startFrame,
        endFrame: config.endFrame,
        eventFrame: config.eventFrame,
        onEventTrigger: () => {
            // Gây sát thương ngay tại event frame (ví dụ frame 11)
        },
        offsetConfigs: config.animationConfigOffset,
        frameRate: /*-1f*/unit.unitData.animations.frameRate,
            onComplete: () => {
                if (unit.currentTarget != null)
                {
                    unit.currentTarget.GetComponent<EnemyController>().TakeDamage(
                        DamageStatic.GetDamageBase((int)unit.unitData.minDamage, (int)unit.unitData.maxDamage),
                        unit.textSO
                    );
                }
                isAttacking = false;
            }
        );
    }

    public override void Update()
    {
        // Đang vung kiếm đánh thì không được làm việc khác
        if (isAttacking) return;

        // PHÒNG THỦ: Nếu mục tiêu đột ngột biến mất hoặc chết, quay về Idle ngay
        if (unit.currentTarget == null || !unit.currentTarget.gameObject.activeSelf ||
            unit.currentTarget.GetComponent<EnemyController>().isDead)
        {
            unit.ResetTarget(); // Tìm mục tiêu mới từ list
            unit.TransitionToState(unit.IdleState);
            return;
        }

        // Đánh xong rồi thì đánh giá lại: còn tầm đánh không? Đủ cooldown chưa?
        if (unit.IsTargetInAttackRange() && unit.IsAlignedWithTarget())
        {
            if (unit.CanAttack())
                Enter(); // Tiếp tục làm một hit đánh mới
            else
                unit.TransitionToState(unit.IdleState); // Chờ hồi chiêu
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