using UnityEngine;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Extensions;
using System;

public class UpgradesManager : MonoBehaviour
{
    public static UpgradesManager Instance { get; private set; }

    [Header("--- Upgrades Configuration ---")]
    // Danh sách ID từ 1 đến 30 phục vụ cho việc vòng lặp
    public List<int> UpgradeIDs = new List<int>();

    // Dictionary lưu trữ local trạng thái nâng cấp: Key = Upgrade ID, Value = Đã nâng cấp hay chưa (true/false)
    public Dictionary<int, bool> UpgradesInventory = new Dictionary<int, bool>();

    public List<BaseUpgradeData> upgradesData = new List<BaseUpgradeData>();

    public event Action<int, Sprite> OnUpgradePurchased; // Sự kiện khi một upgrade được mua thành công

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeUpgradeIDs();
        }
        else
        {
            Destroy(gameObject);
        }
        upgradesData.Sort((a, b) =>
            int.Parse(a.upgradeID).CompareTo(int.Parse(b.upgradeID)));
    }

    // Khởi tạo danh sách ID từ 1 đến 30
    private void InitializeUpgradeIDs()
    {
        UpgradeIDs.Clear();
        for (int i = 1; i <= 30; i++)
        {
            UpgradeIDs.Add(i);
            UpgradesInventory[i] = false; // Mặc định local ban đầu là false
        }
    }

    /// <summary>
    /// Hàm thực hiện nâng cấp công trình/kỹ năng trong Kingdom
    /// </summary>
    /// <param name="upgradeId">ID của upgrade (1-30)</param>
    /// <param name="starCost">Số sao cần thiết để nâng cấp</param>
    public void UpgradeSkill(int upgradeId, int starCost)
    {
        // 1. Kiểm tra ID hợp lệ
        if (!UpgradesInventory.ContainsKey(upgradeId))
        {
            Debug.LogError($"[UpgradesManager] Không tìm thấy Upgrade ID: {upgradeId}");
            return;
        }

        // 2. Kiểm tra xem đã nâng cấp từ trước chưa
        if (UpgradesInventory[upgradeId])
        {
            Debug.LogWarning($"[UpgradesManager] Upgrade ID {upgradeId} đã được nâng cấp trước đó rồi!");
            return;
        }

        // 3. Kiểm tra PlayerManager và số lượng Sao (TotalStars) hiện tại
        if (PlayerManager.Instance == null)
        {
            Debug.LogError("[UpgradesManager] PlayerManager.Instance đang null!");
            return;
        }

        if (PlayerManager.Instance.TotalStars < starCost)
        {
            Debug.LogWarning($"[UpgradesManager] Không đủ Sao! Cần: {starCost}, Hiện có: {PlayerManager.Instance.TotalStars}");
            return;
        }

        // 4. Đủ điều kiện -> Tiến hành trừ sao ở local trước
        PlayerManager.Instance.TotalStars -= starCost;
        UpgradesInventory[upgradeId] = true;

        // 5. Cập nhật dữ liệu gộp lên Firestore (Cập nhật cả TotalStars mới và trạng thái Upgrade)
        string userId = PlayerManager.Instance.PlayerID;
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("[UpgradesManager] PlayerID trống, không thể lưu lên Firebase!");
            return;
        }

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference userDocRef = db.Collection("Players").Document(userId);

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "TotalStars", PlayerManager.Instance.TotalStars },
            { $"Upgrade_{upgradeId}", true }
        };

        userDocRef.UpdateAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError($"[UpgradesManager] Lỗi khi lưu Upgrade_{upgradeId} lên Firestore!");
                // Rollback dữ liệu local nếu cần thiết tùy logic game của bạn
            }
            else
            {
                OnUpgradePurchased?.Invoke(upgradeId,
                    upgradesData[upgradeId -1].upgradeIcon); // Kích hoạt sự kiện nâng cấp thành công
                Debug.Log($"[UpgradesManager] Nâng cấp thành công ID {upgradeId}. Trừ {starCost} sao. Đã đồng bộ lên Cloud.");
            }
        });
    }
}