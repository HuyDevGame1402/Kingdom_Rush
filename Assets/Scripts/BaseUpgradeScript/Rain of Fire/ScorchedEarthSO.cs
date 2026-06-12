using UnityEngine;

[CreateAssetMenu(fileName = "ScorchedEarthSO",
    menuName = "Kingdom Rush/Upgrades/RainofFire/ScorchedEarthSO")]
public class ScorchedEarthSO : BaseUpgradeData
{

    public int timeBurn = 5; // 5s đốt cháy mặt đất
    public int minDamageBurn = 10;
    public int maxDamageBurn = 20;
    public override void ApplyUpgrade()
    {

    }
}