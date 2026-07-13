using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    GoldBag,
    Heartbox,
    Frozotov,
    Dynamite,
    ChillWand,
    FatBoy
}

public class ItemManager : MonoBehaviour
{
    
    public static ItemManager Instance { get; private set; }

    [System.Serializable]
    public struct ItemData
    {
        public ItemType type;
        public int initialCount;
    }

    [SerializeField] private List<ItemData> startingItems;

    private Dictionary<ItemType, int> itemRegistry = new Dictionary<ItemType, int>();

    private void Awake()
    {
        Instance = this;
        foreach (var item in startingItems)
        {
            itemRegistry[item.type] = item.initialCount;
        }
    }

    public int GetItemCount(ItemType type)
    {
        return itemRegistry.TryGetValue(type, out int count) ? count : 0;
    }

    public void AddItem(ItemType type, int amount)
    {
        if (!itemRegistry.ContainsKey(type)) itemRegistry[type] = 0;
        itemRegistry[type] += amount;
    }

    public void RemoveItem(ItemType type, int amount)
    {
        if (itemRegistry.ContainsKey(type))
        {
            itemRegistry[type] = Mathf.Max(0, itemRegistry[type] - amount);
        }
    }
}