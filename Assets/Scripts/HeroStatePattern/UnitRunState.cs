using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRunState : UnitBaseState
{
    public UnitRunState(BaseUnitStateMachine unit) : base(unit) { }

    public override void Enter()
    {
        var config = unit.unitData.animations.run;
        SpriteSheetAnimator.Instance.PlayAnimation(unit.spriteObject, unit.unitData.animations.animPrefix, config.startFrame, config.endFrame);
    }

    //public override void Update()
    //{
    //    // Không có mục tiêu -> Quay về đứng im ngay
    //    if (unit.currentTarget == null || !unit.currentTarget.gameObject.activeSelf)
    //    {
    //        unit.TransitionToState(unit.IdleState);
    //        return;
    //    }

    //    Vector2 línhPos2D = unit.transform.position;
    //    Vector2 mụcTiêuPos2D = unit.currentTarget.position;
    //    float distance = Vector2.Distance(línhPos2D, mụcTiêuPos2D);

    //    if (unit.IsTargetEnemy())
    //    {
    //        // Gặp địch trong tầm chém -> Dừng lại để chuẩn bị Đánh
    //        if (distance <= unit.unitData.attackRange)
    //        {
    //            unit.TransitionToState(unit.IdleState);
    //            return;
    //        }
    //    }
    //    else
    //    {
    //        // Đến điểm Point chỉ định (Sai số an toàn 0.2f)
    //        if (distance <= 0.2f)
    //        {
    //            unit.currentTarget = null; // Bẻ gãy liên kết mục tiêu cũ để không bị lặp logic
    //            unit.TransitionToState(unit.IdleState); // Ép về đứng im
    //            return;
    //        }
    //    }

    //    // --- THỰC HIỆN DI CHUYỂN TỊNH TIẾN ---
    //    Vector3 direction = ((Vector3)mụcTiêuPos2D - unit.transform.position).normalized;
    //    unit.transform.position += direction * unit.unitData.moveSpeed * Time.deltaTime;

    //    // Xoay mặt Sprite
    //    if (direction.x != 0)
    //    {
    //        unit.spriteObject.transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
    //    }
    //}
    public override void Update()
    {
        if (unit.currentTarget == null || !unit.currentTarget.gameObject.activeSelf)
        {
            Debug.Log($"<color=cyan>[RunState]</color> {unit.gameObject.name} phát hiện currentTarget bằng NULL. Quay về Idle.");
            unit.TransitionToState(unit.IdleState);
            return;
        }

        Vector2 línhPos2D = unit.transform.position;
        Vector2 mụcTiêuPos2D = unit.currentTarget.position;
        float distance = Vector2.Distance(línhPos2D, mụcTiêuPos2D);

        if (unit.IsTargetEnemy())
        {
            if (distance <= unit.unitData.attackRange)
            {
                unit.TransitionToState(unit.IdleState);
                return;
            }
        }
        else
        {
            // Đến điểm Point chỉ định (Sai số an toàn 0.2f)
            if (distance <= 0.2f)
            {
                Debug.Log($"<color=green>[RunState]</color> {unit.gameObject.name} ĐÃ ĐẾN ĐIỂM (Khoảng cách: {distance}). Tiến hành xóa target.");
                unit.currentTarget = null;
                unit.TransitionToState(unit.IdleState);
                return;
            }
        }

        // --- DI CHUYỂN ---
        Vector3 direction = ((Vector3)mụcTiêuPos2D - unit.transform.position).normalized;
        unit.transform.position += direction * unit.unitData.moveSpeed * Time.deltaTime;

        if (direction.x != 0)
        {
            unit.spriteObject.transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
        }
    }

    public override void Exit() { }
}
