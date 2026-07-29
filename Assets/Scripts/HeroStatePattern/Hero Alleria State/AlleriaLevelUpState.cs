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
        CheckLevelBuffWildCat();
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

    private void CheckLevelBuffWildCat()
    {
        if(hero.GetHeroDataInGame().currentLevel < hero.AlleriaData.wildcatStats[1].requiredHeroLevel)
        {
            return;
        }
        // < level 10 và > level 7 vì < 7 đã return trên r
        if(hero.GetHeroDataInGame().currentLevel < hero.AlleriaData.wildcatStats[2].requiredHeroLevel)
        {
            // setup damage vs armor
            UpdateDamageAndArmorForWildCat(hero.AlleriaData.wildcatStats[1]);

            // setup health
            UpdateHealthForWildCat(hero.AlleriaData.wildcatStats[1]);
        }
        // = level 10
        else
        {
            UpdateDamageAndArmorForWildCat(hero.AlleriaData.wildcatStats[2]);
            UpdateHealthForWildCat(hero.AlleriaData.wildcatStats[2]);
        }
    }

    private void UpdateDamageAndArmorForWildCat(WildcatStat wildcatStat)
    {
        hero.GetWildCat().GetComponent<HeroDataInGame>().SetParameters(
                wildcatStat.minDamage,
                wildcatStat.maxDamage,
                0);
    }

    private void UpdateHealthForWildCat(WildcatStat wildcatStat)
    {
        // nếu chết r thì đặt lại max health
        if (hero.GetWildCat().GetComponent<HealthHero>().IsDead())
        {
            hero.GetWildCat().GetComponent<HealthHero>().ChangeMaxHealth(
                wildcatStat.maxHP);
        }
        // nếu chưa thì đặt max health cùng vs reset health về max
        else
        {
            hero.GetWildCat().GetComponent<HealthHero>().InitHealth(
                wildcatStat.maxHP);
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