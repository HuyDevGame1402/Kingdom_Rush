using System.Collections.Generic;
using UnityEngine;
using System;


#if UNITY_EDITOR
using UnityEditor; // Thêm thư viện này để báo cho Unity lưu File Asset
#endif

// --- 1. Struct cho kỹ năng Courage ---
[Serializable]
public struct CourageSkillLevelStat
{
    public int abilityLevel;       // Cấp độ kỹ năng (1 - 3)
    public int requiredHeroLevel;  // Cấp độ Hero tối thiểu để mở/nâng cấp (2, 5, 8)
    public int damageBoost;        // Sát thương cộng thêm (+2, +4, +6)
    public float healBoostPercent; // Tỉ lệ hồi máu theo Max HP (0.15f = 15%)
    public float armorBoostPercent;// Giáp cộng thêm (0.05f, 0.10f, 0.15f)
    public float duration;         // Thời gian hiệu lực (6s)
    public float cooldown;         // Thời gian hồi chiêu (8s)
    public float aoeRadius;        // Bán kính tác dụng
    public int minRequiredAllies;  // Số đồng minh cận chiến tối thiểu xung quanh (2)
}

// --- 2. Struct cho kỹ năng Shield of Retribution ---
[Serializable]
public struct ShieldOfRetributionSkillLevelStat
{
    public int abilityLevel;            // Cấp độ kỹ năng (1 - 3)
    public int requiredHeroLevel;       // Cấp độ Hero tối thiểu để mở/nâng cấp (4, 7, 10)
    public float damageReflectedPercent;// Tỉ lệ phản sát thương True Damage (1.0f, 1.5f, 2.0f)
    public float triggerChance;         // Tỉ lệ kích hoạt (0.20f, 0.40f, 0.60f)
}

[CreateAssetMenu(fileName = "NewHeroGeraldLightseekerData", menuName = "KingdomRush/Hero Data")]
public class HeroGeraldLightseekerDataSO : UnitDataSO
{
    [Header("Hero Specific Animations")]
    public GeraldLightseekerAnimationConfig heroAnimations;

    [Header("Hero Upgrade Data By Level")]
    public List<HeroLevelStat> levelStats = new List<HeroLevelStat>();

    [Header("Hero Skills Data")]
    public List<CourageSkillLevelStat> courageSkillStats = new List<CourageSkillLevelStat>();
    public List<ShieldOfRetributionSkillLevelStat> shieldSkillStats = new List<ShieldOfRetributionSkillLevelStat>();

    public HeroLevelStat GetStatForLevel(int level)
    {
        int index = Mathf.Clamp(level - 1, 0, levelStats.Count - 1);
        return levelStats[index];
    }

    [ContextMenu("Load Default 10 Levels Data")]
    public void LoadDefaultData()
    {
        // --- 1. Dữ liệu chỉ số cơ bản theo cấp độ ---
        levelStats = new List<HeroLevelStat>
        {
            new HeroLevelStat { level = 1,  expToNextLevel = 30,  maxHP = 400, casualHP = 480, minDamage = 11, maxDamage = 18, minRangedDamage = 0, maxRangedDamage = 0, armorType = ArmorType.Low,    armorPercentage = 0.30f, respawnTime = 15f },
            new HeroLevelStat { level = 2,  expToNextLevel = 70,  maxHP = 420, casualHP = 504, minDamage = 12, maxDamage = 20, minRangedDamage = 0, maxRangedDamage = 0, armorType = ArmorType.Low,    armorPercentage = 0.30f, respawnTime = 15f },
            new HeroLevelStat { level = 3,  expToNextLevel = 120, maxHP = 440, casualHP = 528, minDamage = 14, maxDamage = 23, minRangedDamage = 0, maxRangedDamage = 0, armorType = ArmorType.Medium, armorPercentage = 0.40f, respawnTime = 15f },
            new HeroLevelStat { level = 4,  expToNextLevel = 180, maxHP = 460, casualHP = 552, minDamage = 15, maxDamage = 25, minRangedDamage = 0, maxRangedDamage = 0, armorType = ArmorType.Medium, armorPercentage = 0.40f, respawnTime = 15f },
            new HeroLevelStat { level = 5,  expToNextLevel = 250, maxHP = 480, casualHP = 576, minDamage = 17, maxDamage = 28, minRangedDamage = 0, maxRangedDamage = 0, armorType = ArmorType.Medium, armorPercentage = 0.50f, respawnTime = 15f },
            new HeroLevelStat { level = 6,  expToNextLevel = 350, maxHP = 500, casualHP = 600, minDamage = 18, maxDamage = 30, minRangedDamage = 0, maxRangedDamage = 0, armorType = ArmorType.Medium, armorPercentage = 0.50f, respawnTime = 15f },
            new HeroLevelStat { level = 7,  expToNextLevel = 480, maxHP = 520, casualHP = 624, minDamage = 20, maxDamage = 33, minRangedDamage = 0, maxRangedDamage = 0, armorType = ArmorType.Medium, armorPercentage = 0.60f, respawnTime = 15f },
            new HeroLevelStat { level = 8,  expToNextLevel = 630, maxHP = 540, casualHP = 648, minDamage = 21, maxDamage = 35, minRangedDamage = 0, maxRangedDamage = 0, armorType = ArmorType.Medium, armorPercentage = 0.60f, respawnTime = 15f },
            new HeroLevelStat { level = 9,  expToNextLevel = 890, maxHP = 560, casualHP = 672, minDamage = 23, maxDamage = 38, minRangedDamage = 0, maxRangedDamage = 0, armorType = ArmorType.High,   armorPercentage = 0.70f, respawnTime = 15f },
            new HeroLevelStat { level = 10, expToNextLevel = 0,   maxHP = 580, casualHP = 696, minDamage = 24, maxDamage = 40, minRangedDamage = 0, maxRangedDamage = 0, armorType = ArmorType.High,   armorPercentage = 0.80f, respawnTime = 15f }
        };

        // --- 2. Dữ liệu Skill: Courage ---
        courageSkillStats = new List<CourageSkillLevelStat>
        {
            new CourageSkillLevelStat { abilityLevel = 1, requiredHeroLevel = 2, damageBoost = 2, healBoostPercent = 0.15f, armorBoostPercent = 0.05f, duration = 6f, cooldown = 8f, aoeRadius = 3f, minRequiredAllies = 2 },
            new CourageSkillLevelStat { abilityLevel = 2, requiredHeroLevel = 5, damageBoost = 4, healBoostPercent = 0.15f, armorBoostPercent = 0.10f, duration = 6f, cooldown = 8f, aoeRadius = 3f, minRequiredAllies = 2 },
            new CourageSkillLevelStat { abilityLevel = 3, requiredHeroLevel = 8, damageBoost = 6, healBoostPercent = 0.15f, armorBoostPercent = 0.15f, duration = 6f, cooldown = 8f, aoeRadius = 3f, minRequiredAllies = 2 }
        };

        // --- 3. Dữ liệu Skill: Shield of Retribution ---
        shieldSkillStats = new List<ShieldOfRetributionSkillLevelStat>
        {
            new ShieldOfRetributionSkillLevelStat { abilityLevel = 1, requiredHeroLevel = 4,  damageReflectedPercent = 1.0f, triggerChance = 0.20f },
            new ShieldOfRetributionSkillLevelStat { abilityLevel = 2, requiredHeroLevel = 7,  damageReflectedPercent = 1.5f, triggerChance = 0.40f },
            new ShieldOfRetributionSkillLevelStat { abilityLevel = 3, requiredHeroLevel = 10, damageReflectedPercent = 2.0f, triggerChance = 0.60f }
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