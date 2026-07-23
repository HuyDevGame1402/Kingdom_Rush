using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRunState : UnitBaseState
{
    // Biến lưu vị trí đích thực tế sau khi đã tính toán (có thể có offset hoặc không)
    private Vector2 actualTargetPosition;

    public UnitRunState(BaseUnitStateMachine unit) : base(unit) 
    {
        
    }

    public override void Enter()
    {
        // Chạy animation di chuyển
        var config = unit.unitData.animations.run;
        SpriteSheetAnimator.Instance.PlayAnimation(unit.spriteObject, unit.unitData.animations.animPrefix, config.startFrame, config.endFrame);

        if (unit.isRunToFlag)
        {
            float randomRadius = Random.Range(0.3f, 0.6f);
            Vector2 randomOffset = Random.insideUnitCircle.normalized * randomRadius;

            actualTargetPosition = (Vector2)unit.positionFlag + randomOffset;
            return;
        }
        else
        {
            // --- KHỞI TẠO ĐIỂM ĐÍCH THỰC TẾ ---
            if (unit.currentTarget != null)
            {
                if (unit.IsTargetEnemy())
                {
                    // Nếu là Enemy: Đi thẳng tới vị trí hiện tại của Enemy
                    actualTargetPosition = unit.currentTarget.position;
                }
                else
                {
                    // Nếu KHÔNG PHẢI Enemy (Điểm chỉ định/Rally Point): 
                    // Random một điểm xung quanh Target trong bán kính nhỏ (Ví dụ: từ 0.3 đến 0.6 đơn vị) để tránh chụm lại một chỗ
                    float randomRadius = Random.Range(0.3f, 0.6f);
                    Vector2 randomOffset = Random.insideUnitCircle.normalized * randomRadius;

                    actualTargetPosition = (Vector2)unit.currentTarget.position + randomOffset;
                }
            }
        }
    }

    public override void Update()
    {
        if ((unit.currentTarget == null || !unit.currentTarget.gameObject.activeSelf)
            && unit.isRunToFlag == false)
        {
            unit.TransitionToState(unit.IdleState);
            return;
        }

        if (unit.IsTargetEnemy())
        {
            actualTargetPosition = unit.currentTarget.position;
        }

        Vector2 currentPos = unit.transform.position;

        float distance = Vector2.Distance(currentPos, actualTargetPosition);

        //--------------------------------------------------
        // ĐỊCH
        //--------------------------------------------------
        if (unit.IsTargetEnemy())
        {
            bool inAttackRange = distance <= unit.unitData.attackRange;

            // Đã tới tầm đánh
            if (inAttackRange)
            {
                // Sử dụng chung hàm IsAlignedWithTarget() thay vì check cứng > 0.1f
                if (!unit.IsAlignedWithTarget())
                {
                    // Nếu chưa thẳng hàng theo tolerance (ví dụ 0.01f), tiếp tục đi chỉnh Y
                    Vector2 alignPos = new Vector2(
                        unit.transform.position.x,
                        unit.currentTarget.position.y);

                    Vector2 dir = (alignPos - currentPos).normalized;

                    unit.transform.position +=
                        (Vector3)(dir * unit.unitData.moveSpeed * Time.deltaTime);

                    if (dir.x != 0)
                    {
                        float scaleX = (dir.x > 0 ? 1 : -1) * unit.unitData.heroScale;
                        unit.spriteObject.transform.localScale = new Vector3(scaleX, unit.unitData.heroScale, 1);
                    }

                    return;
                }

                // Đã ngang hàng -> CHỈ chuyển sang Attack nếu đã HỒI COOLDOWN
                if (unit.CanAttack())
                {
                    unit.TransitionToState(unit.AttackState);
                }
                else
                {
                    // Nếu chưa hồi chiêu thì về Idle đứng đợi, tránh lặp State gây lỗi tốc đánh
                    unit.TransitionToState(unit.IdleState);
                }
                return;
            }
        }
        //--------------------------------------------------
        // POINT
        //--------------------------------------------------
        else
        {
            if (distance <= 0.15f)
            {
                if (unit.isRunToFlag)
                {
                    unit.isRunToFlag = false;
                }
                else
                {
                    unit.currentTarget = null;
                    unit.TransitionToState(unit.IdleState);
                }
                return;
            }
        }

        //--------------------------------------------------
        // MOVE
        //--------------------------------------------------
        Vector3 direction =
            ((Vector3)actualTargetPosition - unit.transform.position).normalized;

        unit.transform.position +=
            direction * unit.unitData.moveSpeed * Time.deltaTime;

        if (direction.x != 0)
        {
            float scaleX =
                (direction.x > 0 ? 1 : -1) * unit.unitData.heroScale;

            unit.spriteObject.transform.localScale =
                new Vector3(scaleX,
                            unit.unitData.heroScale,
                            1);
        }
    }

    public override void Exit() { }
}