using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class SelectTowerManager : MonoBehaviour
{
    public static SelectTowerManager Instance { get; private set; }
    [SerializeField] private Transform grounds;
    [SerializeField] private Transform backgroundSprite;
    [SerializeField] private Transform selectTowerBase;
    [SerializeField] private Transform updateTower;
    [SerializeField] private TextMeshPro priceUpdateTower;
    [SerializeField] private OnClickBuyTower onclickBuyTowerInUpdateTower;
    [SerializeField] private Transform lockUpdateTower;
    [SerializeField] private OnClickBuyTower onclickBuyTowerInLockTower;
    [SerializeField] private Transform updateTowerLevelMax;
    [SerializeField] private bool isSelected;
    private bool isShow;
    [SerializeField] private Transform groundSelected;
    [SerializeField] private float offsetY;

    [SerializeField] private Sprite tickSelectTower;
    [SerializeField] private Sprite tickSelectTowerGray;
    [SerializeField] private List<OnClickChooseTower> onClickChooseTowers = new List<OnClickChooseTower>();
    [SerializeField] private Transform transformSelectedTower;
    private Transform towerSelected;
    private BaseTowerSO baseTowerSelected;
    [SerializeField] private Transform mainDescriptionTower;

    private Vector3 offsetRight = new Vector3(2.5f, 1, 0);
    [SerializeField] private Vector3 offsetLeft = new Vector3(-4.5f, 1, 0);

    public Transform towerTest;
    private GameObject towerCreate;
    [SerializeField] private Transform towerParent;
    private bool isBuy;

    [SerializeField] private TowerInfoBuy towerInfoBuy;

    [SerializeField] private OnClickUpdateTower onClickUpdateTower;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RegisterEventOnClickSelectGround();
        OnRegisterEventChooseTower();
        onClickUpdateTower.OnUpdateTower += UpdateTower;
    }
    private void OnDestroy()
    {
        UnRegisterEventOnClickSelectGround();
    }
    private void RegisterEventOnClickSelectGround()
    {
        for(int i = 0; i < grounds.childCount; i++)
        {
            if (grounds.GetChild(i) != null && grounds.GetChild(i).GetComponent<BuildPlot>() != null)
            {
                grounds.GetChild(i).GetComponent<BuildPlot>().OnClickBuildTower += SelectTowerManager_OnClickBuildTower;
            }
        }
    }
    private void UnRegisterEventOnClickSelectGround()
    {
        if (grounds == null) return;
        for (int i = 0; i < grounds.childCount; i++)
        {
            if(grounds.GetChild(i) != null && grounds.GetChild(i).GetComponent<BuildPlot>() != null)
            {
                grounds.GetChild(i).GetComponent<BuildPlot>().OnClickBuildTower -= SelectTowerManager_OnClickBuildTower;
            }
        }
    }

    private void OnRegisterEventChooseTower()
    {
        for (int i = 0; i < onClickChooseTowers.Count; i++)
        {
            onClickChooseTowers[i].OnClickChooseTowerEvent += SelectTowerManager_OnClickChooseTowerEvent; ;
        }
    }

    private void SelectTowerManager_OnClickChooseTowerEvent(Transform arg1, BaseTowerSO arg2, bool arg3)
    {
        DisableTowerInfoBuy();
        if (transformSelectedTower != null)
        {
            isBuy = SetIsBuy(transformSelectedTower);
            if (isBuy)
            {
                transformSelectedTower.GetComponent<SpriteRenderer>().sprite = baseTowerSelected.towerIcon;
            }
            else
            {
                transformSelectedTower.GetComponent<SpriteRenderer>().sprite = baseTowerSelected.towerIconGray;
            }
            
        }
        if(transformSelectedTower != arg1 && transformSelectedTower != null)
        {
            transformSelectedTower.GetComponent<OnClickChooseTower>().SetIsSelected(false);
        }
        if (arg3 && SetIsBuy(arg1))
        {
            towerCreate = Instantiate(arg2.towerPrefab, towerParent);
            towerCreate.transform.position = groundSelected.position + arg2.offsetPositionSpawnTower;
            
            if(towerCreate.TryGetComponent(out BarrackSpawnHero barrackSpawnHero))
            {
                barrackSpawnHero.SetTargetSpawn(groundSelected.GetChild(2));
            }
            towerCreate.GetComponent<TowerLevelUp>().groundTower = groundSelected;
            groundSelected.GetComponent<BuildPlot>().DisableCapsualCollider();
            // Remove Bountry
            if (GoldManager.Instance != null)
            {
                GoldManager.Instance.RemoveGold(arg2.priceTower);
            }

            Hide();
            groundSelected = null;
            return;
        }
        transformSelectedTower = arg1;
        baseTowerSelected = arg2;
        isBuy = SetIsBuy(transformSelectedTower);
        if (isBuy)
        {
            transformSelectedTower.GetComponent<SpriteRenderer>().sprite = tickSelectTower;
        }
        else
        {
            transformSelectedTower.GetComponent<SpriteRenderer>().sprite = tickSelectTowerGray;
        }
        transformSelectedTower.GetComponent<OnClickChooseTower>().SetIsSelected(true);
    }

    private void UpdateTower(Transform towerSelected ,BaseTowerSO baseTowerSO, 
        TowerUpLevelSO currentTowerUpLevelSO)
    {

        if(towerSelected.TryGetComponent(out IHasUpdateTower updateTowerScript))
        {
            updateTowerScript.UpdateTower(currentTowerUpLevelSO);
        }
        else
        {
            towerCreate = Instantiate(baseTowerSO.towerPrefab, towerParent);
            towerCreate.transform.position = groundSelected.position + baseTowerSO.offsetPositionSpawnTower;

            if (towerCreate.TryGetComponent(out BarrackSpawnHero barrackSpawnHero))
            {
                barrackSpawnHero.SetTargetSpawn(groundSelected.GetChild(2));
            }
            towerCreate.GetComponent<TowerLevelUp>().groundTower = groundSelected;
        }
        // Remove Bountry
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.RemoveGold(baseTowerSO.priceTower);
        }
        Hide();
        groundSelected = null;
        return;
    }

    private bool SetIsBuy(Transform transformSelectedTower)
    {
        return GoldManager.Instance.CheckGold(transformSelectedTower.GetComponent<
                OnClickChooseTower>().GetTowerSO().priceTower);
    }

    private void SelectTowerManager_OnClickBuildTower(Transform arg1, bool arg2)
    {
        onClickUpdateTower.ResetOnClickUpdateTower();
        DisableTowerInfoBuy();
        backgroundSprite.gameObject.SetActive(false);
        selectTowerBase.gameObject.SetActive(false);
        updateTower.gameObject.SetActive(false);
        lockUpdateTower.gameObject.SetActive(false);
        if (groundSelected == arg1)
        {
            Hide();
            groundSelected = null;
            if (MapPathManager.Instance != null) MapPathManager.Instance.ActivePolygonCollider2D();
        }
        else
        {
            groundSelected = arg1;
            ResetSelectTower();
        }
        if (arg2 == false && groundSelected != null)
        {
            ActiveViewSelectedInit(true);
            transform.position = arg1.transform.position + new Vector3(0, offsetY, 0);
            if (MapPathManager.Instance != null) MapPathManager.Instance.DisablePolygonCollider2D();
        }
    }
    private void ActiveViewSelectedInit(bool isActive)
    {
        backgroundSprite.gameObject.SetActive(isActive);
        selectTowerBase.gameObject.SetActive(isActive);
    }

    public void ActiveViewUpdateTower(Transform tower ,int priceUpdate, float offsetY)
    {
        Hide();
        backgroundSprite.gameObject.SetActive(true);
        updateTower.gameObject.SetActive(true);
        priceUpdateTower.text = priceUpdate.ToString();
        towerSelected = tower;
        transform.position = tower.transform.position + new Vector3(0, offsetY, 0);
        groundSelected = null;
        onclickBuyTowerInUpdateTower.tower = tower;
    }

    public void ActiveViewLockTower(Transform tower, float offsetY)
    {
        Hide();
        backgroundSprite.gameObject.SetActive(true);
        lockUpdateTower.gameObject.SetActive(true);
        towerSelected = tower;
        transform.position = tower.transform.position + new Vector3(0, offsetY, 0);
        groundSelected = null;
        onclickBuyTowerInLockTower.tower = tower;
    }
    public void DisableTowerInfoBuy()
    {
        onclickBuyTowerInLockTower.SetConfirmSale(false);
        onclickBuyTowerInUpdateTower.SetConfirmSale(false);
        onclickBuyTowerInLockTower.SetupSpriteMoney();
        onclickBuyTowerInUpdateTower.SetupSpriteMoney();
        towerInfoBuy.ActiveChildCount(false);
    }
    public void DisableViewTower()
    {
        Hide();
    }

    private void Hide()
    {
        backgroundSprite.gameObject.SetActive(false);
        selectTowerBase.gameObject.SetActive(false);
        updateTower.gameObject.SetActive(false);
        lockUpdateTower.gameObject.SetActive(false);
        ResetSelectTower();
    }

    private void ResetSelectTower()
    {
        if (mainDescriptionTower != null)
        {
            mainDescriptionTower.gameObject.SetActive(false);
        }
        if (transformSelectedTower != null)
        {
            transformSelectedTower.GetComponent<OnClickChooseTower>().SetIsSelected(false);
            transformSelectedTower.GetComponent<SpriteRenderer>().sprite = baseTowerSelected.towerIcon;
        }
        if(towerSelected != null)
        {
            towerSelected.GetComponent<OnClickChooseTowerInGame>().SetIsSelected(false);
        }
        towerSelected = null;
        transformSelectedTower = null;
        baseTowerSelected = null;
    }
    public Vector3 GetPositionDes()
    {
        if (groundSelected == null) return Vector3.zero;
        if(groundSelected.position.x > 3)
        {
            return groundSelected.position + offsetLeft;
        }
        return groundSelected.position + offsetRight;
    }

    public OnClickUpdateTower GetTransformOnClickUpdateTower()
    {
        return onClickUpdateTower;
    }

    public void SetupGround(Transform ground)
    {
        groundSelected = ground;
    }
}
