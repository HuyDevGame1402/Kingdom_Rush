using UnityEngine;

public class LongShootUpdateTower : MonoBehaviour, IHasUpdateTower
{
    [SerializeField] private TowerLevelUp towerLevelUp;
    [SerializeField] private TowerStateMachine towerStateMachine;
    [SerializeField] private ArcherTowerSetupAnimation towerAnimation;

    private bool isUpdateAnimationHero;

    private void Awake()
    {
        towerLevelUp = GetComponent<TowerLevelUp>();
        towerStateMachine = GetComponent<TowerStateMachine>();
    }

    public void UpdateTower(TowerUpLevelSO towerLevelUpSO)
    {
        if(towerLevelUpSO.currentCastleData.animationHero != towerStateMachine.GetDataTower().animationHero)
        {
            isUpdateAnimationHero = true;
        }
        towerLevelUp.SetTowerLevelUpSO(towerLevelUpSO);
        towerLevelUp.SetCurrentBaseTowerSO(towerLevelUpSO.currentBaseTowerSO);

        towerStateMachine.SetCastleData(towerLevelUpSO.currentCastleData);
        towerAnimation.InitTower(
            towerStateMachine.GetDataTower().animationTower,
            towerStateMachine.GetDataTower().frameTowerStartIdle,
            towerStateMachine.GetDataTower().frameTowerEndIdle);

        if (isUpdateAnimationHero)
        {
            towerAnimation.ReloadAnimationHeroList();
        }
    }
}
