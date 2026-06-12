using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System;

public class ShopItemManager : MonoBehaviour
{
    public static ShopItemManager Instance { get; private set; }

    [Header("--- Kho Vật Phẩm (ID -> Số lượng) ---")]
    // Key: ID Vật phẩm (1: HeartBox, 2: FROZOTOV, 3: DYNAMITE, 4: FAT BOY, 5: GOLD BAG, 6: CHILL WAND)
    // Value: Số lượng người chơi đang sở hữu
    public Dictionary<int, int> ShopInventory = new Dictionary<int, int>();

    // Chuỗi ID để quản lý lưu trữ tiện lợi
    public readonly int[] ItemIDs = { 1, 2, 3, 4, 5, 6 };

    public Action<int, int> EventBuyItem;

    private void Awake()
    {
        // Cơ chế Don't Destroy On Load xuyên Scene
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Khởi tạo sạch Dictionary ban đầu bằng 0
            ResetInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Reset toàn bộ kho đồ về 0
    public void ResetInventory()
    {
        foreach (int id in ItemIDs)
        {
            ShopInventory[id] = 0;
        }
    }

    // Lấy ra số lượng vật phẩm hiện tại theo ID
    public int GetItemCount(int itemId)
    {
        if (ShopInventory.ContainsKey(itemId))
        {
            return ShopInventory[itemId];
        }
        return 0;
    }

    // Hàm thực hiện logic mua Item trong Shop
    public void BuyItem(int itemId, int priceGems)
    {
        // 1. Kiểm tra xem PlayerManager có đủ Gems không (Giả định bạn có Class PlayerManager giữ tổng số Gems)
        if (PlayerManager.Instance.Gems >= priceGems)
        {
            // Trừ tiền nội bộ
            PlayerManager.Instance.Gems -= priceGems;

            // Tăng số lượng trong Dictionary
            if (ShopInventory.ContainsKey(itemId))
            {
                ShopInventory[itemId]++;
            }
            else
            {
                ShopInventory[itemId] = 1;
            }

            Debug.Log($"[Shop] Mua thành công Item ID {itemId}. Số lượng hiện tại: {ShopInventory[itemId]}");

            // 2. Đồng bộ trực tiếp lên Firebase Firestore ngay lập tức để tránh hack
            string pId = PlayerManager.Instance.PlayerID;
            FirebaseFirestore.DefaultInstance.Collection("Players").Document(pId)
                .UpdateAsync(new Dictionary<string, object>
                {
                    { "Gems", PlayerManager.Instance.Gems },
                    { $"Item_{itemId}", ShopInventory[itemId] }
                }).ContinueWithOnMainThread(task => {
                    if (task.IsCompleted)
                    {
                        EventBuyItem?.Invoke(itemId, PlayerManager.Instance.Gems);
                    }
                });
        }
        else
        {
            Debug.LogWarning("[Shop] Không đủ Gems để mua vật phẩm này!");
        }
    }
}