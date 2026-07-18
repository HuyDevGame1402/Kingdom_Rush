using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(
    fileName = "TowerUpLevelSO",
    menuName = "TowerLevelUp"
)]
public class TowerUpLevelSO : ScriptableObject
{
    [Header("Base")]
    public BaseTowerSO currentBaseTowerSO;
    [Header("Tower Value and Frame Animation")]
    public CastleData currentCastleData;

    public List<TowerUpLevelSO> towerNextLevel = new List<TowerUpLevelSO>();
}
