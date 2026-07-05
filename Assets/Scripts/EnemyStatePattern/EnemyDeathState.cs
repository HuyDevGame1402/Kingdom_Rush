using UnityEngine;

public class EnemyDeathState : IEnemyState
{
    public void EnterState(EnemyController enemy)
    {
        enemy.isDead = true;

        if (EnemySpriteAnimator.Instance != null)
        {
            string id = enemy.unitData.unitName;
            string prefix = enemy.unitData.animations.animPrefix; // Sử dụng tiền tố động từ SO
            float frameRate = enemy.unitData.animations.frameRate;

            // Lấy nguyên cụm cấu hình hoạt ảnh chết (chứa cả list offset nếu có)
            AnimationFrameRange deathConfig = enemy.unitData.animations.death;

            // Gọi hàm PlayAnimationByRange nâng cấp
            EnemySpriteAnimator.Instance.PlayAnimationByRange(
                enemy.gameObject, id, prefix, deathConfig, frameRate,
                onComplete: () => {
                    Debug.Log($"💀 [{enemy.name}] Đã diễn xong hoạt ảnh chết. Xóa GameObject.");
                    //Object.Destroy(enemy.gameObject);
                    //enemy.gameObject.SetActive(false);
                    enemy.ShowHealthInGround();
                }
            );
        }
        else
        {
            // Phòng trường hợp không có Animator trong Scene thì xóa luôn để tránh kẹt game
            Object.Destroy(enemy.gameObject);
        }
    }

    public void UpdateState(EnemyController enemy) { }
    public void ExitState(EnemyController enemy) { }
}