using UnityEngine;

public abstract class BaseUpgradeData : ScriptableObject
{
    [Header("--- UPGRADE INFO ---")]
    public string upgradeID;          // ID duy nhất của nâng cấp (vd: Archer_1, Mage_3)
    public string upgradeName;        // Tên hiển thị nâng cấp
    [TextArea(3, 6)]
    public string description;        // Mô tả hiệu ứng nâng cấp
    public Sprite upgradeIcon;        // Icon hiển thị trên cây nâng cấp
    public Sprite upgradeIconBrown;

    [Header("--- COST ---")]
    public int starsRequired;         // Số lượng Star cần để nâng cấp cấp độ này

    /// <summary>
    /// Hàm abstract bắt buộc các class con phải override để thực thi logic nâng cấp cụ thể.
    /// Bạn có thể truyền thêm các tham số hệ thống nếu cần quản lý.
    /// </summary>
    public abstract void ApplyUpgrade();
}