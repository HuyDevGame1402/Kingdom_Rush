using UnityEngine;

[CreateAssetMenu(fileName = "NewHeroData", menuName = "Kingdom Rush/Hero Data", order = 1)]
public class HeroData : ScriptableObject
{
    [Header("--- PROFILE ---")]
    public int heroID;
    public string heroName;
    [TextArea(2, 5)]
    public string description;
    public Sprite heroIcon;
    public Sprite heroIconInGame;
    public Sprite heroUISelect;
    public Sprite imageButtonLock;
    public GameObject heroPrefab; // Prefab chứa model/sprite và logic di chuyển của Hero

    [Header("--- Base Stats UI ---")]
    public int health;
    public int attackMelee;
    public int attackRanged;
    public int speed;
    public int priceHeroNumber;
    public string priceHeroText;

    [Header("--- BASE STATS ---")]
    public float moveSpeed;
    public float attackCooldown;
    public int respawnTime;
    public float healthRegenRate; // HP hồi phục mỗi giây

    public ChampionStats[] levelStats;

    [Header("--- HERO ABILITIES ---")]
    public HeroSkill[] skills; // Mảng chứa các kỹ năng đặc biệt của Hero
}

// Struct phụ trợ để định nghĩa kỹ năng của Hero
[System.Serializable]
public struct HeroSkill
{
    public string skillName;
    [TextArea(2, 4)]
    public string skillDescription;
    public Sprite skillIcon;
    public float cooldown;
    public int maxLevel;
}


[System.Serializable]
public struct ChampionStats
{
    public int level;
    public int maxHealth;
    public int minDamageMelee;
    public int maxDamageMelee;
    public int minDamageRanged;
    public int maxDamageRanged;
    public float armor; // % giáp
}