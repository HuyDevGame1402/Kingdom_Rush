using UnityEngine;

public class LogicFrozotov : MonoBehaviour, IHasLogicOption
{
    [SerializeField] private GameObject frozotovPrefab;
    [SerializeField] private BagOptionUI bagOptionUI;
    private GameObject frozotovCreate;

    public void Execute(Vector3 pos)
    {
        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.RemoveItem(ItemType.Frozotov, 1);
        }
        frozotovCreate = Instantiate(frozotovPrefab);
        if (frozotovCreate.TryGetComponent(out ThrowableObject frozotovScript))
        {
            frozotovScript.InitializeFromSky(pos);
        }
        bagOptionUI.UpdateSpriteNormal();
    }
}
