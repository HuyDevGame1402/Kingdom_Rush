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
        Action onComplete, bool isLongAttackRangeCurrent, bool isLongAttackRange)
    {
        AnimationFrameRange config;
        if (isLongAttackRangeCurrent == false && isLongAttackRange == false)
        {
            config = (animData.attacks != null && animData.attacks.Count > 0)
                   ? animData.GetRandomAttack()
                   : animData.attack;
        }
        else if (isLongAttackRange && isLongAttackRangeCurrent)
        {
            // tại index = 0 thì chính là attack đánh xa
            config = animData.attacks[0];
        }
        else if (isLongAttackRangeCurrent == false && isLongAttackRange == true)
        {
            config = animData.attacks[1];
        }
        // thêm vào cho có chứ k rơi vào nhánh này
        else
        {
            config = animData.attacks[0];
        }

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