using UnityEngine;

[CreateAssetMenu(fileName = "WarriorsSO",
    menuName = "Kingdom Rush/Upgrades/Reinforcements/WarriorsSO")]
public class WarriorsSO : BaseUpgradeData
{
    // buff cho nông dân
    public int healthBuff = 90;
    public int armorBuff = 20; // 20% giáp
    public int minDamageBuff = 3;
    public int maxDamageBuff = 6;
    public override void ApplyUpgrade()
    {

    }
}
