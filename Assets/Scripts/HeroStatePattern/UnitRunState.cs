using UnityEngine;

public class UnitRunState : UnitBaseState
{
    private Vector2 actualTargetPosition;

    public UnitRunState(BaseUnitStateMachine unit) : base(unit) { }

    public override void Enter()
    {
        // Chạy animation di chuyển
        unit.baseUnitAnimationHandler.PlayRunAnimation(unit.unitData.animations, unit.spriteObject);

        // Đặt mục tiêu di chuyển
        SetTargetDestination();
    }

    private void SetTargetDestination()
    {
        // ƯU TIÊN 1: Nếu đang di chuyển theo cờ -> Tính vị trí cờ (+ offset ngẫu nhiên)
        if (unit.isRunToFlag)
        {
            if (unit.isHero)
            {
                actualTargetPosition = (Vector2)unit.positionFlag;
            }
            else
            {
                float randomRadius = Random.Range(0.3f, 0.6f);
                Vector2 randomOffset = Random.insideUnitCircle.normalized * randomRadius;
                actualTargetPosition = (Vector2)unit.positionFlag + randomOffset;
            }
        }
        // ƯU TIÊN 2: Nếu không có cờ -> Đi theo target hiện tại (Enemy hoặc Point)
        else if (unit.currentTarget != null)
        {
            if (unit.IsTargetEnemy())
            {
                actualTargetPosition = unit.currentTarget.position;
            }
            else
            {
                float randomRadius = Random.Range(0.3f, 0.6f);
                Vector2 randomOffset = Random.insideUnitCircle.normalized * randomRadius;
                actualTargetPosition = (Vector2)unit.currentTarget.position + randomOffset;
            }
        }
    }

    public override void Update()
    {
        // 1. Mất target VÀ không chạy tới cờ -> Về Idle
        if ((unit.currentTarget == null || !unit.currentTarget.gameObject.activeSelf) && !unit.isRunToFlag)
        {
            unit.TransitionToState(unit.IdleState);
            return;
        }

        // 2. Cập nhật vị trí đuổi theo Enemy (chỉ khi không đi tới cờ)
        if (!unit.isRunToFlag && unit.IsTargetEnemy() && unit.currentTarget != null)
        {
            actualTargetPosition = unit.currentTarget.position;
        }

        Vector2 currentPos = unit.transform.position;
        float distance = Vector2.Distance(currentPos, actualTargetPosition);

        // --------------------------------------------------
        // TRƯỜNG HỢP 1: ĐANG CHẠY ĐẾN CỜ
        // --------------------------------------------------
        if (unit.isRunToFlag)
        {
            if (distance <= 0.15f)
            {
                unit.isRunToFlag = false;
                unit.TransitionToState(unit.IdleState);
                return;
            }
        }
        // --------------------------------------------------
        // TRƯỜNG HỢP 2: TỰ ĐỘNG ĐỦI THEO ĐỊCH
        // --------------------------------------------------
        else if (unit.IsTargetEnemy())
        {
            // --- XỬ LÝ DÀNH CHO UNIT ĐÁNH XA ---
            if (unit.unitData.isLongRangeAttack)
            {
                bool isInLongRange = unit.CheckEnemyInAttackLongRange();

                if (isInLongRange)
                {
                    // Dừng di chuyển ngay lập tức khi quái đã vào tầm đánh xa
                    if (unit.CanAttack())
                    {
                        unit.TransitionToState(unit.AttackState);
                    }
                    else
                    {
                        // Nếu đang chờ Cooldown, đứng đợi (Idle) chứ KHÔNG chạy tiếp vào mặt Enemy
                        unit.TransitionToState(unit.IdleState);
                    }
                    return; // 🛑 Bắt buộc return để KHÔNG chạy xuống đoạn code di chuyển ở dưới
                }
            }

            // --- XỬ LÝ CẬN CHIẾN (Hoặc Unit đánh xa khi Enemy đã áp sát Cận chiến) ---
            bool inAttackRange = distance <= unit.unitData.attackRange;

            if (inAttackRange)
            {
                // Căn chỉnh trục Y với quái trước khi đánh
                if (!unit.IsAlignedWithTarget())
                {
                    Vector2 alignPos = new Vector2(unit.transform.position.x, unit.currentTarget.position.y);
                    Vector2 dirAlign = (alignPos - currentPos).normalized;

                    //unit.transform.position += (Vector3)(dirAlign * unit.unitData.moveSpeed * Time.deltaTime);
                    unit.rb.MovePosition(
                        unit.rb.position +
                        dirAlign * unit.unitData.moveSpeed * Time.fixedDeltaTime);
                    if (dirAlign.x != 0)
                    {
                        float scaleX = (dirAlign.x > 0 ? 1 : -1) * unit.unitData.heroScale;
                        unit.spriteObject.transform.localScale = new Vector3(scaleX, unit.unitData.heroScale, 1);
                    }
                    return;
                }

                if (unit.CanAttack())
                {
                    unit.TransitionToState(unit.AttackState);
                }
                else
                {
                    unit.TransitionToState(unit.IdleState);
                }
                return;
            }
        }
        // --------------------------------------------------
        // TRƯỜNG HỢP 3: ĐIỂM CHỈ ĐỊNH KHÁC
        // --------------------------------------------------
        else
        {
            if (distance <= 0.15f)
            {
                unit.currentTarget = null;
                unit.TransitionToState(unit.IdleState);
                return;
            }
        }
    }
    public override void FixedUpdate()
    {
        Vector2 direction =
        (actualTargetPosition - unit.rb.position).normalized;

        unit.rb.MovePosition(
            unit.rb.position +
            direction * unit.unitData.moveSpeed * Time.fixedDeltaTime);

        if (direction.x != 0)
        {
            float scaleX = (direction.x > 0 ? 1 : -1) * unit.unitData.heroScale;
            unit.spriteObject.transform.localScale = new Vector3(scaleX, unit.unitData.heroScale, 1);
        }
    }
    public override void Exit() { }
}