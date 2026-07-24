using UnityEngine;
using System;

public class FinnalGameKingdomRush : MonoBehaviour
{
    public static FinnalGameKingdomRush Instance { get; private set; }

    [SerializeField] private int starWinGame = 0;

    public event Action OnCompleteCalculatorStarGame;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(LevelEnemySpawner.Instance != null)
        {
            LevelEnemySpawner.Instance.OnFinalGameKingdomRush += WinGame;
        }
    }

    public int GetStarWinGame()
    {
        return starWinGame;
    }

    public void SetStarWinGame(int starWinGame)
    {
        this.starWinGame = starWinGame;
    }

    private void WinGame()
    {
        CalculatorStarGame();
        OnCompleteCalculatorStarGame?.Invoke();
    }


    private void CalculatorStarGame()
    {
        if(LiveManager.Instance != null)
        {
            if(LiveManager.Instance.GetLiveGame() >= 18)
            {
                starWinGame = 3;
                return;
            }
            else if(LiveManager.Instance.GetLiveGame() >= 6)
            {
                starWinGame = 2;
                return;
            }
            else if (LiveManager.Instance.GetLiveGame() >= 1)
            {
                starWinGame = 1;
                return;
            }
        }
    }

}
