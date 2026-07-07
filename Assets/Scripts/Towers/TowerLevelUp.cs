using UnityEngine;

public class TowerLevelUp : MonoBehaviour
{
    [SerializeField] private BaseTowerSO currentBaseSOTower;
    [SerializeField] private BaseTowerSO baseTowerSONextLevel;

    public Transform groundTower;

    public BaseTowerSO GetBaseTowerSO()
    {
        return baseTowerSONextLevel;
    }

    public BaseTowerSO GetCurrentBaseTowerSO()
    {
        return currentBaseSOTower;
    }
}
