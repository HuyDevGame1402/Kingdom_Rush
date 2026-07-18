using UnityEngine;

public class BarrackUpdateTower : MonoBehaviour, IHasUpdateTower
{
    [SerializeField] private BarracksAnimation barracksAnimation;
    [SerializeField] private TowerLevelUp towerLevelUp;
    [SerializeField] private BarrackSpawnHero barrackSpawnHero;
     
    private void Awake()
    {
        barracksAnimation = GetComponent<BarracksAnimation>();
        towerLevelUp = GetComponent<TowerLevelUp>();
        barrackSpawnHero = GetComponent<BarrackSpawnHero>();
    }

    public void UpdateTower(TowerUpLevelSO towerLevelUpSO)
    {
        towerLevelUp.SetTowerLevelUpSO(towerLevelUpSO);
        towerLevelUp.SetCurrentBaseTowerSO(towerLevelUpSO.currentBaseTowerSO);
        barracksAnimation.SetNameTowerAnimation(towerLevelUpSO.currentCastleData.animationTower);
        barracksAnimation.PlayAnimation();

        for(int i = 0; i < barrackSpawnHero.heroSpawnList.Count; i++)
        {
            barrackSpawnHero.heroSpawnList[i].GetComponent<BaseUnitStateMachine>().unitData = 
                towerLevelUpSO.currentCastleData.heroDataSO;
            barrackSpawnHero.heroSpawnList[i].GetComponent<BaseUnitStateMachine>().ReloadAnimation();
        }

    }
}
