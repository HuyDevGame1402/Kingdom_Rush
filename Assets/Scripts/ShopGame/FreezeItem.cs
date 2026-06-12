using UnityEngine;

[CreateAssetMenu(fileName = "New Freeze Potion", menuName = "Kingdom Rush/Items/Freeze")]
public class FreezeItem : ItemAbstract
{
    [Header("Thông số đặc trưng")]
    public float freezingRange = 5.0f;
    public float freezingTime = 5f;

    public override void Use(Vector3 spawnPosition)
    {
        // Thực tế: Sử dụng Physics2D.OverlapCircle hoặc Raycast tại vị trí spawnPosition để tìm mục tiêu gần nhất và áp dụng hiệu ứng.
    }
}