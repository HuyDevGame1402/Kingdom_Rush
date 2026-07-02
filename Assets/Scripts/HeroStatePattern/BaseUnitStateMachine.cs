using UnityEngine;

public class BaseUnitStateMachine : MonoBehaviour
{
    [Header("Data Configuration")]
    public UnitDataSO unitData;
    public GameObject spriteObject;

    [Header("Combat Targets (Dành cho AI)")]
    public Transform currentTarget;
    [HideInInspector] public float lastAttackTime;

    // Quản lý các States
    public UnitBaseState CurrentState { get; private set; }

    public UnitIdleState IdleState { get; private set; }
    public UnitRunState RunState { get; private set; }
    public UnitAttackState AttackState { get; private set; }
    public UnitDeathState DeathState { get; private set; }

    protected virtual void Awake()
    {
        // Khởi tạo các trạng thái có sẵn
        IdleState = new UnitIdleState(this);
        RunState = new UnitRunState(this);
        AttackState = new UnitAttackState(this);
        DeathState = new UnitDeathState(this);
    }

    protected virtual void Start()
    {
        // Trạng thái ban đầu luôn là Idle
        TransitionToState(IdleState);
    }

    protected virtual void Update()
    {
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
}