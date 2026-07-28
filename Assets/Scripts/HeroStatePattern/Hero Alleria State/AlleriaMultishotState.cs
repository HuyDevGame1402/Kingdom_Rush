using UnityEngine;

public class AlleriaMultishotState : UnitBaseState
{
    private HeroAlleriaController hero;
    private bool isSkillFinished;

    public AlleriaMultishotState(HeroAlleriaController stateMachine) : base(stateMachine)
    {
        this.hero = stateMachine;
    }

    public override void Enter()
    {
        isSkillFinished = false;

        if (hero.baseUnitAnimationHandler is CharacterAnimationHandler customHandler)
        {
            customHandler.PlayMultishotSkillAnimation(
                hero.unitData.animations,
                hero.AlleriaData.heroAnimations,
                hero.spriteObject,
                onComplete: () =>
                {
                    hero.PerformMultishot();
                    isSkillFinished = true;
                }
            );
        }
        else
        {
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
    public override void FixedUpdate()
    {

    }
    public override void Exit() { }
}