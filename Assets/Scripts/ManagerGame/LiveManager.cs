using System;
using UnityEngine;

public class LiveManager : MonoBehaviour
{
    public static LiveManager Instance { get; private set; }
    private int liveGame;

    public event Action<int> LiveChange;
    public event Action OnGameDefeat;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        liveGame = GameManager.Instance.levelData.live;
        LiveChange?.Invoke(liveGame);
    }

    public void AddLive(int liveAdd)
    {
        liveGame += liveAdd;
        LiveChange?.Invoke(liveGame);
    }
    public void RemoveLive(int liveRemove)
    {
        if (liveGame == 0) return;
        liveGame -= liveRemove;
        if (liveGame < 0)
        {
            liveGame = 0;
        }
        if(SoundGameAttackManager.Instance != null)
        {
            SoundGameAttackManager.Instance.PlayAudioLosseLife();
        }
        LiveChange?.Invoke(liveGame);
        if (liveGame == 0)
        {
            if (LevelEnemySpawner.Instance != null)
            {
                LevelEnemySpawner.Instance.InvokeEventCalculatorGems();
            }
            OnGameDefeat?.Invoke();
        }
    }

    public bool CheckLive(int live)
    {
        return liveGame >= live;
    }

    public int GetLiveGame()
    {
        return liveGame;
    }
}
