using UnityEngine;

public class TowerIdleState : ITowerState
{
    public void EnterState(TowerStateMachine tower)
    {
        tower.CurrentTarget = null;
        // Chỗ này bạn có thể bật Animation đứng yên (Idle) cho trụ nếu có
        if(tower.isCreateTower == false)
        {
            tower.archerTowerAnimation.CreateTower(tower.GetDataTower());
            tower.isCreateTower = true;
            return;
        }
        tower.archerTowerAnimation.IdleHeros();
    }

    public void UpdateState(TowerStateMachine tower)
    {
        // Liên tục quét tìm quái
        // tower.FindTarget();

        // Nếu thấy quái xuất hiện, chuyển ngay sang trạng thái Bắn (Attack)
        if (tower.CurrentTarget != null)
        {
            tower.TransitionToState(tower.AttackState);
        }
    }

    public void ExitState(TowerStateMachine tower)
    {
        Debug.Log("[Tower] Phát hiện mục tiêu! Thoát trạng thái Idle.");
    }
}