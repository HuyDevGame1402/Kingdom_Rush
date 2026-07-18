using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TowerInfoView : MonoBehaviour
{
    [SerializeField] private List<OnClickChooseTower> onClickChooseTowers = new List<OnClickChooseTower>();
    [SerializeField] private OnClickUpdateTower onClickUpdateTower;

    [Header("UI Reference")]
    [SerializeField] private Transform mainUI;
    [SerializeField] private TextMeshPro towerName;
    [SerializeField] private TextMeshPro towerDes;

    [Header("UI Tower Version 1 - Long Attack Rate")]
    [SerializeField] private Transform longAttackRateTransform;
    [SerializeField] private TextMeshPro attackTxtVer1;
    [SerializeField] private SpriteRenderer spriteAttackVer1;
    [SerializeField] private TextMeshPro attackRateTxtVer1;

    [Header("UI Tower Version 2 - Close Combat")]
    [SerializeField] private Transform closeCombatTransform;
    [SerializeField] private TextMeshPro attackTxtVer2;
    [SerializeField] private TextMeshPro armorTxtVer2;
    [SerializeField] private TextMeshPro healthTxtVer2;

    private void Start()
    {
        OnRegisterEventChooseTower();
    }

    private void OnRegisterEventChooseTower()
    {
        for(int i = 0; i < onClickChooseTowers.Count; i++)
        {
            onClickChooseTowers[i].OnClickChooseTowerEvent += TowerInfoView_OnClickChooseTowerEvent;
        }
        onClickUpdateTower.OnClickChooseTowerUpdateEvent += TowerInfoView_OnClickChooseTowerEvent;
    }

    private void TowerInfoView_OnClickChooseTowerEvent(Transform arg1, BaseTowerSO arg2, bool arg3)
    {
        towerDes.text = arg2.towerDescription;
        towerName.text = arg2.towerName;
        if (arg2 is TowerSOLongRangeAttack longRangeTower)
        {
            attackRateTxtVer1.text = longRangeTower.attackRate;
            attackTxtVer1.text = longRangeTower.minAttack.ToString()
                + "-" + longRangeTower.maxAttack.ToString();
            spriteAttackVer1.sprite = longRangeTower.iconAttack;
            longAttackRateTransform.gameObject.SetActive(true);
            closeCombatTransform.gameObject.SetActive(false);
        }
        else if (arg2 is TowerSOCloseCombat closeCombatTower)
        {
            attackTxtVer2.text = closeCombatTower.minAttack.ToString()
                + "-" + closeCombatTower.maxAttack.ToString();
            healthTxtVer2.text = closeCombatTower.health.ToString();
            armorTxtVer2.text = closeCombatTower.armor;
            closeCombatTransform.gameObject.SetActive(true);
            longAttackRateTransform.gameObject.SetActive(false);
        }
        if (arg3)
        {
            mainUI.gameObject.SetActive(false);
            return;
        }
        mainUI.gameObject.SetActive(true);
        transform.position = SelectTowerManager.Instance.GetPositionDes();
    }
    public void Hide()
    {
        mainUI.gameObject.SetActive(false);
    }
}
