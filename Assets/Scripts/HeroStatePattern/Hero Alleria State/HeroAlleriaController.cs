using UnityEngine;

public class HeroAlleriaController : BaseUnitStateMachine
{
    public HeroAlleriaSwiftwindDataSO AlleriaData => unitData as HeroAlleriaSwiftwindDataSO;

    [Header("Skill Settings - Multishot")]
    [SerializeField] private float multishotCooldown = 4f;

    private float lastMultishotTime = -999f;

    private GameObject currentSummonedWildcat; // Quản lý tối đa 1 Wildcat trên sân

    // States của Alleria
    public AlleriaMultishotState MultishotState { get; private set; }
    public AlleriaCallOfTheWildState CallOfTheWildState { get; private set; }
    public AlleriaLevelUpState LevelUpState { get; private set; }

    private HeroDataInGame heroDataInGame;
    private HeroAlleriaSpawnWildCat heroAlleriaSpawnWildCat;

    protected override void Awake()
    {
        base.Awake();

        // Khởi tạo các State
        MultishotState = new AlleriaMultishotState(this);
        CallOfTheWildState = new AlleriaCallOfTheWildState(this);
        LevelUpState = new AlleriaLevelUpState(this);

        transform.GetComponent<HeroDataInGame>().OnLevelUpEvent += TriggerLevelUp;
        transform.GetComponent<HeroDataInGame>().OnMoveToFlagEvent += HeroAlleriaController_OnMoveToFlagEvent;
        heroDataInGame = GetComponent<HeroDataInGame>();
        heroAlleriaSpawnWildCat = GetComponent<HeroAlleriaSpawnWildCat>();
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

        // Ưu tiên kiểm tra gọi Wildcat nếu chưa có Wildcat trên sân
        CheckAutoCallOfTheWildSkill();

        // Kiểm tra thi triển Multishot khi đang giao tranh
        CheckAutoMultishotSkill();
    }

    private void CheckAutoCallOfTheWildSkill()
    {
        if(heroDataInGame.currentLevel < AlleriaData.wildcatStats[0].requiredHeroLevel
            || heroAlleriaSpawnWildCat.GetIsReadySpawn() == false)
        {
            return;
        }

        if (CurrentState == IdleState || CurrentState == AttackState)
        {
            TransitionToState(CallOfTheWildState);
        }
    }

    public void CallWildCat()
    {
        // Instaine ra
        if (currentSummonedWildcat == null)
        {
            heroAlleriaSpawnWildCat.CreateWildCat();
        }
        // active lại
        else
        {
            heroAlleriaSpawnWildCat.RespawnWildCat();
        }
    }

    private void CheckAutoMultishotSkill()
    {
        if (targetList.Count > 0) return;
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

    public override void ResetTarget()
    {
        currentTarget = null;
        // Ưu tiên tìm trong targetList trước (cận chiến)
        if (targetList.Count > 0)
        {
            for (int i = 0; i < targetList.Count; i++)
            {
                if (targetList[i].TryGetComponent(out EnemyController enemyController))
                {
                    if (enemyController.CheckAttackerCount() && enemyController.isDead == false)
                    {
                        currentTarget = targetList[i];
                        return;
                    }
                }
            }
            currentTarget = targetList[Random.Range(0, targetList.Count)];
            return;
        }

        // Fallback: nếu không có mục tiêu cận chiến thì tìm trong targetLongRangeList
        if (targetLongRangeList.Count > 0)
        {
            for (int i = 0; i < targetLongRangeList.Count; i++)
            {
                if (targetLongRangeList[i].TryGetComponent(out EnemyController enemyController))
                {
                    if (enemyController.CheckAttackerCount() && enemyController.isDead == false)
                    {
                        currentTarget = targetLongRangeList[i];
                        return;
                    }
                }
            }
            currentTarget = targetLongRangeList[Random.Range(0, targetLongRangeList.Count)];
        }
    }

    public override void MoveToFlag(Vector3 flagPos)
    {
        base.MoveToFlag(flagPos);

        if (currentSummonedWildcat != null &&
            !currentSummonedWildcat.GetComponent<HealthHero>().IsDead())
        {
            // Hướng của hero tới điểm cờ (hoặc dùng transform.right/up tùy game của bạn)
            Vector2 forward = ((Vector2)flagPos - (Vector2)transform.position).normalized;

            // Random góc ±45 độ quanh hướng forward
            float angle = Random.Range(-45f, 45f);
            Vector2 randomDir = Quaternion.Euler(0, 0, angle) * forward;

            // Random khoảng cách
            float distance = Random.Range(0.5f, 1.5f);

            Vector2 targetPos = (Vector2)transform.position + randomDir * distance;

            currentSummonedWildcat.GetComponent<BaseUnitStateMachine>()
                .MoveToFlag(targetPos);
        }
    }

    public HeroDataInGame GetHeroDataInGame()
    {
        return heroDataInGame;
    }

    public void SetWildCat(GameObject wildCat)
    {
        currentSummonedWildcat = wildCat;
    }

    public GameObject GetWildCat()
    {
        return currentSummonedWildcat;
    }
}