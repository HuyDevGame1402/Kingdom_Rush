using UnityEngine;
using Firebase.Firestore; 
using Firebase.Extensions;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Header("--- Thông Tin Cơ Bản Người Chơi ---")]
    public string PlayerID;
    public int Gems;
    public int TotalStars;
    public int CurrentLevel;
    public event Action<int> OnUpdateGems;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool ConsumeGems(int amount)
    {
        if (Gems >= amount)
        {
            Gems -= amount;
            Debug.Log($"[PlayerManager] Đã trừ {amount} Gems. Số dư hiện tại: {Gems}");
            return true;
        }
        else
        {
            Debug.LogWarning("[PlayerManager] Không đủ Gems để thực hiện giao dịch!");
            return false;
        }
    }
    public void AddGems(int amount, Action<bool> onComplete = null)
    {
        Gems += amount;
        Debug.Log($"[PlayerManager] Đã cộng {amount} Gems. Số dư mới: {Gems}");

        // Nếu chưa có PlayerID (chưa đăng nhập) thì chỉ tăng local rồi chạy callback
        if (string.IsNullOrEmpty(PlayerID))
        {
            onComplete?.Invoke(true);
            return;
        }

        // Đồng bộ ngay lập tức lên Firestore để tránh mất dữ liệu hoặc hack local
        FirebaseFirestore.DefaultInstance.Collection("Players").Document(PlayerID)
            .UpdateAsync("Gems", Gems)
            .ContinueWithOnMainThread(task => {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    Debug.Log("[PlayerManager] Đồng bộ Gems lên Firebase THÀNH CÔNG!");
                    onComplete?.Invoke(true);
                    OnUpdateGems?.Invoke(Gems);
                }
                else
                {
                    Debug.LogError("[PlayerManager] Lỗi đồng bộ Gems lên Firebase!");
                    onComplete?.Invoke(false);
                }
            });
    }
}