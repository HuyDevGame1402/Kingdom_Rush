using UnityEngine;

[CreateAssetMenu(fileName = "TheFastFuriousSO",
    menuName = "Kingdom Rush/Upgrades/RainofFire/TheFastFuriousSO")]
public class TheFastFuriousSO : BaseUpgradeData
{

    public int radiusAttack = 25; // 25% sát thương bán kính
    public int minDamage = 90;
    public int maxDamage = 120;
    public int timeColdDown = 10; // giảm 10s hồi chiêu
    public override void ApplyUpgrade()
    {

    }
}