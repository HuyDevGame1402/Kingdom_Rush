using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HeroGeraldController : BaseUnitStateMachine
{
    public HeroGeraldLightseekerDataSO GeraldData => unitData as HeroGeraldLightseekerDataSO;

    [Header("Skill Settings")]
    [SerializeField] private float courageCooldown = 8f;
    [SerializeField] private float courageBuffRadius = 5f;

    [Header("Shield Block Settings")]
    [SerializeField] private float shieldBlockCooldown = 10f;
    [Range(0f, 1f)][SerializeField] private float blockChance = 0.3f;

    private float lastCourageTime = -999f;
    private float lastShieldBlockTime = -999f;

    public bool IsBlocking { get; set; }

    // States của Hero
    public GeraldCourageState CourageState { get; private set; }
    public GeraldShieldBlockState ShieldBlockState { get; private set; }
    public GeraldLevelUpState LevelUpState { get; private set; }

    [SerializeField] private TriggerHeroInSide triggerHeroInSide;
    [SerializeField] private HeroDataInGame heroDataInGame;

    public float percentCounterDamage;
    public float percentDamageAttack;

    protected override void Awake()
    {
        base.Awake();

        // Khởi tạo các Skill State
        CourageState = new GeraldCourageState(this);
        ShieldBlockState = new GeraldShieldBlockState(this);
        LevelUpState = new GeraldLevelUpState(this);

        transform.GetComponent<HeroDataInGame>().OnLevelUpEvent += TriggerLevelUp;
        transform.GetComponent<HeroDataInGame>().OnMoveToFlagEvent += HeroGeraldController_OnMoveToFlagEvent;
        if(triggerHeroInSide == null) triggerHeroInSide = GetComponent<TriggerHeroInSide>();
        if(heroDataInGame == null) heroDataInGame = GetComponent<HeroDataInGame>();

        //transform.GetComponent<HealthHero>().OnHitDamageSheldEvent += HeroGeraldController_OnHitDamageSheldEvent;
    }

    private void HeroGeraldController_OnHitDamageSheldEvent(int damage, Transform attacker)
    {
        if (TryTriggerShieldBlock())
        {
            // Phản damage
            attacker.GetComponent<EnemyController>().TakeDamage((int)(damage * percentDamageAttack)
                , textSO, DamageType.True, transform);
        }
    }

    private void HeroGeraldController_OnMoveToFlagEvent(Vector3 pos)
    {
        MoveToFlag(pos);
    }

    protected override void Start()
    {
        base.Start();
        if (HeroVisualGUIManager.Instance != null)
        {
            HeroVisualGUIManager.Instance.SetPlayerHero(transform);
        }
    }

    protected override void Update()
    {
        base.Update();

        // Nếu đang trong quá trình Level Up -> Khóa không cho dùng Skill Courage tự động
        if (CurrentState == LevelUpState) return;

        // Kiểm tra điều kiện tự động dùng Skill Courage khi đang giao tranh
        CheckAutoCourageSkill();
    }

    private void CheckAutoCourageSkill()
    {
        if(triggerHeroInSide != null && triggerHeroInSide.CheckCountSolider(2) && heroDataInGame.currentLevel
             >= GeraldData.courageSkillStats[0].requiredHeroLevel)
        {
            if (Time.time >= lastCourageTime + courageCooldown)
            {
                if (CurrentState == IdleState || CurrentState == AttackState)
                {
                    if (currentTarget != null)
                    {
                        lastCourageTime = Time.time;
                        TransitionToState(CourageState);
                    }
                }
            }
        }
    }

    public void BuffCourageSkillForHero(StatModifier newMod, float healthBuffMax)
    {
        triggerHeroInSide.AddBuffForSoliderInSide(newMod, healthBuffMax);
    }

    /// <summary>
    /// Kích hoạt khi Hero Lên Cấp (Gọi từ Event / Health / Level System)
    /// </summary>
    public void TriggerLevelUp(int currentLevel)
    {
        // Nếu đã chết thì không chạy Level Up Animation
        if (CurrentState == DeathState || isDead) return;

        TransitionToState(LevelUpState);
    }

    /// <summary>
    /// Kích hoạt khi bị quái đánh (Gọi từ hàm TakeDamage trong HealthHero)
    /// </summary>
    public bool TryTriggerShieldBlock()
    {
        // Đang Level Up hoặc Đã chết -> Không thể Block
        if (CurrentState == LevelUpState || CurrentState == DeathState
            || CurrentState == ShieldBlockState) return false;
        TransitionToState(ShieldBlockState);
        return true;
    }

    /// <summary>
    /// Logic thi triển Buff đồng đội
    /// </summary>
    public void ApplyCourageBuff()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, courageBuffRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player") || hit.CompareTag("Soldier"))
            {
                Debug.Log($"<color=cyan>[BUFF COURAGE]</color> Đã buff cho: {hit.name}");
            }
        }
    }

    public HeroDataInGame GetHeroDataInGame()
    {
        return heroDataInGame;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, courageBuffRadius);
    }

    public bool CheckCounterDamage(int damage, Transform attacker)
    {
        if(heroDataInGame.currentLevel < GeraldData.shieldSkillStats[0].requiredHeroLevel)
        {
            return false;
        }

        if(heroDataInGame.currentLevel >= GeraldData.shieldSkillStats[0].requiredHeroLevel
            && heroDataInGame.currentLevel < GeraldData.shieldSkillStats[1].requiredHeroLevel)
        {
            percentDamageAttack = GeraldData.shieldSkillStats[0].damageReflectedPercent;
            percentCounterDamage = GeraldData.shieldSkillStats[0].triggerChance;
            HeroGeraldController_OnHitDamageSheldEvent(damage, attacker);
            return Random.value < percentCounterDamage;
        }
        else if(heroDataInGame.currentLevel >= GeraldData.shieldSkillStats[1].requiredHeroLevel
            && heroDataInGame.currentLevel < GeraldData.shieldSkillStats[2].requiredHeroLevel)
        {
            percentDamageAttack = GeraldData.shieldSkillStats[1].damageReflectedPercent;
            percentCounterDamage = GeraldData.shieldSkillStats[1].triggerChance;
            HeroGeraldController_OnHitDamageSheldEvent(damage, attacker);
            return Random.value < percentCounterDamage;
        }
        else if (heroDataInGame.currentLevel >= GeraldData.shieldSkillStats[2].requiredHeroLevel)
        {
            percentDamageAttack = GeraldData.shieldSkillStats[2].damageReflectedPercent;
            percentCounterDamage = GeraldData.shieldSkillStats[2].triggerChance;
            HeroGeraldController_OnHitDamageSheldEvent(damage, attacker);
            return Random.value < percentCounterDamage;
        }
        return false;
    }
}