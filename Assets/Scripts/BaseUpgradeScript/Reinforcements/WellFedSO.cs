using UnityEngine;

[CreateAssetMenu(fileName = "WellFedSO",
    menuName = "Kingdom Rush/Upgrades/Reinforcements/WellFedSO")]
public class WellFedSO : BaseUpgradeData
{
    // buff cho nông dân
    public int healthBuff = 50;
    public int minDamageBuff = 1;
    public int maxDamageBuff = 3;
    public override void ApplyUpgrade()
    {

    }
}
