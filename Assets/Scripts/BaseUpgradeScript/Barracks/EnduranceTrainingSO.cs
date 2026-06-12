using UnityEngine;

[CreateAssetMenu(fileName = "EnduranceTrainingSO",
    menuName = "Kingdom Rush/Upgrades/Barracks/EnduranceTrainingSO")]
public class EnduranceTrainingSO : BaseUpgradeData
{
    public int healthBuff = 10; // tăng 10% health lính trụ tối đa 20%
    public override void ApplyUpgrade()
    {

    }
}
