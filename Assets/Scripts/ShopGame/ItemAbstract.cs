using UnityEngine;

public abstract class ItemAbstract : ScriptableObject
{
    [Header("Thông số cơ bản")]
    public string itemId;
    public string itemName;
    [TextArea(2, 5)] public string description;
    public int gemCost; 
    public Sprite itemIcon;

    
    public virtual bool Buy()
    {
        Debug.Log($"[SHOP] Đang thực hiện mua: {itemName} tiêu tốn {gemCost} kim cương.");
        return true;
    }

    public abstract void Use(Vector3 spawnPosition);
}