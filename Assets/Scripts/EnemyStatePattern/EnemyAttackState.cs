using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private float attackTimer = 0f;

    private int finnalDamage;
    private bool isCauseDamage;

    public void EnterState(EnemyController enemy)
    {
        // Sẵn sàng tấn công ngay khi vừa áp sát mục tiêu
        attackTimer = enemy.unitData.attackCooldown;
        // Quay mặt về phía target ngay khi bắt đầu tấn công
        UpdateFacing(enemy);
    }

    public void UpdateState(EnemyController enemy)
    {
        if (enemy.isDead || enemy.isFrozen) return;

        if (enemy.target != null && enemy.target.TryGetComponent(out BaseUnitStateMachine baseUnitStateMachine)
            && baseUnitStateMachine.isRunToFlag == true)
        {
            enemy.target = null;
            enemy.ResetTarget();
        }

        // Nếu mục tiêu bỗng nhiên biến mất, mất tầm đánh HOẶC bị lệch trục Y quá 0.05f -> quay lại MoveState để đuổi tiếp
        if (enemy.target == null || !enemy.IsTargetInAttackRange() || !enemy.IsAlignedWithTarget(0.1f))
        {
            enemy.TransitionToState(enemy.MoveState);
            return;
        }
        UpdateFacing(enemy);
        // Đếm ngược thời gian hồi đòn đánh (Cooldown)
        attackTimer += Time.deltaTime;

        if (attackTimer >= enemy.unitData.attackCooldown)
        {
            attackTimer = 0f;
            ExecuteAttack(enemy);
        }
    }

    private void ExecuteAttack(EnemyController enemy)
    {
        if (CharacterSpriteAnimator.Instance == null) return;
        isCauseDamage = false;
        // 1. Lấy dữ liệu định danh và cấu hình đòn đánh từ ScriptableObject
        string id = enemy.unitData.unitName;
        string prefix = enemy.unitData.animations.animPrefix; // Dùng prefix động thay vì fix cứng "goblin_"
        float frameRate = enemy.unitData.animations.frameRate;

        // Cụm cấu hình tấn công và thủ thế (Idle)
        AnimationFrameRange attackConfig = enemy.unitData.animations.attack;
        AnimationFrameRange idleConfig = enemy.unitData.animations.idle;
        finnalDamage = DamageStatic.GetDamageBase((int)enemy.unitData.minDamage,
                            (int)enemy.unitData.maxDamage);
        // 2. Kích hoạt hoạt ảnh tấn công (Truyền nguyên cụm Object chứa list Offset chỉnh tay)
        CharacterSpriteAnimator.Instance.PlayAnimationByRange(
            enemy.gameObject, id, prefix, attackConfig, frameRate,
            onEventTrigger: () =>
            {
                if (enemy.target.GetComponent<BaseUnitStateMachine>().isRunToFlag == false)
                {
                    if (enemy.target.TryGetComponent(out HeroGeraldController geraldController))
                    {
                        if (geraldController.CheckCounterDamage(finnalDamage, enemy.transform))
                        {
                            isCauseDamage = true;

                            if (enemy.target.GetComponent<HealthHero>().IsDead())
                            {
                                enemy.ResetTarget();
                            }
                        }
                    }
                }
            }
            ,
            onComplete: () => {
                // Khi bổ củi xong, nếu mục tiêu vẫn trong tầm thì quay về hoạt ảnh Idle đứng thủ thế
                //if (!enemy.isDead && enemy.target != null && enemy.IsTargetInAttackRange())
                //{
                //    CharacterSpriteAnimator.Instance.PlayAnimationByRange(
                //        enemy.gameObject, id, prefix, idleConfig, frameRate, null, null
                //    );

                //    if(enemy.target.TryGetComponent(out HealthHero healthHero) &&
                //    enemy.target.TryGetComponent(out BaseUnitStateMachine baseUnitStateMachine) &&
                //    baseUnitStateMachine.isRunToFlag == false)
                //    {
                //        healthHero.ApplyDamage(finnalDamage, enemy.transform);

                //        if (healthHero.IsDead())
                //        {
                //            enemy.ResetTarget();
                //        }
                //    }
                //}
                if (isCauseDamage == false)
                {
                    AttackSoliderDamage(enemy, id, prefix, idleConfig, finnalDamage);
                }
                CharacterSpriteAnimator.Instance.PlayAnimationByRange(
                    enemy.gameObject, id, prefix, idleConfig, frameRate, null, null
                );
            }
        );
    }

    private void AttackSoliderDamage(EnemyController enemy, string id, string prefix, AnimationFrameRange
        idleConfig, float frameRate)
    {
        if (!enemy.isDead && enemy.target != null && enemy.IsTargetInAttackRange())
        {

            if (enemy.target.TryGetComponent(out HealthHero healthHero) &&
            enemy.target.TryGetComponent(out BaseUnitStateMachine baseUnitStateMachine) &&
            baseUnitStateMachine.isRunToFlag == false)
            {
                healthHero.ApplyDamage(finnalDamage, enemy.transform);

                if (healthHero.IsDead())
                {
                    enemy.ResetTarget();
                }
            }
        }
    }

    // Hàm Coroutine phụ trợ giúp trì hoãn việc trừ máu sao cho khớp với visual ảnh đang vung vũ khí
    private IEnumerator DelayDealDamage(float delayTime, EnemyController enemy, float damage)
    {
        yield return new WaitForSeconds(delayTime);

        // Kiểm tra lại xem lúc vũ khí chạm đất thì quái còn sống và mục tiêu còn đó không
        if (enemy != null && !enemy.isDead && enemy.target != null)
        {
            Debug.Log($"⚔️ [{enemy.name}] Vũ khí chạm mục tiêu! Gây {damage} sát thương.");

            // Đoạn này thực tế bạn sẽ gọi hàm trừ máu của mục tiêu:
            // var playerUnit = enemy.target.GetComponent<PlayerUnitBase>();
            // if(playerUnit != null) playerUnit.TakeDamage(damage);
        }
    }

    private void UpdateFacing(EnemyController enemy)
    {
        if (enemy.target == null) return;

        float dx = enemy.target.position.x - enemy.transform.position.x;
        if (dx > 0.05f) enemy.transform.localScale = enemy.unitData.localScaleRight;
        else if (dx < -0.05f) enemy.transform.localScale = enemy.unitData.localScaleLeft;
    }

    public void ExitState(EnemyController enemy)
    {
        attackTimer = 0f;
    }
}