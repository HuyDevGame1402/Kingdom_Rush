using UnityEngine;

public class OnClickChooseTowerInGame : MonoBehaviour
{
    private TowerLevelUp towerLevelUp;

    [SerializeField] private float offsetY = 0.5f;

    private bool isSelected = false;

    private void Awake()
    {
        towerLevelUp = GetComponent<TowerLevelUp>();
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance == null || SelectTowerManager.Instance == null)
        {
            return;
        }
        if (SoundInGameManager.Instance != null)
        {
            SoundInGameManager.Instance.PlaySoundOpenTowerMenu();
        }
        if (SelectTowerManager.Instance != null)
        {
            SelectTowerManager.Instance.DisableTowerInfoBuy();
        }
        isSelected = !isSelected;

        if (isSelected)
        {
            // có tower để nâng cấp
            if (GameManager.Instance.CheckTowerLevelUp(towerLevelUp.GetBaseTowerSO()))
            {
                SelectTowerManager.Instance.ActiveViewUpdateTower(transform, towerLevelUp.GetBaseTowerSO()
                    .priceTower, offsetY);
                Debug.LogWarning("Có nâng cấp" + towerLevelUp.GetBaseTowerSO().name);
            }
            // k có tower nâng cấp
            else
            {
                SelectTowerManager.Instance.ActiveViewLockTower(transform, offsetY);
                Debug.LogWarning("Không có nâng cấp" + towerLevelUp.GetBaseTowerSO().name);
            }
        }
        else
        {
            if (SelectTowerManager.Instance != null)
            {
                SelectTowerManager.Instance.DisableViewTower();
            }
        }
    }
    public void SetIsSelected(bool isSelected)
    {
        this.isSelected = isSelected;
    }
}
