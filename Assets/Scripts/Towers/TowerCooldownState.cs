using UnityEngine;

public class TowerCooldownState : ITowerState
{
    private float cooldownTimer;

    public void EnterState(TowerStateMachine tower)
    {
        // Đặt đồng hồ đếm ngược bằng thời gian hồi của Trụ
        cooldownTimer = tower.GetDataTower().attackRate;
    }

    public void UpdateState(TowerStateMachine tower)
    {
        // Trừ thời gian theo thời gian thực
        cooldownTimer -= Time.deltaTime;

        // Khi hết thời gian nạp đạn
        if (cooldownTimer <= 0f)
        {
            if (tower.CurrentTarget != null)
            {
                // Nếu quái vẫn nằm trong tầm bắn, quay lại bắn tiếp viên tiếp theo
                tower.TransitionToState(tower.AttackState);
                Debug.Log("Attack Tiếp tục");
            }
            else
            {
                // Nếu không còn quái, quay về trạng thái đứng đợi quét mục tiêu mới
                tower.TransitionToState(tower.IdleState);
            }
        }
    }

    public void ExitState(TowerStateMachine tower)
    {
        // Đạn đã nạp xong, sẵn sàng hành động
    }
}