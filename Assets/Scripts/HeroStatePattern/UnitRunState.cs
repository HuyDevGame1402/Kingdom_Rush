using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRunState : UnitBaseState
{
    private Vector2 actualTargetPosition;

    public UnitRunState(BaseUnitStateMachine unit) : base(unit) { }

    public override void Enter()
    {
        // Chạy animation di chuyển
        var config = unit.unitData.animations.run;
        SpriteSheetAnimator.Instance.PlayAnimation(unit.spriteObject, unit.unitData.animations.animPrefix, config.startFrame, config.endFrame);

        // Đặt mục tiêu di chuyển
        SetTargetDestination();
    }

    private void SetTargetDestination()
    {
        // ƯU TIÊN 1: Nếu đang di chuyển theo cờ -> Tính vị trí cờ (+ offset ngẫu nhiên)
        if (unit.isRunToFlag)
        {
            float randomRadius = Random.Range(0.3f, 0.6f);
            Vector2 randomOffset = Random.insideUnitCircle.normalized * randomRadius;
            actualTargetPosition = (Vector2)unit.positionFlag + randomOffset;
        }
        // ƯU TIÊN 2: Nếu không có cờ -> Đi theo target hiện tại (Enemy hoặc Point)
        else if (unit.currentTarget != null)
        {
            if (unit.IsTargetEnemy())
            {
                actualTargetPosition = unit.currentTarget.position;
            }
            else
            {
                float randomRadius = Random.Range(0.3f, 0.6f);
                Vector2 randomOffset = Random.insideUnitCircle.normalized * randomRadius;
                actualTargetPosition = (Vector2)unit.currentTarget.position + randomOffset;
            }
        }
    }

    public override void Update()
    {
        // Nếu mất target VÀ không có lệnh chạy tới cờ -> Quay về Idle
        if ((unit.currentTarget == null || !unit.currentTarget.gameObject.activeSelf) && !unit.isRunToFlag)
        {
            unit.TransitionToState(unit.IdleState);
            return;
        }

        // CHỈ cập nhật đuổi theo Enemy NẾU lính KHÔNG trong trạng thái chạy theo Cờ
        if (!unit.isRunToFlag && unit.IsTargetEnemy() && unit.currentTarget != null)
        {
            actualTargetPosition = unit.currentTarget.position;
        }

        Vector2 currentPos = unit.transform.position;
        float distance = Vector2.Distance(currentPos, actualTargetPosition);

        //--------------------------------------------------
        // TRƯỜNG HỢP 1: ĐANG CHẠY ĐẾN CỜ (IS RUN TO FLAG)
        //--------------------------------------------------
        if (unit.isRunToFlag)
        {
            // Đến đích cờ (bán kính <= 0.15f)
            if (distance <= 0.15f)
            {
                unit.isRunToFlag = false; // ✅ Đã đến cờ -> Reset trạng thái cờ
                unit.TransitionToState(unit.IdleState); // Về Idle để bắt đầu quét tìm enemy xung quanh cờ mới
                return;
            }
        }
        //--------------------------------------------------
        // TRƯỜNG HỢP 2: TỰ ĐỘNG ĐỦI THEO ĐỊCH (KHI KHÔNG CÓ CỜ)
        //--------------------------------------------------
        else if (unit.IsTargetEnemy())
        {
            bool inAttackRange = distance <= unit.unitData.attackRange;

            if (inAttackRange)
            {
                // Căn chỉnh trục Y với quái trước khi đánh
                if (!unit.IsAlignedWithTarget())
                {
                    Vector2 alignPos = new Vector2(unit.transform.position.x, unit.currentTarget.position.y);
                    Vector2 dirAlign = (alignPos - currentPos).normalized;

                    unit.transform.position += (Vector3)(dirAlign * unit.unitData.moveSpeed * Time.deltaTime);

                    if (dirAlign.x != 0)
                    {
                        float scaleX = (dirAlign.x > 0 ? 1 : -1) * unit.unitData.heroScale;
                        unit.spriteObject.transform.localScale = new Vector3(scaleX, unit.unitData.heroScale, 1);
                    }
                    return;
                }

                // Đã nằm trong tầm đánh & thẳng hàng Y
                if (unit.CanAttack())
                {
                    unit.TransitionToState(unit.AttackState);
                }
                else
                {
                    unit.TransitionToState(unit.IdleState);
                }
                return;
            }
        }
        //--------------------------------------------------
        // TRƯỜNG HỢP 3: ĐIỂM CHỈ ĐỊNH KHÁC
        //--------------------------------------------------
        else
        {
            if (distance <= 0.15f)
            {
                unit.currentTarget = null;
                unit.TransitionToState(unit.IdleState);
                return;
            }
        }

        //--------------------------------------------------
        // XỬ LÝ DI CHUYỂN (MOVE LOGIC)
        //--------------------------------------------------
        Vector3 direction = ((Vector3)actualTargetPosition - unit.transform.position).normalized;
        unit.transform.position += direction * unit.unitData.moveSpeed * Time.deltaTime;

        if (direction.x != 0)
        {
            float scaleX = (direction.x > 0 ? 1 : -1) * unit.unitData.heroScale;
            unit.spriteObject.transform.localScale = new Vector3(scaleX, unit.unitData.heroScale, 1);
        }
    }

    public override void Exit() { }
}