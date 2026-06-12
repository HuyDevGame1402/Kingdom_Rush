using UnityEngine;
using System;

public class LoadingTower : MonoBehaviour
{

    [SerializeField] private float timerInitTower = 1.5f;
    [SerializeField] private float timerMaxInitTower = 1.5f;
    public event EventHandler OnCompletedInitTower;
    public event Action<float> OnInitTower;

    private void OnEnable()
    {
        timerInitTower = timerMaxInitTower;
    }
    private void Update()
    {
        if(timerInitTower > 0f)
        {
            timerInitTower -= Time.deltaTime;
            OnInitTower?.Invoke((timerMaxInitTower - timerInitTower) / timerMaxInitTower);
            if (timerInitTower <= 0f)
            {
                OnCompletedInitTower?.Invoke(this, EventArgs.Empty);
                timerInitTower = 0f;
                gameObject.SetActive(false);
            }
        }
    }
}
