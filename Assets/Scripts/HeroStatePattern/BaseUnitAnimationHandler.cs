using UnityEngine;
using System;

public abstract class BaseUnitAnimationHandler : MonoBehaviour
{
    // Các phương thức trừu tượng bắt buộc các Handler con phải triển khai
    public abstract void PlayIdleAnimation(UnitAnimationConfig animData, GameObject target);

    public abstract void PlayRunAnimation(UnitAnimationConfig animData, GameObject target);

    public abstract void PlayAttackAnimation(
        UnitAnimationConfig animData,
        GameObject target,
        Action onEventTrigger,
        Action onComplete, bool isLongAttackRangeCurrent, bool isLongAttackRange
    );

    public abstract void PlayDeathAnimation(
        UnitAnimationConfig animData,
        GameObject target,
        Action onComplete
    );
}