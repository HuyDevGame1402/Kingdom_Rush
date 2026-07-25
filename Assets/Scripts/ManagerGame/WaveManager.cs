using System;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    private int waveGame;
    private int waveMax;

    public event Action<int, int> WaveChange;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        waveMax = GameManager.Instance.levelData.wave;
        WaveChange?.Invoke(waveGame, waveMax);
    }

    public void AddWave()
    {
        waveGame += 1;
        WaveChange?.Invoke(waveGame, waveMax);
    }
}
