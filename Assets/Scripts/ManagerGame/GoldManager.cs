using UnityEngine;
using System;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }
    private int goldGame;

    public event Action<int> GoldChange;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        goldGame = GameManager.Instance.levelData.startingGold;
        GoldChange?.Invoke(goldGame);
    }

    public void AddGold(int goldAdd)
    {
        goldGame += goldAdd;
        GoldChange?.Invoke(goldGame);
    }
    public void RemoveGold(int goldRemove)
    {
        goldGame -= goldRemove;
        if(goldGame < 0)
        {
            goldGame = 0;
        }
        GoldChange?.Invoke(goldGame);
    }

    public bool CheckGold(int goldBuy)
    {
        return goldGame >= goldBuy;
    }
    public int GetGold()
    {
        return goldGame;
    }
}
