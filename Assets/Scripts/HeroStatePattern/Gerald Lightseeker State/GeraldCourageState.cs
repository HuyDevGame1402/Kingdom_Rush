using UnityEngine;

public class GeraldCourageState : UnitBaseState
{
    private HeroGeraldController hero;
    private bool isSkillFinished;

    public GeraldCourageState(HeroGeraldController stateMachine) : base(stateMachine)
    {
        this.hero = stateMachine;
    }

    public override void Enter()
    {
        isSkillFinished = false;

        // Gọi animation gõ khiên
        if (hero.baseUnitAnimationHandler is CharacterAnimationHandler customHandler)
        {
            customHandler.PlayCourageSkillAnimation(
                hero.unitData.animations,
                hero.GeraldData.heroAnimations,
                hero.spriteObject,
                onComplete: () =>
                {
                    // Khi animation kết thúc -> Thi triển hiệu ứng Buff
                    hero.ApplyCourageBuff();
                    isSkillFinished = true;
                }
            );
        }
    }

    public override void Update()
    {
        if (isSkillFinished)
        {
            // Trở về Idle hoặc Attack tùy thuộc mục tiêu
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