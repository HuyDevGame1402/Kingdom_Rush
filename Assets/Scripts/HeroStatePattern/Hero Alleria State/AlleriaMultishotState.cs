using System.Collections.Generic;
using UnityEngine;

public class AlleriaMultishotState : UnitBaseState
{
    private HeroAlleriaController hero;
    private bool isSkillFinished;
    private int finnalDamage;
    private int arrowCount;
    private int enemyCount;

    private List<Transform> enemyListAttack;
    private HeroAlleriaSpawnArrow heroAlleriaSpawnArrow;
    private int index;


    public AlleriaMultishotState(HeroAlleriaController stateMachine) : base(stateMachine)
    {
        this.hero = stateMachine;
    }

    public override void Enter()
    {
        isSkillFinished = false;

        if(heroAlleriaSpawnArrow == null)
        {
            heroAlleriaSpawnArrow = unit.GetComponent<HeroAlleriaSpawnArrow>();
        }

        if (hero.baseUnitAnimationHandler is CharacterAnimationHandler customHandler)
        {
            customHandler.PlayMultishotSkillAnimation(
                hero.unitData.animations,
                hero.AlleriaData.heroAnimations,
                hero.spriteObject,
                onEventTrigger: () =>
                {
                    SpawnMultiArrow();
                },
                onComplete: () =>
                {
                    //hero.PerformMultishot();
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

    private void SpawnMultiArrow()
    {
        if(hero.GetHeroDataInGame().currentLevel >= hero.AlleriaData.multishotLevels[0].requiredHeroLevel
            && hero.GetHeroDataInGame().currentLevel < hero.AlleriaData.multishotLevels[1].requiredHeroLevel)
        {
            arrowCount = hero.AlleriaData.multishotLevels[0].arrowCount;
        }
        else if(hero.GetHeroDataInGame().currentLevel >= hero.AlleriaData.multishotLevels[1].requiredHeroLevel
            && hero.GetHeroDataInGame().currentLevel < hero.AlleriaData.multishotLevels[2].requiredHeroLevel)
        {
            arrowCount = hero.AlleriaData.multishotLevels[1].arrowCount;
        }
        else if (hero.GetHeroDataInGame().currentLevel >= hero.AlleriaData.multishotLevels[2].requiredHeroLevel)
        {
            arrowCount = hero.AlleriaData.multishotLevels[2].arrowCount;
        }
        finnalDamage = DamageStatic.GetDamageBase(hero.GetHeroDataInGame().GetMinRangedDamage(),
                    hero.GetHeroDataInGame().GetMaxRangedDamage());
        enemyCount = hero.targetLongRangeList.Count;
        if (enemyCount == 0) return;
        if (enemyCount <= arrowCount)
        {
            finnalDamage = (int)((finnalDamage * arrowCount) / hero.targetLongRangeList.Count);
            for(int i = 0; i < enemyCount; i++)
            {
                Debug.LogWarning("Attack Skill Multi Arrow " + "Damage cuối là: " + finnalDamage);
                heroAlleriaSpawnArrow.SpawnBullet(hero.targetLongRangeList[i], finnalDamage);
            }
        }
        else
        {
            enemyListAttack = new List<Transform>(hero.targetLongRangeList);

            for(int i = 0; i < arrowCount; i++)
            {
                if (enemyListAttack.Count == 0) break;
                index = Random.Range(0, enemyListAttack.Count);
                heroAlleriaSpawnArrow.SpawnBullet(enemyListAttack[index], finnalDamage);
                enemyListAttack.RemoveAt(index);
            }
        }
    }

    public override void FixedUpdate()
    {

    }
    public override void Exit() { }
}