using System;
using UnityEngine;

public class SpriteSheetAnimationHandler : BaseUnitAnimationHandler
{
    public override void PlayIdleAnimation(UnitAnimationConfig animData, GameObject target)
    {
        var config = animData.idle;
        SpriteSheetAnimator.Instance.PlayAnimation(
            target,
            animData.animPrefix,
            config.startFrame,
            config.endFrame
        );
    }

    public override void PlayRunAnimation(UnitAnimationConfig animData, GameObject target)
    {
        var config = animData.run;
        SpriteSheetAnimator.Instance.PlayAnimation(
            target,
            animData.animPrefix,
            config.startFrame,
            config.endFrame
        );
    }

    public override void PlayAttackAnimation(
        UnitAnimationConfig animData,
        GameObject target,
        Action onEventTrigger,
        Action onComplete)
    {
        AnimationFrameRange config = animData.attacks != null && animData.attacks.Count > 0
            ? animData.GetRandomAttack()
            : animData.attack;

        SpriteSheetAnimator.Instance.PlayAnimation(
            target: target,
            animPrefix: animData.animPrefix,
            startFrame: config.startFrame,
            endFrame: config.endFrame,
            eventFrame: config.eventFrame,
            onEventTrigger: onEventTrigger,
            offsetConfigs: config.animationConfigOffset,
            frameRate: animData.frameRate,
            onComplete: onComplete
        );
    }

    public override void PlayDeathAnimation(UnitAnimationConfig animData, GameObject target, Action onComplete)
    {
        var config = animData.death;
        SpriteSheetAnimator.Instance.PlayAnimation(
            target: target,
            animPrefix: animData.animPrefix,
            startFrame: config.startFrame,
            endFrame: config.endFrame,
            frameRate: -1f,
            onComplete: onComplete
        );
    }
}