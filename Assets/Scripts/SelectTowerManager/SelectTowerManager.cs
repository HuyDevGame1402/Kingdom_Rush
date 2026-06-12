using UnityEngine;
using System.Collections.Generic;

public class SelectTowerManager : MonoBehaviour
{
    public static SelectTowerManager Instance { get; private set; }
    [SerializeField] private Transform grounds;
    [SerializeField] private Transform backgroundSprite;
    [SerializeField] private Transform selectTowerBase;
    [SerializeField] private Transform updateTower;
    [SerializeField] private Transform lockUpdateTower;
    [SerializeField] private Transform updateTowerLevelMax;
    [SerializeField] private bool isSelected;
    private bool isShow;
    private Transform groundSelected;
    [SerializeField] private float offsetY;

    [SerializeField] private Sprite tickSelectTower;
    [SerializeField] private List<OnClickChooseTower> onClickChooseTowers = new List<OnClickChooseTower>();
    private Transform transformSelectedTower;
    private BaseTowerSO baseTowerSelected;
    [SerializeField] private Transform mainDescriptionTower;

    private Vector3 offsetRight = new Vector3(2.5f, 1, 0);
    [SerializeField] private Vector3 offsetLeft = new Vector3(-4.5f, 1, 0);

    public Transform towerTest;
    private GameObject towerCreate;
    [SerializeField] private Transform towerParent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RegisterEventOnClickSelectGround();
        OnRegisterEventChooseTower();
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
        if(transformSelectedTower != null)
        {
            transformSelectedTower.GetComponent<SpriteRenderer>().sprite = baseTowerSelected.towerIcon;
        }
        if(transformSelectedTower != arg1 && transformSelectedTower != null)
        {
            transformSelectedTower.GetComponent<OnClickChooseTower>().SetIsSelected(false);
        }
        if (arg3)
        {
            towerCreate = Instantiate(arg2.towerPrefab, towerParent);
            towerCreate.transform.position = groundSelected.position + arg2.offsetPositionSpawnTower;
            Hide();
            groundSelected = null;
            return;
        }
        transformSelectedTower = arg1;
        baseTowerSelected = arg2;
        transformSelectedTower.GetComponent<SpriteRenderer>().sprite = tickSelectTower;
        transformSelectedTower.GetComponent<OnClickChooseTower>().SetIsSelected(true);
    }

    private void SelectTowerManager_OnClickBuildTower(Transform arg1, bool arg2)
    {
        if (groundSelected == arg1)
        {
            Hide();
            groundSelected = null;
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
        }
    }
    private void ActiveViewSelectedInit(bool isActive)
    {
        backgroundSprite.gameObject.SetActive(isActive);
        selectTowerBase.gameObject.SetActive(isActive);
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
}
