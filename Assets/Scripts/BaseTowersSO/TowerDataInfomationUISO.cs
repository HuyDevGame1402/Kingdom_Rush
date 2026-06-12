using UnityEngine;

public enum DataUIVersion
{
    Version1,
    Version2,
}

public class TowerDataInfomationUISO : ScriptableObject
{
    public DataUIVersion versionDataUI;

    [Header("Basic Information")]
    public string towerName;
    public int idTower;
    public Sprite imageSmall;
    public Sprite imageLarge;

    [TextArea(3, 10)]
    public string description;

    [Header("Combat Stats")]
    public float minAttackRange; // Tầm tấn công tối thiểu (min tc)
    public float maxAttackRange; // Tầm tấn công tối đa (max tc)

    [Header("Special Abilities")]
    public string specials; // Các đặc tính/kỹ năng đặc biệt (kiểu chuỗi)
}
