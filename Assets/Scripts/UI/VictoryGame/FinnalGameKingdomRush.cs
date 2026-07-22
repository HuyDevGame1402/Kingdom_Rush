using UnityEngine;

public class FinnalGameKingdomRush : MonoBehaviour
{
    public static FinnalGameKingdomRush Instance { get; private set; }


    [SerializeField] private int starWinGame = 0;

    private void Awake()
    {
        Instance = this;
    }

    public int GetStarWinGame()
    {
        return starWinGame;
    }

}
