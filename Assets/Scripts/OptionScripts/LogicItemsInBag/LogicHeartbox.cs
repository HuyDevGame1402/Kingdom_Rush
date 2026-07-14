using UnityEngine;

public class LogicHeartbox : MonoBehaviour, IHasLogicOption
{
    [SerializeField] private int liveAdd = 5;

    [SerializeField] private BagOptionUI bagOptionUI;

    public void Execute(Vector3 pos)
    {
        if (LiveManager.Instance != null)
        {
            LiveManager.Instance.AddLive(liveAdd);
        }

        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.RemoveItem(ItemType.Heartbox, 1);
        }

        bagOptionUI.UpdateSpriteNormal();
    }
}
