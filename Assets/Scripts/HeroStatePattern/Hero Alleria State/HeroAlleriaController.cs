using UnityEngine;

public class HeroAlleriaController : BaseUnitStateMachine
{
    public HeroAlleriaSwiftwindDataSO AlleriaData => unitData as HeroAlleriaSwiftwindDataSO;

    [Header("Skill Settings - Multishot")]
    [SerializeField] private float multishotCooldown = 4f;

    [Header("Skill Settings - Call of the Wild (Wildcat)")]
    [SerializeField] private float callOfTheWildCooldown = 20f;
    [SerializeField] private GameObject wildcatPrefab; // Prefab con linh miêu được gọi ra

    private float lastMultishotTime = -999f;
    private float lastCallOfTheWildTime = -999f;

    private GameObject currentSummonedWildcat; // Quản lý tối đa 1 Wildcat trên sân

    // States của Alleria
    public AlleriaMultishotState MultishotState { get; private set; }
    public AlleriaCallOfTheWildState CallOfTheWildState { get; private set; }
    public AlleriaLevelUpState LevelUpState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        // Khởi tạo các State
        MultishotState = new AlleriaMultishotState(this);
        CallOfTheWildState = new AlleriaCallOfTheWildState(this);
        LevelUpState = new AlleriaLevelUpState(this);

        transform.GetComponent<HeroDataInGame>().OnLevelUpEvent += TriggerLevelUp;
        transform.GetComponent<HeroDataInGame>().OnMoveToFlagEvent += HeroAlleriaController_OnMoveToFlagEvent;
    }

    private void HeroAlleriaController_OnMoveToFlagEvent(Vector3 pos)
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

        // Nếu đang Level Up -> Không dùng skill tự động
        if (CurrentState == LevelUpState) return;

        //// Ưu tiên kiểm tra gọi Wildcat nếu chưa có Wildcat trên sân
        //CheckAutoCallOfTheWildSkill();

        //// Kiểm tra thi triển Multishot khi đang giao tranh
        //CheckAutoMultishotSkill();
    }

    private void CheckAutoCallOfTheWildSkill()
    {
        // Điều kiện: Đã hết Cooldown VÀ Linh miêu hiện tại chưa được triệu hồi (hoặc đã chết)
        if (currentSummonedWildcat == null && Time.time >= lastCallOfTheWildTime + callOfTheWildCooldown)
        {
            if (CurrentState == IdleState || CurrentState == AttackState)
            {
                lastCallOfTheWildTime = Time.time;
                TransitionToState(CallOfTheWildState);
            }
        }
    }

    private void CheckAutoMultishotSkill()
    {
        if (Time.time >= lastMultishotTime + multishotCooldown)
        {
            if (CurrentState == IdleState || CurrentState == AttackState)
            {
                if (currentTarget != null)
                {
                    lastMultishotTime = Time.time;
                    TransitionToState(MultishotState);
                }
            }
        }
    }

    public void TriggerLevelUp(int currentLevel)
    {
        if (CurrentState == DeathState || isDead) return;

        TransitionToState(LevelUpState);
    }

    /// <summary>
    /// Logic triệu hồi Linh Miêu (Wildcat)
    /// </summary>
    public void SummonWildcat()
    {
        if (wildcatPrefab == null)
        {
            Debug.LogWarning("[ALLERIA] Chưa gán Wildcat Prefab!");
            return;
        }

        // Triệu hồi Wildcat tại vị trí của Alleria
        currentSummonedWildcat = Instantiate(wildcatPrefab, transform.position, Quaternion.identity);
        Debug.Log("<color=green>[SUMMON WILDCAT]</color> Alleria đã triệu hồi Wildcat!");
    }

    /// <summary>
    /// Logic bắn nhiều mũi tên (Multishot)
    /// </summary>
    public void PerformMultishot()
    {
        Debug.Log("<color=yellow>[MULTISHOT]</color> Alleria bắn chiêu Multishot!");
        // Viết logic sinh ra các Projectile Arrow bắn vào mục tiêu ở đây
    }
}