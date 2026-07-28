using UnityEngine;

public class AlleriaLevelUpState : UnitBaseState
{
    private HeroAlleriaController hero;
    private bool isLevelUpFinished;

    public AlleriaLevelUpState(HeroAlleriaController stateMachine) : base(stateMachine)
    {
        this.hero = stateMachine;
    }

    public override void Enter()
    {
        isLevelUpFinished = false;

        if (hero.baseUnitAnimationHandler is CharacterAnimationHandler customHandler)
        {
            customHandler.PlayLevelUpAnimation(
                hero.unitData.animations,
                hero.AlleriaData.heroAnimations,
                hero.spriteObject,
                onComplete: () =>
                {
                    isLevelUpFinished = true;
                }
            );
        }
        else
        {
            isLevelUpFinished = true;
        }
    }

    public override void Update()
    {
        if (isLevelUpFinished)
        {
            if (hero.currentTarget != null && hero.IsTargetInAttackRange())
            {
                hero.TransitionToState(hero.AttackState);
            }
            else
            {
                hero.TransitionToState(hero.IdleState);
            }
        }
    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {
        isLevelUpFinished = false;
    }
}