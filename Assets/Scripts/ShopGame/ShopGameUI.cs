using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class ShopGameUI : MonoBehaviour
{
    [SerializeField] private List<int> idListItemRight = new List<int>();
    [SerializeField] private List<OnClickButtonShopGame> onClickButtonShopGame;
    [Header("--- UI References ---")]
    [SerializeField] private Transform itemSelected;
    [SerializeField] private Transform descriptionItem;
    [SerializeField] private TextMeshProUGUI gemText;
    [SerializeField] private OnClickBuyItem onClickBuyItem;
    [SerializeField] private Transform uiItemRightShop;
    [SerializeField] private TextMeshProUGUI diamonTextMenuGame;

    private void Start()
    {
        RegisterEventOnClickItem();
        LoadGameToUI();
        if(ShopItemManager.Instance != null)
        {
            ShopItemManager.Instance.EventBuyItem += UpdateItemShopGame;
        }
        if(PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnUpdateGems += OnUpdateGems;
        }
    }
    private void OnUpdateGems(int amount)
    {
        diamonTextMenuGame.text = amount.ToString();
    }
    private void OnDestroy()
    {
        UnregisterEventOnClickItem();
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnUpdateGems -= OnUpdateGems;
        }
    }

    private void RegisterEventOnClickItem()
    {
        for(int i = 0; i < onClickButtonShopGame.Count; i++)
        {
            onClickButtonShopGame[i].OnClickItem += OnShowItem;
        }
    }

    private void UnregisterEventOnClickItem()
    {
        for (int i = 0; i < onClickButtonShopGame.Count; i++)
        {
            onClickButtonShopGame[i].OnClickItem -= OnShowItem;
        }
    }

    private void OnShowItem(Transform buttonClicked,ItemAbstract itemScriptableObject)
    {
        if(itemSelected != null)
        {
            itemSelected.GetChild(1).gameObject.SetActive(false);
        }
        itemSelected = buttonClicked.parent;
        itemSelected.GetChild(1).gameObject.SetActive(true);
        DescriptionItemUI(itemScriptableObject);
        onClickBuyItem.SetItemSelected(itemScriptableObject);
    }

    private void DescriptionItemUI(ItemAbstract itemSelected)
    {
        // name
        descriptionItem.GetChild(0).GetComponent<TextMeshProUGUI>().text = itemSelected.name;
        // cost
        descriptionItem.GetChild(1).GetComponent<TextMeshProUGUI>().text = itemSelected.gemCost.ToString();
        // description
        descriptionItem.GetChild(2).GetComponent<TextMeshProUGUI>().text = itemSelected.description;
    }

    private void LoadGameToUI()
    {
        if(PlayerManager.Instance != null)
        {
            gemText.text = PlayerManager.Instance.Gems.ToString();
        }
        LoadItemInUI();
    }
    private void UpdateItemShopGame(int itemId, int gems)
    {
        gemText.text = gems.ToString();
        diamonTextMenuGame.text = gems.ToString();
        UpdateItemShopPositionRight(itemId);
    }
    private void LoadItemInUI()
    {
        if(ShopItemManager.Instance != null)
        {
            for (int i = 0; i < idListItemRight.Count; i++)
            {
                if(ShopItemManager.Instance.GetItemCount(idListItemRight[i]) > 0)
                {
                    ShowItemShopRight(i);
                }
                else
                {
                    uiItemRightShop.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
    private void ShowItemShopRight(int index)
    {
        uiItemRightShop.GetChild(index).gameObject.SetActive(true);
        uiItemRightShop.GetChild(index).GetChild(1).GetComponent<TextMeshProUGUI>()
            .text = ShopItemManager.Instance.GetItemCount(idListItemRight[index]).ToString();
    }
    public void UpdateItemShopPositionRight(int itemId)
    {
        for(int i = 0; i < idListItemRight.Count; i++)
        {
            if (idListItemRight[i] == itemId)
            {
                ShowItemShopRight(i);
                return;
            }
        }
    }
}
