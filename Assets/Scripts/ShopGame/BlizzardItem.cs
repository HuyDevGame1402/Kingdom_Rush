using UnityEngine;

[CreateAssetMenu(fileName = "New BlizzardItem Potion", menuName = "Kingdom Rush/Items/Blizzard")]
public class BlizzardItem : ItemAbstract
{
    [Header("Thông số đặc trưng")]
    public int freezingTime = 5; // đóng băng 5s
    public override void Use(Vector3 spawnPosition)
    {
        // đóng băng tất cả enemy trên map
    }
}
