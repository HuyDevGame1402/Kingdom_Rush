using UnityEngine;

[CreateAssetMenu(fileName = "PrecisionSO",
    menuName = "Kingdom Rush/Upgrades/Archer/PrecisionSO")]
public class Precision : BaseUpgradeData
{
    public int doubleDamage = 10; // tăng 10% x2 damage
    public override void ApplyUpgrade()
    {

    }
}
