using UnityEngine;

[CreateAssetMenu(fileName = "SlowCurseSO",
    menuName = "Kingdom Rush/Upgrades/Magic/SlowCurseSO")]
public class SlowCurseSO : BaseUpgradeData
{
    public int slowSpeed = 50; // giảm 50% tốc độ của enemy
    public override void ApplyUpgrade()
    {

    }
}
