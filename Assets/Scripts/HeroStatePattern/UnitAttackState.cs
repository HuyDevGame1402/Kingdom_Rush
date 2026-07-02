using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackState : UnitBaseState
{
    private bool isAttacking;

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
                // Gây sát thương lên mục tiêu ngay tại event frame (ví dụ frame 11)
            },
            frameRate: -1f,
            onComplete: () => {
                if (unit.currentTarget != null)
                {
                    unit.currentTarget.GetComponent<EnemyController>().TakeDamage(
                        DamageStatic.GetDamageBase((int)unit.unitData.minDamage, (int)unit.unitData.maxDamage));

                }
                isAttacking = false; // Hoàn thành chuỗi đánh
            }
        );
    }

    public override void Update()
    {
        // Đang vung kiếm đánh thì không được làm việc khác
        if (isAttacking) return;

        // Đánh xong rồi thì đánh giá lại: còn tầm đánh không? Đủ cooldown chưa?
        if (unit.IsTargetInAttackRange() && unit.IsAlignedWithTarget())
        {
            if (unit.CanAttack() && unit.currentTarget.GetComponent<EnemyController>().isDead == false)
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