using UnityEngine;

public class GeraldShieldBlockState : UnitBaseState
{
    private HeroGeraldController hero;
    private bool isSkillFinished;

    public GeraldShieldBlockState(HeroGeraldController stateMachine) : base(stateMachine)
    {
        this.hero = stateMachine;
    }

    public override void Enter()
    {
        isSkillFinished = false;
        hero.IsBlocking = true; // Đánh dấu trạng thái miễn/giảm sát thương

        if (hero.baseUnitAnimationHandler is CharacterAnimationHandler customHandler)
        {
            customHandler.PlayShieldBlockAnimation(
                hero.unitData.animations,
                hero.GeraldData.heroAnimations,
                hero.spriteObject,
                onComplete: () =>
                {
                    hero.IsBlocking = false;
                    isSkillFinished = true;
                }
            );
        }
    }

    public override void Update()
    {
        if (isSkillFinished)
        {
            hero.TransitionToState(hero.IdleState);
        }
    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {
        hero.IsBlocking = false;
    }
}