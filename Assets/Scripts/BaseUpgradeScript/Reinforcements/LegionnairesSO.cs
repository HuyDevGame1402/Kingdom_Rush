using UnityEngine;

[CreateAssetMenu(fileName = "LegionnairesSO",
    menuName = "Kingdom Rush/Upgrades/Reinforcements/LegionnairesSO")]
public class LegionnairesSO : BaseUpgradeData
{
    // buff cho nông dân
    public int healthBuff = 110;
    public int armorBuff = 30; // 20% giáp
    public int minDamageBuff = 6;
    public int maxDamageBuff = 10;
    public override void ApplyUpgrade()
    {

    }
}

