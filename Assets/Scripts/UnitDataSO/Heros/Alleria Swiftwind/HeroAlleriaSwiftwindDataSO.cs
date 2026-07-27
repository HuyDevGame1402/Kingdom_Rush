using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#region Skill Data Structures
[Serializable]
public struct MultishotSkillLevelData
{
    public int abilityLevel;     // Cấp chiêu (1, 2, 3)
    public int requiredHeroLevel; // Cấp Hero mở khóa (2, 5, 8)
    public int arrowCount;        // Số lượng mũi tên (3, 4, 5)
}

[Serializable]
public struct WildcatStat
{
    public int abilityLevel;     // Cấp chiêu (1, 2, 3)
    public int requiredHeroLevel; // Cấp Hero mở khóa (4, 7, 10)

    [Header("Health")]
    public int maxHP;
    public int casualHP;
    public int flashHP;

    [Header("Damage (Physical - 0.8s)")]
    public int minDamage;
    public int maxDamage;

    [Header("Defense & Respawn")]
    public ArmorType armorType;  // None
    public float respawnTime;    // 20s
}
#endregion

[CreateAssetMenu(fileName = "NewHeroAlleriaSwiftwindData", menuName = "KingdomRush/Hero Alleria Data")]
public class HeroAlleriaSwiftwindDataSO : UnitDataSO
{
    [Header("Hero Specific Animations")]
    public AlleriaSwiftwindAnimationConfig heroAnimations;

    [Header("Skill 1: Multishot Config")]
    public float multishotCooldown = 4f;
    public List<MultishotSkillLevelData> multishotLevels = new List<MultishotSkillLevelData>();

    [Header("Skill 2: Call of the Wild (Wildcat Config)")]
    public float wildcatRespawnTime = 20f;
    public List<WildcatStat> wildcatStats = new List<WildcatStat>();

    [Header("Hero Upgrade Data By Level")]
    public List<HeroLevelStat> levelStats = new List<HeroLevelStat>();

    public HeroLevelStat GetStatForLevel(int level)
    {
        int index = Mathf.Clamp(level - 1, 0, levelStats.Count - 1);
        return levelStats[index];
    }

    [ContextMenu("Load Default 10 Levels Data")]
    public void LoadDefaultData()
    {
        // 1. Khởi tạo 10 cấp độ cho Alleria Swiftwind
        levelStats = new List<HeroLevelStat>
        {
            new HeroLevelStat { level = 1,  expToNextLevel = 30,  maxHP = 250, casualHP = 300, minDamage = 2,  maxDamage = 4,  minRangedDamage = 7,  maxRangedDamage = 12, armorType = ArmorType.None, armorPercentage = 0f, respawnTime = 15f },
            new HeroLevelStat { level = 2,  expToNextLevel = 70,  maxHP = 270, casualHP = 324, minDamage = 4,  maxDamage = 6,  minRangedDamage = 8,  maxRangedDamage = 14, armorType = ArmorType.None, armorPercentage = 0f, respawnTime = 15f },
            new HeroLevelStat { level = 3,  expToNextLevel = 120, maxHP = 290, casualHP = 348, minDamage = 6,  maxDamage = 8,  minRangedDamage = 9,  maxRangedDamage = 15, armorType = ArmorType.None, armorPercentage = 0f, respawnTime = 15f },
            new HeroLevelStat { level = 4,  expToNextLevel = 180, maxHP = 310, casualHP = 372, minDamage = 7,  maxDamage = 11, minRangedDamage = 10, maxRangedDamage = 17, armorType = ArmorType.None, armorPercentage = 0f, respawnTime = 15f },
            new HeroLevelStat { level = 5,  expToNextLevel = 250, maxHP = 330, casualHP = 396, minDamage = 9,  maxDamage = 13, minRangedDamage = 11, maxRangedDamage = 18, armorType = ArmorType.None, armorPercentage = 0f, respawnTime = 15f },
            new HeroLevelStat { level = 6,  expToNextLevel = 350, maxHP = 350, casualHP = 420, minDamage = 10, maxDamage = 16, minRangedDamage = 12, maxRangedDamage = 20, armorType = ArmorType.None, armorPercentage = 0f, respawnTime = 15f },
            new HeroLevelStat { level = 7,  expToNextLevel = 480, maxHP = 370, casualHP = 444, minDamage = 12, maxDamage = 18, minRangedDamage = 13, maxRangedDamage = 21, armorType = ArmorType.None, armorPercentage = 0f, respawnTime = 15f },
            new HeroLevelStat { level = 8,  expToNextLevel = 630, maxHP = 390, casualHP = 468, minDamage = 14, maxDamage = 20, minRangedDamage = 14, maxRangedDamage = 23, armorType = ArmorType.None, armorPercentage = 0f, respawnTime = 15f },
            new HeroLevelStat { level = 9,  expToNextLevel = 890, maxHP = 410, casualHP = 492, minDamage = 15, maxDamage = 23, minRangedDamage = 14, maxRangedDamage = 24, armorType = ArmorType.None, armorPercentage = 0f, respawnTime = 15f },
            new HeroLevelStat { level = 10, expToNextLevel = 0,   maxHP = 430, casualHP = 516, minDamage = 17, maxDamage = 25, minRangedDamage = 15, maxRangedDamage = 26, armorType = ArmorType.None, armorPercentage = 0f, respawnTime = 15f }
        };

        // 2. Khởi tạo dữ liệu chiêu Multishot
        multishotLevels = new List<MultishotSkillLevelData>
        {
            new MultishotSkillLevelData { abilityLevel = 1, requiredHeroLevel = 2, arrowCount = 3 },
            new MultishotSkillLevelData { abilityLevel = 2, requiredHeroLevel = 5, arrowCount = 4 },
            new MultishotSkillLevelData { abilityLevel = 3, requiredHeroLevel = 8, arrowCount = 5 }
        };

        // 3. Khởi tạo dữ liệu chiêu Call of the Wild (Wildcat)
        wildcatStats = new List<WildcatStat>
        {
            new WildcatStat { abilityLevel = 1, requiredHeroLevel = 4,  maxHP = 200, casualHP = 240, flashHP = 250, minDamage = 6,  maxDamage = 8,  armorType = ArmorType.None, respawnTime = 20f },
            new WildcatStat { abilityLevel = 2, requiredHeroLevel = 7,  maxHP = 400, casualHP = 480, flashHP = 500, minDamage = 10, maxDamage = 12, armorType = ArmorType.None, respawnTime = 20f },
            new WildcatStat { abilityLevel = 3, requiredHeroLevel = 10, maxHP = 600, casualHP = 720, flashHP = 750, minDamage = 14, maxDamage = 16, armorType = ArmorType.None, respawnTime = 20f }
        };

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
#endif
    }

    private void Reset()
    {
        LoadDefaultData();
    }
}