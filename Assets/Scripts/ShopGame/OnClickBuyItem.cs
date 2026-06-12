using UnityEngine;
using System;
using UnityEngine.UI;

public class OnClickBuyItem : MonoBehaviour
{
    [SerializeField] private ItemAbstract itemSelected;
    public Action<ItemAbstract> OnClickBuytItem;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(BuyItem);
    }

    private void BuyItem()
    {
        if(PlayerManager.Instance != null && PlayerManager.Instance.Gems >=
            itemSelected.gemCost && ShopItemManager.Instance != null)
        {
            ShopItemManager.Instance.BuyItem(int.Parse(itemSelected.itemId),
                itemSelected.gemCost);
        }
    }

    public void SetItemSelected(ItemAbstract itemSelected)
    {
        this.itemSelected = itemSelected;
    }
}
