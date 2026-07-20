using UnityEngine;

public class LogicHeartbox : MonoBehaviour, IHasLogicOption
{
    [SerializeField] private int liveAdd = 5;

    [SerializeField] private BagOptionUI bagOptionUI;
    [SerializeField] private AudioClip inappHeart;
    [SerializeField] private AudioSource audioSource;

    public void Execute(Vector3 pos)
    {
        if (LiveManager.Instance != null)
        {
            LiveManager.Instance.AddLive(liveAdd);
        }
        audioSource.PlayOneShot(inappHeart);
        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.RemoveItem(ItemType.Heartbox, 1);
        }

        bagOptionUI.UpdateSpriteNormal();
    }
}
