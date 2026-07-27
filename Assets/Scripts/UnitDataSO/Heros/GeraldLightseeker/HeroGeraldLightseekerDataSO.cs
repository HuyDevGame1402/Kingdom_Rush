using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor; // Thêm thư viện này để báo cho Unity lưu File Asset
#endif

[CreateAssetMenu(fileName = "NewHeroGeraldLightseekerData", menuName = "KingdomRush/Hero Data")]
public class HeroGeraldLightseekerDataSO : UnitDataSO
{

    [Header("Hero Specific Animations")]
    public GeraldLightseekerAnimationConfig heroAnimations;

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