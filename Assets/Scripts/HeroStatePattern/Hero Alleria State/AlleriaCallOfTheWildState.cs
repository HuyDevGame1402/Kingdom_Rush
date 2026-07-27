using UnityEngine;

public class AlleriaCallOfTheWildState : UnitBaseState
{
    private HeroAlleriaController hero;
    private bool isSkillFinished;

    public AlleriaCallOfTheWildState(HeroAlleriaController stateMachine) : base(stateMachine)
    {
        this.hero = stateMachine;
    }

    public override void Enter()
    {
        isSkillFinished = false;

        if (hero.baseUnitAnimationHandler is CharacterAnimationHandler customHandler)
        {
            customHandler.PlayCallOfTheWildSkillAnimation(
                hero.unitData.animations,
                hero.AlleriaData.heroAnimations,
                hero.spriteObject,
                onComplete: () =>
                {
                    hero.SummonWildcat();
                    isSkillFinished = true;
                }
            );
        }
        else
        {
            hero.SummonWildcat();
            isSkillFinished = true;
        }
    }

    public override void Update()
    {
        if (isSkillFinished)
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

    public override void Exit() { }
}