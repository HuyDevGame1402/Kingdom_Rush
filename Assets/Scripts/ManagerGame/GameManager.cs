using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public LevelData levelData;

    private void Awake()
    {
        Instance = this;
    }

    public bool CheckTowerLevelUp(BaseTowerSO baseTowerSO)
    {
        return levelData.allowedBaseTowers.Contains(baseTowerSO); 
    }
}
