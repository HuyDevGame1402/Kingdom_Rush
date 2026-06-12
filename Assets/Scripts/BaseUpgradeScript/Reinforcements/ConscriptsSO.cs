using UnityEngine;

[CreateAssetMenu(fileName = "ConscriptsSO",
    menuName = "Kingdom Rush/Upgrades/Reinforcements/ConscriptsSO")]
public class ConscriptsSO : BaseUpgradeData
{
    // buff cho nông dân
    public int healthBuff = 70;
    public int armorBuff = 10; // 10% giáp
    public int minDamageBuff = 2;
    public int maxDamageBuff = 4;
    public override void ApplyUpgrade()
    {

    }
}