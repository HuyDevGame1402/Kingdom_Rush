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
        if(GameManager.Instance == null || SelectTowerManager.Instance == null)
        {
            return;
        }
        isSelected = !isSelected;

        if (isSelected)
        {
            // có tower để nâng cấp
            if (GameManager.Instance.CheckTowerLevelUp(towerLevelUp.GetBaseTowerSO()))
            {
                SelectTowerManager.Instance.ActiveViewUpdateTower(transform, towerLevelUp.GetBaseTowerSO()
                    .priceTower, offsetY);
            }
            // k có tower nâng cấp
            else
            {
                SelectTowerManager.Instance.ActiveViewLockTower(transform, offsetY);
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
