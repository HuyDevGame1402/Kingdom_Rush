using UnityEngine;
using System;

public class OnClickChooseTower : MonoBehaviour
{
    [SerializeField] private BaseTowerSO towerSO;
    public event Action<Transform, BaseTowerSO, bool> OnClickChooseTowerEvent;
    private bool isSelected;
    private void OnMouseDown()
    {
        OnClickChooseTowerEvent?.Invoke(transform, towerSO, isSelected);
        //isSlected = !isSlected;
        if(isSelected == true && SoundInGameManager.Instance != null)
        {
            SoundInGameManager.Instance.PlayMouseOverTowerIcon();
        }
    }
    public void SetTowerSO(BaseTowerSO towerSo)
    {
        this.towerSO = towerSo;
    }
    public BaseTowerSO GetTowerSO()
    {
        return this.towerSO;
    }
    public void SetIsSelected(bool isSelected)
    {
        this.isSelected = isSelected;
    }
}
