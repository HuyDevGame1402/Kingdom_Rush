using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    public void EnterState(EnemyController enemy)
    {
        // Khi bị đóng băng hoặc ép đứng im, lập tức kích hoạt hoạt ảnh Idle thủ thế
        if (EnemySpriteAnimator.Instance != null)
        {
            string id = enemy.unitData.unitName;
            string prefix = enemy.unitData.animations.animPrefix;
            float frameRate = enemy.unitData.animations.frameRate;
            AnimationFrameRange idleConfig = enemy.unitData.animations.idle;

            // Chạy hoạt ảnh đứng im dựa trên cấu hình ScriptableObject của quái
            EnemySpriteAnimator.Instance.PlayAnimationByRange(
                enemy.gameObject, id, prefix, idleConfig, frameRate
            );
        }

        // Reset lại hướng ghi nhớ hoạt ảnh để khi quái di chuyển trở lại sẽ cập nhật ngay lập tức
        enemy.ResetAnimDirection();
    }

    public void UpdateState(EnemyController enemy)
    {
        // Trạng thái đứng im / đóng băng hoàn toàn: 
        // KHÔNG di chuyển (HandleMovement)
        // KHÔNG xử lý mục tiêu (target)
        // Đóng băng toàn bộ logic cho đến khi có một ngoại lực hoặc hệ thống hiệu ứng 
        // chuyển trạng thái của quái sang State khác (như MoveState).
    }

    public void ExitState(EnemyController enemy)
    {
        // Thực hiện dọn dẹp logic khi quái hết bị đóng băng (nếu cần)
    }
}