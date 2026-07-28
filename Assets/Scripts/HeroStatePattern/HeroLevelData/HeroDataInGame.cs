using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class StatModifier
{
    public string sourceID; // ID nguồn buff (VD: "Gerald_Courage", "Tower_Aura", "Potion_Damage")
    public int valueDamage;     // Giá trị cộng thêm (+2 damage, +0.1f armor...)
    public float valueArmor;
    public float duration;  // Thời gian còn lại (-1 nghĩa là vĩnh viễn)

    public StatModifier(string sourceID, int valueDamage, float valueArmor, float duration)
    {
        this.sourceID = sourceID;
        this.valueDamage = valueDamage;
        this.valueArmor = valueArmor;
        this.duration = duration;
    }
}

public class HeroDataInGame : MonoBehaviour
{
    public int currentLevel = 1;
    public HeroLevelStat heroLevelStat;

    public int minDamage;
    public int maxDamage;
    public int minRangedDamage;
    public int maxRangedDamage;
    public float armor;

    [SerializeField] private int finalMinDamage;
    [SerializeField] private int finalMaxDamage;
    [SerializeField] private int finalMinRangedDamage;
    [SerializeField] private int finalMaxRangedDamage;
    [SerializeField] private float finalArmor;
    private bool hasChanges;

    public int currentExp;
    public int nextExp;
    public event Action<int, int> OnChangeExpEvent;
    public event Action<int> OnLevelUpEvent;
    public event Action<Vector3> OnMoveToFlagEvent;

    [SerializeField] private BaseUnitStateMachine baseStateMachine;

    public Vector3 positionFlag;

    [SerializeField] private List<StatModifier> activeModifiers = new List<StatModifier>();

    private void Awake()
    {
        transform.GetComponent<HealthHero>().OnDead += ResetActiveModifers;
    }

    private void Start()
    {
        if(baseStateMachine == null) baseStateMachine = GetComponent<BaseUnitStateMachine>();

        if (baseStateMachine.unitData.isHasLevelHero)
        {
            InitDataLevel();
        }
        else
        {
            InitDataSOBase();
        }
    }

    private void InitDataLevel()
    {
        heroLevelStat = transform.GetComponent<IHasDataLevel>().GetHeroLevelStat(currentLevel);
        minDamage = heroLevelStat.minDamage;
        maxDamage = heroLevelStat.maxDamage;
        minRangedDamage = heroLevelStat.minRangedDamage;
        maxRangedDamage = heroLevelStat.maxRangedDamage;
        armor = heroLevelStat.armorPercentage;
        nextExp = heroLevelStat.expToNextLevel;
        finalArmor = armor;
        finalMinDamage = minDamage;
        finalMaxDamage = maxDamage;
        finalMinRangedDamage = minRangedDamage;
        finalMaxRangedDamage = maxRangedDamage;
        RecalculateStats();
    }

    private void InitDataSOBase()
    {
        minDamage = (int)baseStateMachine.unitData.minDamage;
        maxDamage = (int)baseStateMachine.unitData.maxDamage;
        armor = baseStateMachine.unitData.armor;
        currentExp = 0;
    }

    public void AddEXP(int expGained)
    {
        currentExp += expGained;

        if(currentExp >= nextExp)
        {
            currentExp -= nextExp;
            currentLevel += 1;
            InitDataLevel();
            OnLevelUpEvent?.Invoke(currentLevel);
        }

        OnChangeExpEvent?.Invoke(currentExp, nextExp);
    }

    public void SetPositionFlag(Vector3 pos)
    {
        positionFlag = pos;
        OnMoveToFlagEvent?.Invoke(positionFlag);
    }

    private void Update()
    {
        if (activeModifiers.Count == 0) return;
        UpdateModifiersTimer();
    }

    public void AddModifier(StatModifier newMod)
    {
        // Kiểm tra xem đã có Buff từ nguồn này chưa (VD: Đã có buff Courage của Gerald rồi)
        StatModifier existingMod = activeModifiers.Find(m => m.sourceID == newMod.sourceID);

        if (existingMod != null)
        {
            // Nếu đã có -> Reset lại thời gian (Refresh Duration)
            existingMod.duration = newMod.duration;
            existingMod.valueDamage = newMod.valueDamage; // Cập nhật lại giá trị nếu skill nâng cấp
            existingMod.valueArmor = newMod.valueArmor;
        }
        else
        {
            // Chưa có -> Thêm mới vào danh sách
            activeModifiers.Add(newMod);
        }

        // Tính toán lại Stat ngay lập tức!
        RecalculateStats();
    }

    private void RecalculateStats()
    {
        finalMinDamage = minDamage;
        finalMaxDamage = maxDamage;
        finalMinRangedDamage = minRangedDamage;
        finalMaxRangedDamage = maxRangedDamage;
        finalArmor = armor;
        foreach (var mod in activeModifiers)
        {
            if(mod.duration > 0)
            {
                finalMinDamage += mod.valueDamage;
                finalMaxDamage += mod.valueDamage;
                finalMinRangedDamage += mod.valueDamage;
                finalMaxRangedDamage += mod.valueDamage;
                finalArmor += mod.valueArmor;
            }
        }
    }
    private void UpdateModifiersTimer()
    {
        hasChanges = false;
        for (int i = activeModifiers.Count - 1; i >= 0; i--)
        {
            // Nếu buff có đếm ngược thời gian (duration > 0)
            if (activeModifiers[i].duration > 0)
            {
                activeModifiers[i].duration -= Time.deltaTime;

                // Khi hết hạn -> Xóa khỏi danh sách
                if (activeModifiers[i].duration <= 0)
                {
                    activeModifiers.RemoveAt(i);
                    hasChanges = true;
                }
            }
        }

        // Nếu có ít nhất 1 buff vừa hết hạn -> Tính lại Stat
        if (hasChanges)
        {
            RecalculateStats();
        }
    }

    public void ResetActiveModifers()
    {
        activeModifiers.Clear();
    }

    public int GetMinDamage()
    {
        return finalMinDamage;
    }
    public int GetMaxDamage()
    {
        return finalMaxDamage;
    }

    public float GetArmor()
    {
        return finalArmor;
    }

    public int GetMinRangedDamage() { return finalMinRangedDamage; }

    public int GetMaxRangedDamage() { return finalMaxRangedDamage; }
}
