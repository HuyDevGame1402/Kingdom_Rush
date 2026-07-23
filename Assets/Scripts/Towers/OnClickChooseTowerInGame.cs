using UnityEngine;

public class OnClickChooseTowerInGame : MonoBehaviour
{
    private TowerLevelUp towerLevelUp;

    [SerializeField] private float offsetY = 0.5f;

    private bool isSelected = false;
    [SerializeField] private int indexTowerUp = 0;
    [SerializeField] private bool isActiveFlagTower;    
    
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
                    .priceTower, offsetY, isActiveFlagTower);
                SelectTowerManager.Instance.GetTransformOnClickUpdateTower().SetBaseTowerSO(
                    towerLevelUp.GetBaseTowerSO());
                SelectTowerManager.Instance.GetTransformOnClickUpdateTower().SetTowerUpLevelSO(
                    towerLevelUp.GetTowerLevelUpSO().towerNextLevel[indexTowerUp]);
                SelectTowerManager.Instance.GetTransformOnClickUpdateTower().SetupTowerSelected(transform);
                SelectTowerManager.Instance.SetupGround(transform.GetComponent<TowerLevelUp>()
                    .groundTower);
                Debug.LogWarning("Có nâng cấp" + towerLevelUp.GetBaseTowerSO().name);
            }
            // k có tower nâng cấp
            else
            {
                SelectTowerManager.Instance.ActiveViewLockTower(transform, offsetY, isActiveFlagTower);
                Debug.LogWarning("Không có nâng cấp" + towerLevelUp.GetBaseTowerSO().name);
            }

            if (MapPathManager.Instance != null) MapPathManager.Instance.DisablePolygonCollider2D();
        }
        else
        {
            if (SelectTowerManager.Instance != null)
            {
                SelectTowerManager.Instance.DisableViewTower();
                SelectTowerManager.Instance.GetTransformOnClickUpdateTower().ResetOnClickUpdateTower();
                SelectTowerManager.Instance.SetupGround(null);
            }
            if (MapPathManager.Instance != null) MapPathManager.Instance.ActivePolygonCollider2D();
        }
    }
    public void SetIsSelected(bool isSelected)
    {
        this.isSelected = isSelected;
    }
}
