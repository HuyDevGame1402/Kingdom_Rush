using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDeathState : UnitBaseState
{
    public UnitDeathState(BaseUnitStateMachine unit) : base(unit) { }

    public override void Enter()
    {
        var config = unit.unitData.animations.death;

        SpriteSheetAnimator.Instance.PlayAnimation(
            target: unit.spriteObject,
            animPrefix: unit.unitData.animations.animPrefix,
            startFrame: config.startFrame,
            endFrame: config.endFrame,
            frameRate: -1f,
            onComplete: () => {
                // Chết xong phim thì tắt hẳn Object hoặc hủy xác
                unit.gameObject.SetActive(false);
            }
        );
    }

    public override void Update() { } // Đã chết thì đóng băng mọi logic
    public override void Exit() { }
}
