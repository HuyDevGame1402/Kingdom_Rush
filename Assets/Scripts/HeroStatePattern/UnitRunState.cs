using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRunState : UnitBaseState
{
    // Biến lưu vị trí đích thực tế sau khi đã tính toán (có thể có offset hoặc không)
    private Vector2 actualTargetPosition;

    public UnitRunState(BaseUnitStateMachine unit) : base(unit) { }

    public override void Enter()
    {
        // Chạy animation di chuyển
        var config = unit.unitData.animations.run;
        SpriteSheetAnimator.Instance.PlayAnimation(unit.spriteObject, unit.unitData.animations.animPrefix, config.startFrame, config.endFrame);

        // --- KHỞI TẠO ĐIỂM ĐÍCH THỰC TẾ ---
        if (unit.currentTarget != null)
        {
            if (unit.IsTargetEnemy())
            {
                // Nếu là Enemy: Đi thẳng tới vị trí hiện tại của Enemy
                actualTargetPosition = unit.currentTarget.position;
            }
            else
            {
                // Nếu KHÔNG PHẢI Enemy (Điểm chỉ định/Rally Point): 
                // Random một điểm xung quanh Target trong bán kính nhỏ (Ví dụ: từ 0.3 đến 0.6 đơn vị) để tránh chụm lại một chỗ
                float randomRadius = Random.Range(0.3f, 0.6f);
                Vector2 randomOffset = Random.insideUnitCircle.normalized * randomRadius;

                actualTargetPosition = (Vector2)unit.currentTarget.position + randomOffset;
            }
        }
    }

    public override void Update()
    {
        // Kiểm tra xem mục tiêu có còn tồn tại không
        if (unit.currentTarget == null || !unit.currentTarget.gameObject.activeSelf)
        {
            Debug.Log($"<color=cyan>[RunState]</color> {unit.gameObject.name} phát hiện currentTarget bằng NULL. Quay về Idle.");
            unit.TransitionToState(unit.IdleState);
            return;
        }

        // Cập nhật vị trí liên tục của Enemy nếu là mục tiêu tấn công (vì Enemy có thể di chuyển)
        if (unit.IsTargetEnemy())
        {
            actualTargetPosition = unit.currentTarget.position;
        }

        Vector2 línhPos2D = unit.transform.position;
        float distance = Vector2.Distance(línhPos2D, actualTargetPosition);

        // --- KIỂM TRA ĐIỀU KIỆN DỪNG LẠI ---
        if (unit.IsTargetEnemy())
        {
            if (distance <= unit.unitData.attackRange)
            {
                unit.TransitionToState(unit.IdleState); // Hoặc chuyển thẳng sang AttackState tùy logic của bạn
                return;
            }
        }
        else
        {
            // Đến điểm Point chỉ định (Vì có offset nên có thể dùng sai số nhỏ 0.1f -> 0.2f)
            if (distance <= 0.15f)
            {
                Debug.Log($"<color=green>[RunState]</color> {unit.gameObject.name} ĐÃ ĐẾN ĐIỂM OFFSET (Khoảng cách tới điểm thực tế: {distance}). Tiến hành xóa target.");
                unit.currentTarget = null;
                unit.TransitionToState(unit.IdleState);
                return;
            }
        }

        // --- DI CHUYỂN ---
        Vector3 direction = ((Vector3)actualTargetPosition - unit.transform.position).normalized;
        unit.transform.position += direction * unit.unitData.moveSpeed * Time.deltaTime;

        // --- LẬT MẶT (FLIP) VÀ SỬA LỖI GIỮ NGUYÊN SCALE ---
        if (direction.x != 0)
        {
            // Cách 1: Sử dụng cấu hình heroScale từ ScriptableObject của bạn (Hãy chắc chắn trong UnitDataSO đang để heroScale = 2)
            float targetScaleX = (direction.x > 0 ? 1 : -1) * unit.unitData.heroScale;
            unit.spriteObject.transform.localScale = new Vector3(targetScaleX, unit.unitData.heroScale, 1);

            /* // Cách 2: Nếu không muốn phụ thuộc ScriptableObject, ép cứng giữ nguyên kích thước trục Y hiện tại (đang là 2)
            float currentAbsoluteScaleY = Mathf.Abs(unit.spriteObject.transform.localScale.y); 
            float targetScaleX = (direction.x > 0 ? 1 : -1) * currentAbsoluteScaleY;
            unit.spriteObject.transform.localScale = new Vector3(targetScaleX, currentAbsoluteScaleY, 1);
            */
        }
    }

    public override void Exit() { }
}