using UnityEngine;

[CreateAssetMenu(fileName = "BlazingSkiesSO",
    menuName = "Kingdom Rush/Upgrades/RainofFire/BlazingSkiesSO")]
public class BlazingSkiesSO : BaseUpgradeData
{

    public int addMeteorite = 2; // thêm 2 thiên thạch
    public int minDamage = 50;
    public int maxDamage = 80;
    public override void ApplyUpgrade()
    {

    }
}

