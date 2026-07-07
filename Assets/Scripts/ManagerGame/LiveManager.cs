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

    public void AddGold(int goldAdd)
    {
        liveGame += goldAdd;
        LiveChange?.Invoke(liveGame);
    }
    public void RemoveGold(int goldRemove)
    {
        liveGame -= goldRemove;
        if (liveGame < 0)
        {
            liveGame = 0;
        }
        LiveChange?.Invoke(liveGame);
    }

    public bool CheckLive(int goldBuy)
    {
        return liveGame >= goldBuy;
    }
}
