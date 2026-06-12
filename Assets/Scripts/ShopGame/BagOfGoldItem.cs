using UnityEngine;

[CreateAssetMenu(fileName = "New BagOfGoldItem Potion", menuName = "Kingdom Rush/Items/BagOfGold")]
public class BagOfGoldItem : ItemAbstract
{
    [Header("Thông số đặc trưng")]
    public int goldAddGame = 500; // thêm 500 vàng vào game khi sử dụng
    public override void Use(Vector3 spawnPosition)
    {
        
    }
}
