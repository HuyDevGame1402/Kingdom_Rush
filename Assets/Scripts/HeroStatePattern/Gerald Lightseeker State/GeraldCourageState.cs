using UnityEngine;

public class GeraldCourageState : UnitBaseState
{
    private HeroGeraldController hero;
    private bool isSkillFinished;

    private StatModifier statModifier;
    private float percentHealthBuffMax;


    public GeraldCourageState(HeroGeraldController stateMachine) : base(stateMachine)
    {
        this.hero = stateMachine;
    }

    public override void Enter()
    {
        isSkillFinished = false;
        SetStatModifier();
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

    private void SetStatModifier()
    {
        if(hero.GetHeroDataInGame().currentLevel >=
            hero.GeraldData.courageSkillStats[0].requiredHeroLevel && 
            hero.GetHeroDataInGame().currentLevel <
            hero.GeraldData.courageSkillStats[1].requiredHeroLevel)
        {
            BuffCouregeForHero(hero.GeraldData.courageSkillStats[0]);
        }
        else if (hero.GetHeroDataInGame().currentLevel >=
            hero.GeraldData.courageSkillStats[1].requiredHeroLevel &&
            hero.GetHeroDataInGame().currentLevel <
            hero.GeraldData.courageSkillStats[2].requiredHeroLevel)
        {
            BuffCouregeForHero(hero.GeraldData.courageSkillStats[1]);
        }
        else if(hero.GetHeroDataInGame().currentLevel >=
            hero.GeraldData.courageSkillStats[2].requiredHeroLevel)
        {
            BuffCouregeForHero(hero.GeraldData.courageSkillStats[2]);
        }
    }

    private void BuffCouregeForHero(CourageSkillLevelStat courageSkillLevelStat)
    {
        hero.BuffCourageSkillForHero(
                new StatModifier("Gerald_Courage",
                courageSkillLevelStat.damageBoost,
                courageSkillLevelStat.armorBoostPercent,
                courageSkillLevelStat.duration),
                courageSkillLevelStat.healBoostPercent);
    }
    public override void FixedUpdate()
    {

    }
}