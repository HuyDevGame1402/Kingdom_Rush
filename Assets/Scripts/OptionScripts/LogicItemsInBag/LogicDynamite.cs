using UnityEngine;

public class LogicDynamite : MonoBehaviour, IHasLogicOption
{
    [SerializeField] private GameObject dynamitePrefab;
    [SerializeField] private BagOptionUI bagOptionUI;
    private GameObject dynamiteCreate;

    public void Execute(Vector3 pos)
    {
        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.RemoveItem(ItemType.Dynamite, 1);
        }
        dynamiteCreate = Instantiate(dynamitePrefab);
        if(dynamiteCreate.TryGetComponent(out ThrowableObject dynamite))
        {
            dynamite.InitializeFromSky(pos);
        }
        bagOptionUI.UpdateSpriteNormal();
    }

}
