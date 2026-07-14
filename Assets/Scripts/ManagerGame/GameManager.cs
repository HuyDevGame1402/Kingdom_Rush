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
    [SerializeField] private LayerMask pathLayer;
    private Vector2 mousePos;
    private Collider2D hit;
    public GameObject frozotov;
    public GameObject dynamite;
    private GameObject test;
    public bool isFrozotovTest = true;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            hit = Physics2D.OverlapPoint(mousePos, pathLayer);

            if (hit != null)
            {
                if (isFrozotovTest)
                {
                    test = Instantiate(frozotov, mousePos, Quaternion.identity);
                }
                else
                {
                    test = Instantiate(dynamite, mousePos, Quaternion.identity);
                }
                test.GetComponent<ThrowableObject>().InitializeFromSky(mousePos);
            }
            else
            {
                Debug.Log("Không phải đường");
            }
        }
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
