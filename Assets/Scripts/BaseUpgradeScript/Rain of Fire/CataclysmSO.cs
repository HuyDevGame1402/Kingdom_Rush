using UnityEngine;

[CreateAssetMenu(fileName = "CataclysmSO",
    menuName = "Kingdom Rush/Upgrades/RainofFire/CataclysmSO")]
public class CataclysmSO : BaseUpgradeData
{

    public int minDamage = 150;
    public int maxDamage = 180;

    public int addMeteorite = 5; // thêm 5 viên

    public override void ApplyUpgrade()
    {

    }
}
