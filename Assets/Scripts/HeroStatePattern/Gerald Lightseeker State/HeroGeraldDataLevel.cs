using UnityEngine;

public class HeroGeraldDataLevel : MonoBehaviour, IHasDataLevel
{
    [SerializeField] HeroGeraldController heroGeraldController;

    private void Awake()
    {
        if(heroGeraldController == null) heroGeraldController = GetComponent<HeroGeraldController>();
    }

    public HeroLevelStat GetHeroLevelStat(int currentLevel)
    {
        return heroGeraldController.GeraldData.GetStatForLevel(currentLevel);
    }
}
