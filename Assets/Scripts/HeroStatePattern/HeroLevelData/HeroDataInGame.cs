using UnityEngine;

public class HeroDataInGame : MonoBehaviour
{
    public int currentLevel = 1;
    public HeroLevelStat heroLevelStat;

    public int minDamage;
    public int maxDamage;
    public float armor;

    public int currentExp;
    public int nextExp;

    [SerializeField] private BaseUnitStateMachine baseStateMachine;

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
        armor = heroLevelStat.armorPercentage;
        currentExp = 0;
        nextExp = heroLevelStat.expToNextLevel;
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
    }
}
