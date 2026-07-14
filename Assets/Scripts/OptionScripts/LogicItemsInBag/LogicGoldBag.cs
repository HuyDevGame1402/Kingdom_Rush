using UnityEngine;

public class LogicGoldBag : MonoBehaviour, IHasLogicOption
{
    [SerializeField] private int goldAdd = 500;

    [SerializeField] private BagOptionUI bagOptionUI;

    public void Execute(Vector3 pos)
    {
        if(GoldManager.Instance != null)
        {
            GoldManager.Instance.AddGold(goldAdd);
        }

        if(ItemManager.Instance != null)
        {
            ItemManager.Instance.RemoveItem(ItemType.GoldBag, 1);
        }

        bagOptionUI.UpdateSpriteNormal();
    }
}
