using UnityEngine;

public class HeroAlleriaSwiftwindLevelData : MonoBehaviour, IHasDataLevel
{
    [SerializeField] HeroAlleriaController heroAlleriaController;

    private void Awake()
    {
        if (heroAlleriaController == null) heroAlleriaController = GetComponent<HeroAlleriaController>();
    }

    public HeroLevelStat GetHeroLevelStat(int currentLevel)
    {
        return heroAlleriaController.AlleriaData.GetStatForLevel(currentLevel);
    }
}
