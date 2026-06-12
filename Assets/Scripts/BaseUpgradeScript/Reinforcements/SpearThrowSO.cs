using UnityEngine;

[CreateAssetMenu(fileName = "SpearThrowSO",
    menuName = "Kingdom Rush/Upgrades/Reinforcements/SpearThrowSO")]
public class SpearThrowSO : BaseUpgradeData
{
    // buff cho nông dân
    public int healthBuff = 110;
    public int armorBuff = 30; // 20% giáp
    public int minDamageBuff = 6;
    public int maxDamageBuff = 10;
    public int minRangedDamage = 24; // sát thương khi ném giáo 
    public int maxRangedDamage = 40; // nâng cấp 4 lính thường ném giáo đc
    public override void ApplyUpgrade()
    {

    }
}

