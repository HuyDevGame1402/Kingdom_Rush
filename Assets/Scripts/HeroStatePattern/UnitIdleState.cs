using UnityEngine;

public class UnitIdleState : UnitBaseState
{
    public UnitIdleState(BaseUnitStateMachine unit) : base(unit) { }

    public override void Enter()
    {
        unit.baseUnitAnimationHandler.PlayIdleAnimation(
            unit.unitData.animations,
            unit.spriteObject);

        // Nếu target chết thì reset
        if (unit.currentTarget != null &&
            unit.IsTargetEnemy() &&
            unit.currentTarget.TryGetComponent(out EnemyController enemy) &&
            enemy.isDead)
        {
            unit.ResetTarget();
        }
    }

    public override void Update()
    {
        if (unit.currentTarget == null || !unit.currentTarget.gameObject.activeSelf)
            return;

        //==================================================
        // TARGET LÀ ENEMY
        //==================================================
        if (unit.IsTargetEnemy())
        {
            //--------------------------------------------------
            // HERO ĐÁNH XA
            //--------------------------------------------------
            if (unit.unitData.isLongRangeAttack)
            {
                // ===== ƯU TIÊN 1 : Enemy áp sát =====
                if (unit.CheckEnemyInAttackCloseCombat())
                {
                    float distance = Vector2.Distance(
                        unit.transform.position,
                        unit.currentTarget.position);

                    if (distance <= unit.unitData.attackRange)
                    {
                        if (unit.IsAlignedWithTarget())
                        {
                            if (unit.CanAttack())
                            {
                                unit.TransitionToState(unit.AttackState);
                            }
                        }
                        else
                        {
                            unit.TransitionToState(unit.RunState);
                        }
                    }
                    else
                    {
                        unit.TransitionToState(unit.RunState);
                    }

                    return;
                }

                // ===== ƯU TIÊN 2 : Enemy ở vùng bắn xa =====
                if (unit.CheckEnemyInAttackLongRange())
                {
                    if (unit.CanAttack())
                    {
                        unit.TransitionToState(unit.AttackState);
                    }

                    // Chưa hết cooldown thì đứng Idle
                    return;
                }

                // ===== Chưa vào bất kỳ vùng nào =====
                unit.TransitionToState(unit.RunState);
                return;
            }

            //--------------------------------------------------
            // HERO CẬN CHIẾN
            //--------------------------------------------------
            float meleeDistance = Vector2.Distance(
                unit.transform.position,
                unit.currentTarget.position);

            if (meleeDistance <= unit.unitData.attackRange)
            {
                if (!unit.IsAlignedWithTarget())
                {
                    unit.TransitionToState(unit.RunState);
                    return;
                }

                if (unit.CanAttack())
                {
                    unit.TransitionToState(unit.AttackState);
                }

                return;
            }

            unit.TransitionToState(unit.RunState);
            return;
        }

        //==================================================
        // TARGET KHÔNG PHẢI ENEMY
        //==================================================
        float pointDistance = Vector2.Distance(
            unit.transform.position,
            unit.currentTarget.position);

        if (pointDistance > 0.25f)
        {
            unit.TransitionToState(unit.RunState);
        }
    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {
    }
}