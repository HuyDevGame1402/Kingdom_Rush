using System;
using UnityEngine;

public class CharacterAnimationHandler : BaseUnitAnimationHandler
{
    [SerializeField] private string animationID;

    public string AnimationID
    {
        get => animationID;
        set => animationID = value;
    }

    public override void PlayIdleAnimation(UnitAnimationConfig animData, GameObject target)
    {
        var config = animData.idle;
        CharacterSpriteAnimator.Instance.PlayAnimationByRange(
            target: target,
            enemyId: animationID,
            animPrefix: animData.animPrefix,
            rangeConfig: config,
            frameRate: animData.frameRate,
            null, null
        );
    }

    public override void PlayRunAnimation(UnitAnimationConfig animData, GameObject target)
    {
        var config = animData.run;
        CharacterSpriteAnimator.Instance.PlayAnimationByRange(
            target: target,
            enemyId: animationID,
            animPrefix: animData.animPrefix,
            rangeConfig: config,
            frameRate: animData.frameRate,
            null,null
        );
    }

    public override void PlayAttackAnimation(
    UnitAnimationConfig animData,
    GameObject target,
    Action onEventTrigger,
    Action onComplete)
    {
        AnimationFrameRange config = (animData.attacks != null && animData.attacks.Count > 0)
            ? animData.GetRandomAttack()
            : animData.attack;

        // Bổ sung tham số onEventTrigger vào call
        CharacterSpriteAnimator.Instance.PlayAnimationByRange(
            target: target,
            enemyId: animationID,
            animPrefix: animData.animPrefix,
            rangeConfig: config,
            frameRate: animData.frameRate,
            onEventTrigger: onEventTrigger, // <-- THÊM DÒNG NÀY
            onComplete: onComplete
        );
    }

    public override void PlayDeathAnimation(UnitAnimationConfig animData, GameObject target, Action onComplete)
    {
        var config = animData.death;
        CharacterSpriteAnimator.Instance.PlayAnimationByRange(
            target: target,
            enemyId: animationID,
            animPrefix: animData.animPrefix,
            rangeConfig: config,
            frameRate: animData.frameRate,
            onComplete: onComplete
        );
    }

    public void PlayCourageSkillAnimation(UnitAnimationConfig baseAnim, GeraldLightseekerAnimationConfig heroAnim, GameObject target, Action onComplete)
    {
        CharacterSpriteAnimator.Instance.PlayAnimationByRange(
            target: target,
            enemyId: animationID,
            animPrefix: baseAnim.animPrefix,
            rangeConfig: heroAnim.courageSkill,
            frameRate: baseAnim.frameRate,
            onComplete: onComplete
        );
    }

    public void PlayShieldBlockAnimation(UnitAnimationConfig baseAnim, GeraldLightseekerAnimationConfig heroAnim, GameObject target, Action onComplete)
    {
        CharacterSpriteAnimator.Instance.PlayAnimationByRange(
            target: target,
            enemyId: animationID,
            animPrefix: baseAnim.animPrefix,
            rangeConfig: heroAnim.shieldBlock,
            frameRate: baseAnim.frameRate,
            onComplete: onComplete
        );
    }
    public void PlayLevelUpAnimation(
        UnitAnimationConfig baseConfig,
        GeraldLightseekerAnimationConfig heroConfig,
        GameObject spriteObject,
        Action onComplete)
    {
        var config = heroConfig.levelUp;

        // Chuyển sang dùng CharacterSpriteAnimator đồng bộ với toàn bộ class
        CharacterSpriteAnimator.Instance.PlayAnimationByRange(
            target: spriteObject,
            enemyId: animationID,
            animPrefix: baseConfig.animPrefix,
            rangeConfig: config,
            frameRate: baseConfig.frameRate,
            onEventTrigger: null,
            onComplete: onComplete
        );
    }
}