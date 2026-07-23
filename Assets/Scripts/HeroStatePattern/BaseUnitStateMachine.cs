using UnityEngine;
using System.Collections.Generic;

public class BaseUnitStateMachine : MonoBehaviour, IResurrection
{
    [Header("Data Configuration")]
    public UnitDataSO unitData;
    public GameObject spriteObject;

    [Header("Combat Targets (Dành cho AI)")]
    public Transform currentTarget;
    public List<Transform> targetList = new List<Transform>();
    [HideInInspector] public float lastAttackTime;

    // Quản lý các States
    public UnitBaseState CurrentState { get; private set; }

    public UnitIdleState IdleState { get; private set; }
    public UnitRunState RunState { get; private set; }
    public UnitAttackState AttackState { get; private set; }
    public UnitDeathState DeathState { get; private set; }

    public HealthHero healthHero;

    private Transform parent;

    public int attackerCount;

    public TextSO textSO;

    public bool isDead;

    private Transform checkTarget;

    public Transform ground;

    public Vector3 positionFlag;
    public bool isRunToFlag;

    protected virtual void Awake()
    {
        // Khởi tạo các trạng thái có sẵn
        IdleState = new UnitIdleState(this);
        RunState = new UnitRunState(this);
        AttackState = new UnitAttackState(this);
        DeathState = new UnitDeathState(this);
        healthHero = GetComponent<HealthHero>();
        healthHero.InitHealth((int)unitData.maxHealth);
        healthHero.OnDead += HealthHero_OnDead;
    }

    private void HealthHero_OnDead()
    {
        isDead = true;
        RemoveAttackerTarget();
        currentTarget = null;
        TransitionToState(DeathState);
        targetList.Clear();
        attackerCount = 0;
        if (parent != null && parent.TryGetComponent(out BarrackSpawnHero barrackSpawnHero))
        {
            barrackSpawnHero.ResurrectionHero(transform, unitData.timeToResurrect);
        }
    }

    private void RemoveAttackerTarget()
    {
        if(currentTarget != null && currentTarget.TryGetComponent(out EnemyController enemyController))
        {
            enemyController.RemoveAttacker();
        }
    }

    protected virtual void Start()
    {
        // Trạng thái ban đầu luôn là Idle
        TransitionToState(IdleState);
    }

    protected virtual void Update()
    {
        // TỰ ĐỘNG DỌN RÁC: Xóa các enemy đã chết hoặc bị ẩn khỏi list liên tục
        CleanDeadTargets();
        // Cập nhật logic của trạng thái hiện tại liên tục
        CurrentState?.Update();
    }

    public void TransitionToState(UnitBaseState newState)
    {
        if (CurrentState == newState) return;

        // Log ra để xem hệ thống có thực sự chuyển từ State cũ sang State mới không
        string oldStateName = CurrentState != null ? CurrentState.GetType().Name : "NULL";
        string newStateName = newState != null ? newState.GetType().Name : "NULL";
        Debug.Log($"<color=yellow>[STATE CHANGE]</color> {gameObject.name} chuyển từ [ {oldStateName} ] -> [ {newStateName} ]");

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }

    // --- CÁC HÀM TIỆN ÍCH DÙNG CHUNG TRONG STATE ---
    public bool IsTargetInAttackRange()
    {
        if (currentTarget == null || !currentTarget.gameObject.activeSelf) return false;
        return Vector3.Distance(transform.position, currentTarget.position) <= unitData.attackRange;
    }

    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + unitData.attackCooldown;
    }
    // ✅ HÀM MỚI: Kiểm tra mục tiêu hiện tại có phải là Kẻ địch hay không
    public bool IsTargetEnemy()
    {
        if (currentTarget == null) return false;

        // Kiểm tra xem mục tiêu có đúng Tag kẻ địch hay không
        return currentTarget.CompareTag("EnemyKingdomRush");
    }
    public bool IsAlignedWithTarget(float tolerance = 0.05f)
    {
        if (currentTarget == null)
            return false;

        return Mathf.Abs(transform.position.y - currentTarget.position.y) <= tolerance;
    }

    public void ResetTarget()
    {
        currentTarget = null;
        if(targetList.Count > 0)
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
        }
    }
    public void SetParent(Transform parent)
    {
        this.parent = parent;
    }

    public void Resurrection(Transform pointSpawn, Transform targetSpawn)
    {
        transform.position = pointSpawn.position;
        currentTarget = targetSpawn;
        healthHero.ResetHealth();
        TransitionToState(RunState);
    }
    public bool CheckAttackerCount()
    {
        return attackerCount < unitData.maxAttacker;
    }
    public void RemoveAttacker()
    {
        attackerCount -= 1;
        if(attackerCount < 0)
        {
            attackerCount = 0;
        }
    }
    private void CleanDeadTargets()
    {
        // Duyệt ngược list từ dưới lên để xóa không bị lỗi Index
        for (int i = targetList.Count - 1; i >= 0; i--)
        {
            checkTarget = targetList[i];

            // Nếu transform bị null, bị ẩn, hoặc có EnemyController đã chết
            if (checkTarget == null || !checkTarget.gameObject.activeInHierarchy ||
               (checkTarget.TryGetComponent(out EnemyController enemy) && enemy.isDead))
            {
                if (checkTarget == currentTarget)
                {
                    currentTarget = null;
                }
                Debug.LogWarning("Đã xóa mục tiêu không hợp lệ khỏi danh sách: " + checkTarget.name);
                targetList.RemoveAt(i);
            }
        }

        // Nếu mất currentTarget nhưng trong list vẫn còn quái sống khác thì chọn lại luôn
        if (currentTarget == null && targetList.Count > 0)
        {
            ResetTarget();
        }
    }
    public bool IsTargetingEnemy(EnemyController enemy)
    {
        return currentTarget == enemy.transform;
    }

    public void ReloadAnimation()
    {
        if(CurrentState == IdleState)
        {
            int currentFrame = SpriteSheetAnimator.Instance.GetCurrentFrameNumber(spriteObject);

            var config = unitData.animations.idle;

            SpriteSheetAnimator.Instance.PlayAnimation(
                target: spriteObject,
                animPrefix: unitData.animations.animPrefix,
                startFrame: config.startFrame,
                endFrame: config.endFrame,
                startFromCurrentFrame: currentFrame,
                frameRate: unitData.animations.frameRate
            );
        }
        else if(CurrentState == RunState)
        {

            int currentFrame = SpriteSheetAnimator.Instance.GetCurrentFrameNumber(spriteObject);

            var config = unitData.animations.run;

            SpriteSheetAnimator.Instance.PlayAnimation(
                target: spriteObject,
                animPrefix: unitData.animations.animPrefix,
                startFrame: config.startFrame,
                endFrame: config.endFrame,
                startFromCurrentFrame: currentFrame,
                frameRate: unitData.animations.frameRate
            );
        }
        else if(CurrentState == AttackState)
        {
            if (!AttackState.IsAttacking)
                return;

            int currentFrame =
            SpriteSheetAnimator.Instance.GetCurrentFrameNumber(spriteObject);

            var config = unitData.animations.attack;

            SpriteSheetAnimator.Instance.PlayAnimationContinue(
                target: spriteObject,
                animPrefix: unitData.animations.animPrefix,
                startFrame: config.startFrame,
                endFrame: config.endFrame,
                startFromCurrentFrame: currentFrame,
                eventFrame: config.eventFrame,
                onEventTrigger: () =>
                {
                    // damage
                },
                offsetConfigs: config.animationConfigOffset,
                frameRate: unitData.animations.frameRate,
                onComplete: () =>
                {
                    // attack complete
                });
        }
        // Dead
        else
        {
            int currentFrame =
            SpriteSheetAnimator.Instance.GetCurrentFrameNumber(spriteObject);

            var config = unitData.animations.death;

            SpriteSheetAnimator.Instance.PlayAnimationContinue(
                target: spriteObject,
                animPrefix: unitData.animations.animPrefix,
                startFrame: config.startFrame,
                endFrame: config.endFrame,
                startFromCurrentFrame: currentFrame,
                frameRate: unitData.animations.frameRate,
                onComplete: () =>
                {
                    gameObject.SetActive(false);
                });
        }
    }
}