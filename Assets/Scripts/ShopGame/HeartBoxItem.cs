using UnityEngine;

[CreateAssetMenu(fileName = "New HeartBox", menuName = "Kingdom Rush/Items/HeartBox")]
public class HeartBoxItem : ItemAbstract
{
    [Header("Thông số đặc trưng")]
    public int lives = 5; // 5 mạng sống

    public override void Use(Vector3 spawnPosition)
    {
        
    }
}
