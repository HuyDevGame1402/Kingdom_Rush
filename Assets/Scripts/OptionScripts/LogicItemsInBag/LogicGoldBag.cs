using UnityEngine;

public class LogicGoldBag : MonoBehaviour, IHasLogicOption
{
    [SerializeField] private int goldAdd = 500;

    [SerializeField] private BagOptionUI bagOptionUI;

    [SerializeField] private AudioClip inappGem;
    [SerializeField] private AudioSource audioSource;

    public void Execute(Vector3 pos)
    {
        if(GoldManager.Instance != null)
        {
            GoldManager.Instance.AddGold(goldAdd);
        }
        audioSource.PlayOneShot(inappGem);
        if(ItemManager.Instance != null)
        {
            ItemManager.Instance.RemoveItem(ItemType.GoldBag, 1);
        }

        bagOptionUI.UpdateSpriteNormal();
    }
}
