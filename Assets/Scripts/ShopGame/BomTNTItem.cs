using UnityEngine;

[CreateAssetMenu(fileName = "New BomTNT Potion", menuName = "Kingdom Rush/Items/BomTNT")]
public class BomTNTItem : ItemAbstract
{
    [Header("Thông số đặc trưng")]
    public float singleBombDuration = 5.0f;

    public override void Use(Vector3 spawnPosition)
    {
        // Thực tế: Sử dụng Physics2D.OverlapCircle hoặc Raycast tại vị trí spawnPosition để tìm mục tiêu gần nhất và áp dụng hiệu ứng.
    }
}
