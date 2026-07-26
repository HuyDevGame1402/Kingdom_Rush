using UnityEngine;

[CreateAssetMenu(fileName = "NewHeroGeraldLightseekerData", menuName = "KingdomRush/Hero Data")]
public class HeroGeraldLightseekerDataSO : UnitDataSO
{
    [Header("Hero Specific Animations")]
    public GeraldLightseekerAnimationConfig heroAnimations;

    [Header("Hero Stats Progression")]
    public float healthPerLevel = 20f;
    public float damagePerLevel = 2f;
}
