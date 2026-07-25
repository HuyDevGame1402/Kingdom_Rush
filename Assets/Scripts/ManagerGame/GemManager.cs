using UnityEngine;
using System;

public class GemManager : MonoBehaviour
{
    public static GemManager Instance { get; private set; }
    [SerializeField] private int gems;

    public event Action<int> OnEndGameGemReward;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(LevelEnemySpawner.Instance != null)
        {
            LevelEnemySpawner.Instance.CalculatorGemEvent += CalculatorGemFinnalGame;
        }
    }

    public void AddGem(int gemAdd)
    {
        gems += gemAdd;
    }

    public int AddGems()
    {
        return gems;
    }

    public void CalculatorGemFinnalGame(int gemDefault)
    {
        if(gems < gemDefault && LiveManager.Instance != null && LiveManager.Instance.GetLiveGame() > 0)
        {
            gems = gemDefault;
        }
        OnEndGameGemReward?.Invoke(gems);
    }
}
