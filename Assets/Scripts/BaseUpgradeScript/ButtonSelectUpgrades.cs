using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class ButtonSelectUpgrades : MonoBehaviour
{
    [SerializeField] private BaseUpgradeData upgrade;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI starCost;
    [SerializeField] private Image imageStar;

    public event Action<Transform , BaseUpgradeData> OnClickSelectUpgrade;

    private void Awake()
    {
        imageStar = transform.GetChild(1).GetComponent<Image>();
        starCost = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        transform.GetComponent<Button>().onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        OnClickSelectUpgrade?.Invoke(transform, upgrade);
    }

    public void Initialize(BaseUpgradeData upgradeData, Color color, bool isOwn)
    {
        upgrade = upgradeData;
        UpdateUI(upgradeData.starsRequired, color, isOwn);
    }
    private void UpdateUI(int star, Color color, bool isOwn)
    {
        starCost.text = star.ToString();
        imageStar.color = color;
        if (isOwn)
        {
            transform.GetComponent<Image>().sprite = upgrade.upgradeIcon;
        }
        else
        {
            transform.GetComponent<Image>().sprite = upgrade.upgradeIconBrown;
        }
    }
    public int GetUpgradeID()
    {
        if (upgrade != null)
        {
            return int.Parse(upgrade.upgradeID);
        }
        return -1; 
    }
    public void UpdateImageButtonAfterBuyUpgrade(Color colorStar)
    {
        transform.GetComponent<Image>().sprite = upgrade.upgradeIcon;
        imageStar.color = colorStar;
        starCost.color = colorStar;
    }
}
