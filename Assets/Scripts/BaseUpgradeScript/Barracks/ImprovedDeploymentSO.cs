using UnityEngine;

[CreateAssetMenu(fileName = "ImprovedDeploymentSO",
    menuName = "Kingdom Rush/Upgrades/Barracks/ImprovedDeploymentSO")]
public class ImprovedDeploymentSO : BaseUpgradeData
{
    public int pointRange = 20; // tăng 20% điểm tập kết
    public int timeReduced = 3; // giảm 3s thời gian hồi sinh
    public override void ApplyUpgrade()
    {

    }
}