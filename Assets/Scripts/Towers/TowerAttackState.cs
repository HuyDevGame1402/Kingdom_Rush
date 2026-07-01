using UnityEngine;

public class TowerAttackState : ITowerState
{
    private bool isAttackAnimationFinished;

    public void EnterState(TowerStateMachine tower)
    {
        isAttackAnimationFinished = false;
        ExecuteAttack(tower);
    }

    public void UpdateState(TowerStateMachine tower)
    {
        // CHỈ chuyển sang Cooldown khi hoạt ảnh bắn đã hoàn thành xong xuôi
        if (isAttackAnimationFinished)
        {
            tower.TransitionToState(tower.CooldownState);
        }
    }

    public void ExitState(TowerStateMachine tower)
    {
        // Reset lại biến trạng thái khi thoát
        isAttackAnimationFinished = false;
    }

    private void ExecuteAttack(TowerStateMachine tower)
    {
        if (tower.CurrentTarget == null)
        {
            isAttackAnimationFinished = true; // Không có quái thì hoàn thành luôn
            return;
        }

        // Truyền một callback vào để báo hiệu khi nào Animation chạy xong hoàn toàn
        tower.archerTowerAnimation.Attack(
            tower.CurrentTarget.GetComponent<EnemyDataScript>().centerEnemy,
            tower,
            () => { isAttackAnimationFinished = true; }, // Hành động chạy khi xong animation
            DamageStatic.GetDamageBase(tower.GetDataTower().minDamage, tower.GetDataTower().maxDamage)
        );

        Debug.Log($"[Tower] Đã bắt đầu hoạt ảnh bắn vào quái: {tower.CurrentTarget.name}");
    }
}
