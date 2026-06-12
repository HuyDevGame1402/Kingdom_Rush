using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;

public class UpgradesUIGame : MonoBehaviour
{
    [SerializeField] private Transform[] rowUpgrades = new Transform[5];
    [SerializeField] private ButtonSelectUpgrades[] arrayButtonSelectUpgrades = new ButtonSelectUpgrades[30];

    [SerializeField] private Transform upgradeSelectButton;

    private int indexArray = 0;
    public Color white;
    public Color brown;

    [Header("Star UI")]
    [SerializeField] private TextMeshProUGUI starTotalText;

    [Header("Upgrade UI And Descriptions")]
    [SerializeField] private Image imageUpgradeDes;
    [SerializeField] private Image imageTick;
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI upgradeDesText;
    [SerializeField] private TextMeshProUGUI upgradeStarCostText;

    [SerializeField] private RectTransform upgradesUI;

    [SerializeField] private BuyUpgradesButton buyUpgradesButton;

    private void Start()
    {
        SetupButtonSelectInArray();
        SetupUpgradesDataAndUIInButton();
        RegisterEventOnClickSelectUpgrades();
        UpdateStartInUI();
        if(UpgradesManager.Instance != null)
        {
            UpgradesManager.Instance.OnUpgradePurchased += UpdateUpgradeInUI;
        }
    }
    private void OnDestroy()
    {
        UnRegisterEventOnClickSelectUpgrades();
    }

    private void UpdateStartInUI()
    {
        if (PlayerManager.Instance != null && starTotalText != null)
        {
            starTotalText.text = PlayerManager.Instance.TotalStars.ToString();
        }
    }

    private void SetupButtonSelectInArray()
    {
        for(int i = 0; i < rowUpgrades.Length; i++)
        {
            for(int j = 0; j < rowUpgrades[i].childCount; j++)
            {
                indexArray = i + j * 5;
                arrayButtonSelectUpgrades[indexArray] = rowUpgrades[i].GetChild(j).GetComponent<ButtonSelectUpgrades>();
            }
        }
    }

    private void SetupUpgradesDataAndUIInButton()
    {
        upgradesUI.localScale = Vector3.zero;
        upgradesUI.gameObject.SetActive(true);
        for (int i = 0; i < arrayButtonSelectUpgrades.Length; i++)
        {
            arrayButtonSelectUpgrades[i].Initialize(
                UpgradesManager.Instance.upgradesData[i],
                UpgradesManager.Instance.UpgradesInventory[
                    i + 1] ? white : brown,
                UpgradesManager.Instance.UpgradesInventory[i + 1]
                );
        }
        upgradesUI.localScale = Vector3.one;
        upgradesUI.gameObject.SetActive(false);
    }

    private void RegisterEventOnClickSelectUpgrades()
    {
        for(int i = 0; i < arrayButtonSelectUpgrades.Length; i++)
        {
            arrayButtonSelectUpgrades[i].OnClickSelectUpgrade += UpDateUIUpgradesSelect;
        }
    }
    private void UnRegisterEventOnClickSelectUpgrades()
    {
        for (int i = 0; i < arrayButtonSelectUpgrades.Length; i++)
        {
            arrayButtonSelectUpgrades[i].OnClickSelectUpgrade -= UpDateUIUpgradesSelect;
        }
    }

    private void UpDateUIUpgradesSelect(Transform buttonSelect, BaseUpgradeData upgradeData)
    {
        if(upgradeSelectButton != null)
        {
            // hide image selected
            upgradeSelectButton.GetChild(3).gameObject.SetActive(false);
        }
        upgradeSelectButton = buttonSelect;
        // show image selected
        upgradeSelectButton.GetChild(3).gameObject.SetActive(true);
        upgradeNameText.text = upgradeData.upgradeName;
        upgradeDesText.text = upgradeData.description;
        upgradeStarCostText.text = upgradeData.starsRequired.ToString();
        if(UpgradesManager.Instance != null)
        {
            if (UpgradesManager.Instance.UpgradesInventory[
                int.Parse(upgradeData.upgradeID)])
            {
                imageUpgradeDes.sprite = upgradeData.upgradeIcon;
                imageTick.gameObject.SetActive(true);
            }
            else
            {
                imageUpgradeDes.sprite = upgradeData.upgradeIconBrown;
                imageTick.gameObject.SetActive(false);
            }
        }
        buyUpgradesButton.SetUpgrades(upgradeData);
    }
    private void UpdateUpgradeInUI(int upgradeId, Sprite spriteUpdate)
    {
        if (upgradeSelectButton.GetComponent<ButtonSelectUpgrades>()
            .GetUpgradeID() == upgradeId)
        {
            // update button select description
            imageTick.gameObject.SetActive(true);
            imageUpgradeDes.sprite = spriteUpdate;
        }
        starTotalText.text = PlayerManager.Instance.TotalStars.ToString();
        indexArray = upgradeId - 1;
        arrayButtonSelectUpgrades[indexArray].UpdateImageButtonAfterBuyUpgrade(white);
    }
}
