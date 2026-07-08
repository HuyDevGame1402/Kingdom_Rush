using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class OnClickMonsterGuide : MonoBehaviour, IPointerClickHandler
{
    public event Action OnClickMonsterGuideUI;
    public event Action SpawnMonsterGuideUI;

    private bool confirmNextWave = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(confirmNextWave == false)
        {
            confirmNextWave = true;
            OnClickMonsterGuideUI?.Invoke();
            // Sound

            if(SoundInGameManager.Instance != null)
            {
                SoundInGameManager.Instance.PlayMouseOverTowerIcon();
            }
        }
        else
        {
            confirmNextWave = false;
            // Sound

            if (SoundInGameManager.Instance != null)
            {
                SoundInGameManager.Instance.PlayWaveComming();
            }
            SpawnMonsterGuideUI?.Invoke();
        }
        
    }
    public void SetConfirmNextWave(bool confirmNextWave)
    {
        this.confirmNextWave = confirmNextWave;
    }
}
