using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemyController : MonoBehaviour
{
    public event Action OnEnemyDestroyed;
    [Header("Data Configuration")]
    public UnitDataSO unitData; // Kéo file ScriptableObject của bạn vào đây

    [Header("Movement Path (Waypoints)")]
    public List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    [Header("Combat Target")]
    [Tooltip("Mục tiêu tấn công (Ví dụ: Lính lác hoặc Tướng của người chơi chặn đường)")]
    public Transform target;

    // Các thuộc tính Runtime (Lấy từ SO sang để có thể bị trừ máu, thay đổi tốc độ khi chơi)
    [HideInInspector] public float currentHealth;
    [HideInInspector] public bool isDead = false;

    // Quản lý State Hiện Tại
    private IEnemyState currentState;

    // Cache các trạng thái để tránh tạo rác bộ nhớ (Garbage Collection)
    public EnemyMoveState MoveState = new EnemyMoveState();
    public EnemyAttackState AttackState = new EnemyAttackState();
    public EnemyDeathState DeathState = new EnemyDeathState();

    // Thêm một biến ẩn ở trên cùng của class EnemyController để ghi nhớ hướng đi hiện tại
    private string lastPlayedAnimDirection = "";

    void Start()
    {
        if (unitData == null)
        {
            Debug.LogError($"⚠️ [{name}] Thiếu file UnitDataSO trên Inspector!");
            return;
        }

        // Khởi tạo chỉ số ban đầu từ ScriptableObject
        currentHealth = unitData.maxHealth;

        // Bắt đầu bằng trạng thái di chuyển
        TransitionToState(MoveState);
    }

    void Update()
    {
        if (isDead) return;

        // Thực thi logic của trạng thái hiện tại mỗi frame
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    public void TransitionToState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);
    }

    // --- LOGIC DI CHUYỂN THEO WAYPOINT & TỰ ĐỔI HƯỚNG SPRITE ---
    public void HandleMovement()
    {
        if (waypoints == null || waypoints.Count == 0 || currentWaypointIndex >= waypoints.Count)
        {
            // Đã đi đến đích cuối cùng của bản đồ (Người chơi mất máu cổng thành)
            ReachedEndOfTheLine();
            return;
        }

        Transform targetPoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetPoint.position - transform.position;
        float distanceThisFrame = unitData.moveSpeed * Time.deltaTime;

        // Nếu khoảng cách đến điểm kiểm tra nhỏ hơn quãng đường di chuyển frame này -> Đã chạm điểm
        if (direction.magnitude <= distanceThisFrame)
        {
            transform.position = targetPoint.position;
            currentWaypointIndex++; // Chuyển sang điểm tiếp theo
        }
        else
        {
            // Di chuyển quái về phía waypoint
            Vector3 moveVector = direction.normalized * distanceThisFrame;
            transform.Translate(moveVector, Space.World);

            // Cập nhật hoạt ảnh chạy chuẩn hướng dựa vào vector di chuyển
            UpdateMoveAnimation(direction.normalized);
        }
    }

    private void UpdateMoveAnimation(Vector2 moveDir)
    {
        if (EnemySpriteAnimator.Instance == null) return;

        // 1. Xử lý lật mặt Sprite trái / phải
        if (moveDir.x > 0.1f) transform.localScale = unitData.localScaleRight;
        else if (moveDir.x < -0.1f) transform.localScale = unitData.localScaleLeft;

        // 2. Phân tích hướng đi và lấy nguyên cụm Config Object từ ScriptableObject
        string currentDirection = "";
        AnimationFrameRange selectedRange = null;

        if (Mathf.Abs(moveDir.y) > Mathf.Abs(moveDir.x))
        {
            if (moveDir.y > 0.1f) // Đang đi lên
            {
                currentDirection = "RunUp";
                selectedRange = unitData.animations.runUp;
            }
            else // Đang đi xuống
            {
                currentDirection = "RunDown";
                selectedRange = unitData.animations.runDown;
            }
        }
        else // Đang đi ngang
        {
            currentDirection = "RunHorizontal";
            selectedRange = unitData.animations.run;
        }

        // Nếu hướng đi vẫn trùng với frame trước, KHÔNG GỌI LẠI ANIMATOR
        if (lastPlayedAnimDirection == currentDirection) return;

        // Ghi nhớ hướng đi mới
        lastPlayedAnimDirection = currentDirection;

        // Gọi Animator thực hiện đổi hành động (Truyền Object cấu hình bọc list Offset bên trong)
        string id = unitData.unitName;
        string prefix = unitData.animations.animPrefix; // Sử dụng prefix động từ SO thay vì fix cứng "goblin_"
        float frameRate = unitData.animations.frameRate;

        EnemySpriteAnimator.Instance.PlayAnimationByRange(gameObject, id, prefix, selectedRange, frameRate);
    }

    // --- CHECK ĐIỀU KIỆN TẤN CÔNG ---
    public bool IsTargetInAttackRange()
    {
        if (target == null) return false;

        float distance = Vector2.Distance(transform.position, target.position);
        return distance <= unitData.attackRange;
    }

    // --- HÀM NHẬN SÁT THƯƠNG ĐỂ KIỂM TRA TRẠNG THÁI CHẾT ---
    public void TakeDamage(float amount)
    {
        if (isDead) return;
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            TransitionToState(DeathState);
        }
    }

    private void ReachedEndOfTheLine()
    {
        OnEnemyDestroyed?.Invoke();
        Debug.Log($"🏰 [{name}] Đã lọt vào nhà chính! Trừ máu của người chơi.");
        isDead = true;
        gameObject.SetActive(false);
    }
    public void SetupWayPoints(Transform road)
    {
        for(int i = 0; i < road.childCount; i++)
        {
            waypoints.Add(road.GetChild(i));
        }
    }
}