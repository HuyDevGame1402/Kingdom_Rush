using UnityEngine;

public class HeroGeraldController : BaseUnitStateMachine
{
    public HeroGeraldLightseekerDataSO GeraldData => unitData as HeroGeraldLightseekerDataSO;

    [Header("Skill Settings")]
    [SerializeField] private float courageCooldown = 15f;
    [SerializeField] private float courageBuffRadius = 5f;
    [SerializeField] private float courageBuffDuration = 5f;
    [SerializeField] private float courageArmorBonus = 10f;

    [Header("Shield Block Settings")]
    [SerializeField] private float shieldBlockCooldown = 10f;
    [Range(0f, 1f)][SerializeField] private float blockChance = 0.3f; // 30% tỷ lệ đỡ đòn khi bị đánh

    private float lastCourageTime = -999f;
    private float lastShieldBlockTime = -999f;

    public bool IsBlocking { get; set; }

    // States của Hero
    public GeraldCourageState CourageState { get; private set; }
    public GeraldShieldBlockState ShieldBlockState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        // Khởi tạo các Skill State
        CourageState = new GeraldCourageState(this);
        ShieldBlockState = new GeraldShieldBlockState(this);
    }

    protected override void Update()
    {
        base.Update();

        // Kiểm tra điều kiện tự động dùng Skill Courage khi đang giao tranh
        CheckAutoCourageSkill();
    }

    private void CheckAutoCourageSkill()
    {
        // Điều kiện dùng Courage: Đã hồi cooldown + Đang ở Idle hoặc Attack + Có kẻ địch xung quanh
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

    /// <summary>
    /// Kích hoạt khi bị quái đánh (Gọi từ hàm TakeDamage trong HealthHero)
    /// </summary>
    public bool TryTriggerShieldBlock()
    {
        if (Time.time >= lastShieldBlockTime + shieldBlockCooldown)
        {
            if (Random.value <= blockChance && CurrentState != DeathState)
            {
                lastShieldBlockTime = Time.time;
                TransitionToState(ShieldBlockState);
                return true; // Đỡ đòn thành công (Miễn/Giảm sát thương)
            }
        }
        return false;
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
                // Gọi Logic Buff giáp/sát thương cho đồng đội xung quanh tại đây
                Debug.Log($"<color=cyan>[BUFF COURAGE]</color> Đã buff cho: {hit.name}");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, courageBuffRadius);
    }
}