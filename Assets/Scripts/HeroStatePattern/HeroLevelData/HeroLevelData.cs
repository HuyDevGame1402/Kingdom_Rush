using UnityEngine;

// Enum quản lý cấp độ giáp
public enum ArmorType
{
    None = 0,
    Low = 30,      // 30%
    Medium = 50,   // 40% - 60%
    High = 80      // 70% - 80%
}

[System.Serializable]
public struct HeroLevelStat
{
    [Header("Level Info")]
    public int level;
    public int expToNextLevel; 

    [Header("Health")]
    public int maxHP;
    public int casualHP; // HP ở chế độ Casual (dành cho chế độ dễ hơn nếu có)

    [Header("Damage (Physical - 1.0s)")]
    public int minDamage;
    public int maxDamage;

    [Header("Armor & Defense")]
    public ArmorType armorType;
    [Range(0f, 1f)] public float armorPercentage; // Ví dụ: 0.3 = 30%, 0.8 = 80%

    [Header("Respawn")]
    public float respawnTime; // Thời gian hồi sinh (15s)
}