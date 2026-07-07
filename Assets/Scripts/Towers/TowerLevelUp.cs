using UnityEngine;

public class TowerLevelUp : MonoBehaviour
{
    [SerializeField] private BaseTowerSO baseTowerSO;

    public Transform groundTower;

    public BaseTowerSO GetBaseTowerSO()
    {
        return baseTowerSO;
    }
}
