using UnityEngine;

public class TowerLevelUp : MonoBehaviour
{
    [SerializeField] private BaseTowerSO currentBaseSOTower;
    [SerializeField] private BaseTowerSO baseTowerSONextLevel;

    [SerializeField] private TowerUpLevelSO towerUpLevelSO;

    public Transform groundTower;

    public BaseTowerSO GetBaseTowerSO()
    {
        return baseTowerSONextLevel;
    }

    public void SetCurrentBaseTowerSO(BaseTowerSO baseTowerSO)
    {
        currentBaseSOTower = baseTowerSO;
    }

    public BaseTowerSO GetCurrentBaseTowerSO()
    {
        return currentBaseSOTower;
    }
    public void SetTowerLevelUpSO(TowerUpLevelSO towerLevetlUp)
    {
        towerUpLevelSO = towerLevetlUp;
    }

    public TowerUpLevelSO GetTowerLevelUpSO()
    {
        return towerUpLevelSO;
    }
}
