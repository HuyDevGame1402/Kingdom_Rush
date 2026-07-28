using UnityEngine;
using System.Collections.Generic;

public class TriggerHeroInSide : MonoBehaviour
{
    [SerializeField] private List<BaseUnitStateMachine> soliderList = new List<BaseUnitStateMachine>();
    private int countCheck;

    public void AddSolider(BaseUnitStateMachine solider)
    {
        if (soliderList.Contains(solider)) return;
        soliderList.Add(solider);
    }

    public void RemoveSolider(BaseUnitStateMachine solider)
    {
        if (soliderList.Contains(solider))
        {
            soliderList.Remove(solider);
        }
    }

    public bool CheckCountSolider(int count)
    {
        if(soliderList.Count < count || soliderList.Count == 0)
        {
            return false;
        }
        countCheck = count;
        for(int i = 0; i < soliderList.Count; i++)
        {
            if (soliderList[i] == transform.GetComponent<BaseUnitStateMachine>())
            {
                continue;
            }
            if(soliderList[i] != null && soliderList[i].isDead == false)
            {
                countCheck -= 1;
                if(countCheck == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void AddBuffForSoliderInSide(StatModifier newMod, float healthBuffMax)
    {
        for (int i = 0; i < soliderList.Count; i++)
        {
            if (soliderList[i] != null && soliderList[i].isDead == false)
            {
                var modCopy = new StatModifier(newMod.sourceID, newMod.valueDamage, newMod.valueArmor, newMod.duration);
                soliderList[i].GetComponent<HeroDataInGame>().AddModifier(modCopy);
                soliderList[i].GetComponent<Health>().BuffHealthWithPercentMaxHealth(healthBuffMax);
                if (soliderList[i].vfxHero != null)
                {
                    soliderList[i].vfxHero.PlayAnimationHeroBarrackBuff(newMod.duration);
                }
            }
        }
    }
}
