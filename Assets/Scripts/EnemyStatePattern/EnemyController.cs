using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

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
    public List<Transform> targetList = new List<Transform>();

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
    public bool IsMoving => currentState == MoveState;
    public Transform centerEnemy;

    [SerializeField] private Health enemyHealth;
    [SerializeField] private Transform healthInGroundSprite;
    [SerializeField] private SpriteRenderer spriteRender;
    

    public float offsetSpawnTextY = 0.5f;

    public int attackerCount;

    public CircleCollider2D colliderTriggerHitDamage;

    private Transform checkTarget;

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
        centerEnemy = transform.GetComponent<EnemyDataScript>().centerEnemy;
        enemyHealth = transform.GetComponent<Health>();

        enemyHealth.InitHealth((int)unitData.maxHealth);
        spriteRender = transform.GetComponent<SpriteRenderer>();
        colliderTriggerHitDamage = transform.GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (isDead) return;

        CleanDeadTargets();

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
    public void TakeDamage(int amount, TextSO textSO)
    {
        if (isDead) return;
        if(enemyHealth != null)
        {
            enemyHealth.ApplyDamage(amount);
            if (enemyHealth.IsDead())
            {
                colliderTriggerHitDamage.enabled = false;
                TextSpawnManager.Instance.SpawnText(transform.position + Vector3.up * offsetSpawnTextY, 
                    textSO.sprites[UnityEngine.Random.Range(0, textSO.sprites.Count)]);
                if (target != null && target.TryGetComponent(out BaseUnitStateMachine heroStateMachine))
                {
                    heroStateMachine.RemoveAttacker();
                }
                attackerCount = 0; // Reset số lượng attacker khi chết
                TransitionToState(DeathState);
                targetList.Clear(); // Xóa danh sách mục tiêu khi chết
            }
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

    public Vector3 GetFuturePosition(float seconds)
    {
        if (isDead)
            return centerEnemy != null ? centerEnemy.position : transform.position;

        if (waypoints == null || waypoints.Count == 0)
            return centerEnemy != null ? centerEnemy.position : transform.position;

        Vector3 centerOffset = centerEnemy != null
            ? centerEnemy.position - transform.position
            : Vector3.zero;

        float remainDistance = unitData.moveSpeed * seconds;

        Vector3 currentPos = transform.position;
        int wpIndex = currentWaypointIndex;

        if(currentState == AttackState)
        {
            return currentPos + centerOffset;
        }

        while (remainDistance > 0f)
        {
            if (wpIndex >= waypoints.Count)
                return currentPos + centerOffset;

            Vector3 nextPoint = waypoints[wpIndex].position;

            float distance = Vector3.Distance(currentPos, nextPoint);

            if (distance > remainDistance)
            {
                Vector3 futurePos =
                    currentPos +
                    (nextPoint - currentPos).normalized * remainDistance;

                return futurePos + centerOffset;
            }

            remainDistance -= distance;
            currentPos = nextPoint;
            wpIndex++;
        }

        return currentPos + centerOffset;
    }
    // Thêm hàm này vào trong class EnemyController
    public bool IsAlignedWithTarget(float tolerance = 0.05f)
    {
        if (target == null) return false;
        return Mathf.Abs(transform.position.y - target.position.y) <= tolerance;
    }
    public void ResetTarget()
    {
        if (targetList.Contains(target))
        {
            targetList.Remove(target);
        }
        if (target != null && target.TryGetComponent(out BaseUnitStateMachine heroStateMachine))
        {
            heroStateMachine.RemoveAttacker();
        }
        target = null;
        if (targetList.Count > 0)
        {
            for(int i = 0; i < targetList.Count; i++)
            {
                if (targetList[i].TryGetComponent(out BaseUnitStateMachine heroStateMachineList)
                    && heroStateMachineList.CheckAttackerCount())
                {
                    target = targetList[i];
                    return;
                }
            }
        }
    }
    public bool CheckAttackerCount()
    {
        return attackerCount < unitData.maxAttacker;
    }
    public void RemoveAttacker()
    {
        attackerCount -= 1;
        if (attackerCount < 0)
        {
            attackerCount = 0;
        }
    }

    public void ShowHealthInGround()
    {
        StartCoroutine(CoroutineHealthinGround());
    }

    private IEnumerator CoroutineHealthinGround()
    {
        healthInGroundSprite.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        spriteRender.enabled = false;
        yield return new WaitForSeconds(1f);
        spriteRender.enabled = true;
        healthInGroundSprite.gameObject.SetActive(false);
        transform.gameObject.SetActive(false);
    }
    private void CleanDeadTargets()
    {
        for (int i = targetList.Count - 1; i >= 0; i--)
        {
            checkTarget = targetList[i];

            if (checkTarget == null || !checkTarget.gameObject.activeInHierarchy ||
               (checkTarget.TryGetComponent(out BaseUnitStateMachine hero) && hero.isDead))
            {
                if (checkTarget == target)
                {
                    target = null;
                }
                targetList.RemoveAt(i);
            }
        }
        if (target == null && targetList.Count > 0)
        {
            ResetTarget();
        }
    }
    // --- TÍNH TOÁN LẠI WAYPOINT GẦN NHẤT KHI RỜI TRẬN ĐẤU ---
    public void RecalculateNextWaypoint()
    {
        if (waypoints == null || waypoints.Count == 0 || currentWaypointIndex >= waypoints.Count)
            return;

        int bestIndex = currentWaypointIndex;
        float minDistance = float.MaxValue;

        // Để an toàn cho các map có đường vòng vèo (nút cổ chai), ta nên quét từ 
        // các waypoint xung quanh vị trí cũ (ví dụ: lùi lại 1 điểm cho chắc chắn)
        int startIndex = Mathf.Max(0, currentWaypointIndex - 1);

        for (int i = startIndex; i < waypoints.Count - 1; i++)
        {
            Vector3 wStart = waypoints[i].position;
            Vector3 wEnd = waypoints[i + 1].position;

            // Tìm điểm gần nhất trên đoạn thẳng từ wStart đến wEnd so với vị trí của Enemy
            Vector3 closestPoint = ClosestPointOnLineSegment(transform.position, wStart, wEnd);
            float dist = Vector3.Distance(transform.position, closestPoint);

            if (dist < minDistance)
            {
                minDistance = dist;
                bestIndex = i + 1; // Điểm đến tiếp theo chính là điểm cuối của đoạn đường này
            }
        }

        currentWaypointIndex = bestIndex;
    }

    // Hàm phụ trợ tìm điểm gần nhất trên một đoạn thẳng (Line Segment)
    private Vector3 ClosestPointOnLineSegment(Vector3 point, Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        float lengthSq = direction.sqrMagnitude;
        if (lengthSq == 0f) return start;

        // Chiếu vị trí Enemy lên đường thẳng tạo bởi 2 waypoint
        float t = Vector3.Dot(point - start, direction) / lengthSq;
        t = Mathf.Clamp01(t); // Giới hạn chỉ nằm trong đoạn thẳng nối giữa 2 điểm

        return start + t * direction;
    }
    public void ResetAnimDirection()
    {
        lastPlayedAnimDirection = "";
    }
}