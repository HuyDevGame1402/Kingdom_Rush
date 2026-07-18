using System;
using UnityEngine;

public class OnClickUpdateTower : MonoBehaviour
{
    [SerializeField] private BaseTowerSO currentTowerSO;
    [SerializeField] private TowerUpLevelSO currentTowerUpLevelSO;
    [SerializeField] private bool isSelected;

    public event Action<Transform, BaseTowerSO, bool> OnClickChooseTowerUpdateEvent;
    public event Action<Transform, BaseTowerSO, TowerUpLevelSO> OnUpdateTower;

    [SerializeField] private Transform updateTowerTransform;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite update;
    [SerializeField] private Sprite tick;

    [SerializeField] private Transform towerSelected;

    private void OnMouseDown()
    {
        OnClickChooseTowerUpdateEvent?.Invoke(transform, currentTowerSO, isSelected);
        if (isSelected == true && SoundInGameManager.Instance != null)
        {
            SoundInGameManager.Instance.PlayMouseOverTowerIcon();
        }

        if(isSelected == false)
        {
            spriteRenderer.sprite = tick;
        }

        if (isSelected)
        {
            ResetOnClickUpdateTower();

            if(towerSelected != null && towerSelected.GetComponent<IHasUpdateTower>() == null)
            {
                towerSelected.gameObject.SetActive(false);
            }
            OnUpdateTower?.Invoke(towerSelected, currentTowerSO, currentTowerUpLevelSO);
            return;
        }

        isSelected = !isSelected;
    }

    public void SetBaseTowerSO(BaseTowerSO baseTowerSO)
    {
        currentTowerSO = baseTowerSO;
    }

    public void SetTowerUpLevelSO(TowerUpLevelSO towerUpLevelSO)
    {
        currentTowerUpLevelSO = towerUpLevelSO;
    }

    public void ResetOnClickUpdateTower()
    {
        isSelected = false;
        spriteRenderer.sprite = update;
        updateTowerTransform.gameObject.SetActive(false);
    }

    public void SetupTowerSelected(Transform towerSelected)
    {
        this.towerSelected = towerSelected;
    }
}
