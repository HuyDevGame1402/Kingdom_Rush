using UnityEngine;

[CreateAssetMenu(fileName = "BetterArmorSO",
    menuName = "Kingdom Rush/Upgrades/Barracks/BetterArmorSO")]
public class BetterArmorSO : BaseUpgradeData
{
    public int armorBuff = 10; // tăng 10% giáp lính trụ
    public override void ApplyUpgrade()
    {

    }
}

