using UnityEngine;

[CreateAssetMenu(fileName = "RangeFinderSO",
    menuName = "Kingdom Rush/Upgrades/Artillery/RangeFinderSO")]
public class RangeFinderSO : BaseUpgradeData
{
    public int attackRange = 10; // tăng 10% tầm bắn
    public override void ApplyUpgrade()
    {

    }
}
