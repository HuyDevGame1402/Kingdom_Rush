using UnityEngine;

public class GemManager : MonoBehaviour
{
    public static GemManager Instance { get; private set; }
    [SerializeField] private int gems;

    private void Awake()
    {
        Instance = this;
    }
    public void AddGem(int gemAdd)
    {
        gems += gemAdd;
    }

    public int AddGems()
    {
        return gems;
    }
}
