using UnityEngine;
using System;

public class LoadingTower : MonoBehaviour
{
    [SerializeField] private TowerSoundBasic towerSoundBasic;
    [SerializeField] private float timerInitTower = 1.5f;
    [SerializeField] private float timerMaxInitTower = 1.5f;
    public event EventHandler OnCompletedInitTower;
    public event Action<float> OnInitTower;

    private bool isDelayShow = false;

    private void OnEnable()
    {
        timerInitTower = timerMaxInitTower;
        towerSoundBasic.PlayAudioTowerBuild();
    }
    private void Update()
    {

        if(timerMaxInitTower == 0f && isDelayShow == false)
        {
            towerSoundBasic.PlayAudioTowerReady();
            OnCompletedInitTower?.Invoke(this, EventArgs.Empty);
            timerInitTower = 0f;
            gameObject.SetActive(false);
        }

        if(timerInitTower > 0f)
        {
            timerInitTower -= Time.deltaTime;
            OnInitTower?.Invoke((timerMaxInitTower - timerInitTower) / timerMaxInitTower);
            if (timerInitTower <= 0f)
            {
                towerSoundBasic.PlayAudioTowerReady();
                OnCompletedInitTower?.Invoke(this, EventArgs.Empty);
                timerInitTower = 0f;
                gameObject.SetActive(false);
            }
        }
    }
}
