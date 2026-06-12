using UnityEngine;
using System;
using UnityEngine.UI;

public class BuyUpgradesButton : MonoBehaviour
{
    [SerializeField] private BaseUpgradeData upgradesSelect;

    public void OnClickBuyUpgrades()
    {
        if(UpgradesManager.Instance != null)
        {
            UpgradesManager.Instance.UpgradeSkill(int.Parse(upgradesSelect.upgradeID), 
                upgradesSelect.starsRequired);
        }
    }
    public void SetUpgrades(BaseUpgradeData upgradesData)
    {
        if (upgradesData != null)
        {
            upgradesSelect = upgradesData;
        }
    }
}
