using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDeathState : UnitBaseState
{
    public UnitDeathState(BaseUnitStateMachine unit) : base(unit) { }

    public override void Enter()
    {
        unit.baseUnitAnimationHandler.PlayDeathAnimation(
            unit.unitData.animations,
            unit.spriteObject,
            onComplete: () => {
                unit.gameObject.SetActive(false);
            }
        );
    }

    public override void Update() { } // Đã chết thì đóng băng mọi logic
    public override void Exit() { }
}
