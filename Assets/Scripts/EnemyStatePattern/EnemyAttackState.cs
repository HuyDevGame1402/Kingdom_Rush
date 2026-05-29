using System.Collections;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private float attackTimer = 0f;

    public void EnterState(EnemyController enemy)
    {
        // Sẵn sàng tấn công ngay khi vừa áp sát mục tiêu
        attackTimer = enemy.unitData.attackCooldown;
    }

    public void UpdateState(EnemyController enemy)
    {
        // Nếu mục tiêu bỗng nhiên biến mất hoặc chạy ra khỏi tầm, tiếp tục di chuyển đi tiếp
        if (enemy.target == null || !enemy.IsTargetInAttackRange())
        {
            enemy.TransitionToState(enemy.MoveState);
            return;
        }

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
        if (EnemySpriteAnimator.Instance == null) return;

        // 1. Lấy dữ liệu định danh và cấu hình đòn đánh từ ScriptableObject
        string id = enemy.unitData.unitName;
        string prefix = enemy.unitData.animations.animPrefix; // Dùng prefix động thay vì fix cứng "goblin_"
        float frameRate = enemy.unitData.animations.frameRate;

        // Cụm cấu hình tấn công và thủ thế (Idle)
        AnimationFrameRange attackConfig = enemy.unitData.animations.attack;
        AnimationFrameRange idleConfig = enemy.unitData.animations.idle;

        // 2. Kích hoạt hoạt ảnh tấn công (Truyền nguyên cụm Object chứa list Offset chỉnh tay)
        EnemySpriteAnimator.Instance.PlayAnimationByRange(
            enemy.gameObject, id, prefix, attackConfig, frameRate,
            onComplete: () => {
                // Khi bổ củi xong, nếu mục tiêu vẫn trong tầm thì quay về hoạt ảnh Idle đứng thủ thế
                if (!enemy.isDead && enemy.target != null && enemy.IsTargetInAttackRange())
                {
                    EnemySpriteAnimator.Instance.PlayAnimationByRange(
                        enemy.gameObject, id, prefix, idleConfig, frameRate
                    );
                }
            }
        );

        // 3. XỬ LÝ EVENT GÂY SÁT THƯƠNG CHUẨN FRAME (Thay thế cho hàm tạo độ trễ ước lượng cũ)
        //float damageAmount = enemy.unitData.GetRandomDamage();

        //// Nếu bạn có cài đặt trúng đòn ở frame cụ thể trên Inspector (Ví dụ: eventFrame = 69)
        //if (attackConfig.hasEvent && attackConfig.eventFrame >= attackConfig.startFrame)
        //{
        //    // Tính toán xem từ lúc bấm nút đánh đến frame trúng đòn mất bao nhiêu giây
        //    int framesToWait = attackConfig.eventFrame - attackConfig.startFrame;
        //    float delayTimeToDamage = framesToWait * frameRate;

        //    // Chạy một hàm Coroutine phụ trợ ngay trên Enemy để chờ đến đúng giây đó rồi trừ máu
        //    enemy.StartCoroutine(DelayDealDamage(delayTimeToDamage, enemy, damageAmount));
        //}
        //else
        //{
        //    // Nếu không cài đặt eventFrame, mặc định gây sát thương ngay lập tức hoặc chia đôi chuỗi như cũ
        //    float fallbackDelay = ((attackConfig.endFrame - attackConfig.startFrame) * 0.5f) * frameRate;
        //    enemy.StartCoroutine(DelayDealDamage(fallbackDelay, enemy, damageAmount));
        //}
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

    public void ExitState(EnemyController enemy)
    {
        attackTimer = 0f;
    }
}