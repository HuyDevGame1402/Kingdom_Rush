using System;
using UnityEngine;

public class LiveManager : MonoBehaviour
{
    public static LiveManager Instance { get; private set; }
    private int liveGame;

    public event Action<int> LiveChange;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        liveGame = GameManager.Instance.levelData.live;
    }

    public void AddLive(int liveAdd)
    {
        liveGame += liveAdd;
        LiveChange?.Invoke(liveGame);
    }
    public void RemoveLive(int liveRemove)
    {
        liveGame -= liveRemove;
        if (liveGame < 0)
        {
            liveGame = 0;
        }
        LiveChange?.Invoke(liveGame);
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
