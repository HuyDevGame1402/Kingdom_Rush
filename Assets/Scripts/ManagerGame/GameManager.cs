using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public LevelData levelData;

    public GameState currentGameState;

    public event Action<GameState> GameStateChanged;

    public enum GameState
    {
        Instruction,
        Prepare,
        Playing, 
        FinishLevel,
    }

    private void Awake()
    {
        Instance = this;
    }

    public bool CheckTowerLevelUp(BaseTowerSO baseTowerSO)
    {
        return levelData.allowedBaseTowers.Contains(baseTowerSO); 
    }

    public void SetState(GameState state)
    {
        currentGameState = state;
        GameStateChanged?.Invoke(currentGameState);
    }

}
