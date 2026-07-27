using UnityEngine;

public class UnitAttackState : UnitBaseState
{
    private bool isAttacking;
    public bool IsAttacking => isAttacking;

    public UnitAttackState(BaseUnitStateMachine unit) : base(unit) { }
    private int damageFinal;
    private bool isLongAttackRangeCurrent;

    public override void Enter()
    {
        FaceTarget();
        isAttacking = true;
        //unit.lastAttackTime = Time.time;
        isLongAttackRangeCurrent = unit.unitData.isLongRangeAttack;

        if (unit.CheckEnemyInAttackLongRange())
        {
            isLongAttackRangeCurrent = true;
        }
        else
        {
            isLongAttackRangeCurrent = false;
        }

        if(unit.unitData.isLongRangeAttack && isLongAttackRangeCurrent == false)
        {
            unit.lastAttackTime = Time.time + unit.unitData.attackCooldownAdd;
        }
        else
        {
            unit.lastAttackTime = Time.time;
        }

        unit.baseUnitAnimationHandler.PlayAttackAnimation(
            unit.unitData.animations,
            unit.spriteObject,
            onEventTrigger: () => {
                // cận chiến
                if(isLongAttackRangeCurrent == false)
                {
                    // Sát thương khi chạm eventFrame (nếu cần xử lý tại frame này)
                    if (unit.currentTarget != null && unit.currentTarget.TryGetComponent(out EnemyController enemy))
                    {
                        damageFinal = DamageStatic.GetDamageBase(unit.GetComponent<HeroDataInGame>().minDamage,
                            unit.GetComponent<HeroDataInGame>().maxDamage);
                        enemy.TakeDamage(damageFinal
                            /*DamageStatic.GetDamageBase((int)unit.unitData.minDamage, (int)unit.unitData.maxDamage)*/,
                            unit.textSO,
                            unit.transform
                        );
                    }

                    // Add Exp if hero is PlayerHero not hero of tower or hero farmer
                    if (unit.TryGetComponent(out HeroEXPManager expManager))
                    {
                        expManager.OnDealDamage(damageFinal);
                    }
                    if (SoundGameAttackManager.Instance != null)
                    {
                        SoundGameAttackManager.Instance.PlayAudioSoliderAttack();
                    }
                }
                // đánh xa
                else
                {
                    if(
                    //unit.currentTarget.TryGetComponent(out EnemyController enemyController) &&
                    //enemyController.isDead == false &&
                    unit.TryGetComponent(out IHasSpawnBullet spawnBullet))
                    {
                        spawnBullet.SpawnBullet(unit.currentTarget);
                    }
                }
            },
            onComplete: () => {
                isAttacking = false;
            }, isLongAttackRangeCurrent, unit.unitData.isLongRangeAttack
        );

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
    private void FaceTarget()
    {
        if (unit.currentTarget == null)
            return;

        float dirX = unit.currentTarget.position.x - unit.transform.position.x;

        if (Mathf.Abs(dirX) < 0.01f)
            return;

        float scaleX = (dirX > 0 ? 1f : -1f) * unit.unitData.heroScale;

        unit.spriteObject.transform.localScale =
            new Vector3(
                scaleX,
                unit.unitData.heroScale,
                1f);
    }
}