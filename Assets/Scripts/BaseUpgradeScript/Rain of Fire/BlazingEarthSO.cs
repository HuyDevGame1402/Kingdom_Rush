using UnityEngine;

[CreateAssetMenu(fileName = "BlazingEarthSO",
    menuName = "Kingdom Rush/Upgrades/RainofFire/BlazingEarthSO")]
public class BlazingEarthSO : BaseUpgradeData
{

    public int timeBurnDouble = 2; // x2 thời gian đốt
    public int doubleDamage = 2; // x2 sát thương
    public int timeColdDown = 10; // giảm 10s hồi chiêu
    public override void ApplyUpgrade()
    {

    }
}
