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
    }
    public void SetTowerSO(BaseTowerSO towerSo)
    {
        this.towerSO = towerSo;
    }
    public void SetIsSelected(bool isSelected)
    {
        this.isSelected = isSelected;
    }
}
