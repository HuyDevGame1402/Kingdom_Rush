using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdleState : UnitBaseState
{
    public UnitIdleState(BaseUnitStateMachine unit) : base(unit) { }

    public override void Enter()
    {
        unit.baseUnitAnimationHandler.PlayIdleAnimation(unit.unitData.animations, unit.spriteObject);
        //var config = unit.unitData.animations.idle;
        //SpriteSheetAnimator.Instance.PlayAnimation(unit.spriteObject, unit.unitData.animations.animPrefix, config.startFrame, config.endFrame);
    
        if(unit.currentTarget != null && unit.IsTargetEnemy() && unit.currentTarget.GetComponent<EnemyController>().isDead)
        {
            unit.ResetTarget();
        }
    }

    public override void Update()
    {
        // Nếu không có mục tiêu nào cả -> Đứng im an toàn, không làm gì hết
        if (unit.currentTarget == null || !unit.currentTarget.gameObject.activeSelf)
            return;

        // Ép tọa độ về 2D để tính khoảng cách chính xác
        Vector2 línhPos2D = unit.transform.position;
        Vector2 mụcTiêuPos2D = unit.currentTarget.position;
        float distance = Vector2.Distance(línhPos2D, mụcTiêuPos2D);

        // PHÂN LOẠI XỬ LÝ RÕ RÀNG:
        if (unit.IsTargetEnemy())
        {
            // --- LOGIC CHIẾN ĐẤU VỚI ENEMY ---
            if (distance <= unit.unitData.attackRange)
            {
                if (unit.IsAlignedWithTarget())
                {
                    if (unit.CanAttack())
                        unit.TransitionToState(unit.AttackState);
                }
                else
                {
                    unit.TransitionToState(unit.RunState);
                }
            }
            else
            {
                unit.TransitionToState(unit.RunState); // Địch ở xa -> Chạy đuổi theo
            }
        }
        else
        {
            // --- LOGIC DI CHUYỂN ĐẾN ĐIỂM POINT ---
            // Chỉ khi nào khoảng cách thực sự LỚN HƠN hẳn phạm vi dừng (0.25f) thì mới được phép Chạy
            if (distance > 0.25f)
            {
                unit.TransitionToState(unit.RunState);
            }
            // Nếu nhỏ hơn hoặc bằng 0.25f, code sẽ đứng im ở IdleState này và không đi đâu cả!
        }
    }

    public override void Exit() { }
}
