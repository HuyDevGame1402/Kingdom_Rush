using UnityEngine;

[CreateAssetMenu(fileName = "IndustrializationSO",
    menuName = "Kingdom Rush/Upgrades/Artillery/IndustrializationSO")]
public class IndustrializationSO : BaseUpgradeData
{
    public int priceReduceSkill = 25; // giảm 25% giá nâng cấp skill của tháp
    public override void ApplyUpgrade()
    {

    }
}

