using UnityEngine;

public class TowerAttackState : ITowerState
{
    public void EnterState(TowerStateMachine tower)
    {
        // Thực hiện hành vi bắn đạn
        ExecuteAttack(tower);
    }

    public void UpdateState(TowerStateMachine tower)
    {
        // Sau khi bắn xong 1 viên, chuyển ngay sang trạng thái Đợi hồi chiêu nạp đạn
        tower.TransitionToState(tower.CooldownState);
    }

    public void ExitState(TowerStateMachine tower)
    {
        // Kết thúc lượt bắn
    }

    private void ExecuteAttack(TowerStateMachine tower)
    {
        if (tower.CurrentTarget == null) return;
        tower.archerTowerAnimation.Attack(tower.CurrentTarget.
            GetComponent<EnemyDataScript>().centerEnemy,
            tower);
        Debug.Log($"[Tower] Đã bắn vào quái: {tower.CurrentTarget.name}");
    }
}