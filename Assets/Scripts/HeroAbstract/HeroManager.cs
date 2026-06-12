using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class HeroManager : MonoBehaviour
{
    public static HeroManager Instance { get; private set; }

    [Header("--- ALL HERO DATA ---")]
    [SerializeField] private List<HeroData> allHeroes; // Kéo thả các file ScriptableObject Hero vào đây (nếu có)

    // Danh sách ID của tất cả 13 Hero
    public List<int> HeroIDs { get; private set; } = new List<int>();

    // Kho lưu trữ ở máy local: Key = heroID, Value = true/false
    public Dictionary<int, bool> HeroInventory { get; private set; } = new Dictionary<int, bool>();

    [SerializeField] private HeroData heroSelected;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PopulateHeroIDs();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Khởi tạo cứng 13 ID từ 1 -> 13 để đảm bảo đồng bộ với Firestore
    private void PopulateHeroIDs()
    {
        HeroIDs.Clear();
        for (int i = 1; i <= 13; i++)
        {
            HeroIDs.Add(i);
        }
    }

    public bool IsHeroOwned(int heroId)
    {
        if (HeroInventory.ContainsKey(heroId))
        {
            return HeroInventory[heroId];
        }
        return false;
    }

    public HeroData GetHeroDataByID(int heroId)
    {
        return allHeroes.Find(h => h.heroID == heroId);
    }

    /// <summary>
    /// Hàm Mua Hero: Chuyển trạng thái sang true và update trực tiếp lên Firestore
    /// </summary>
    public void BuyHero(int heroId)
    {
        if (PlayerManager.Instance == null || string.IsNullOrEmpty(PlayerManager.Instance.PlayerID))
        {
            Debug.LogError("Không tìm thấy PlayerID hợp lệ!");
            return;
        }

        if (!HeroInventory.ContainsKey(heroId))
        {
            Debug.LogError($"Không tồn tại Hero ID = {heroId} trong hệ thống!");
            return;
        }

        if (HeroInventory[heroId])
        {
            Debug.LogWarning("Hero này đã được sở hữu rồi!");
            return;
        }

        string userId = PlayerManager.Instance.PlayerID;
        string fieldName = $"Hero_{heroId}";

        DocumentReference userDocRef = FirebaseFirestore.DefaultInstance.Collection("Players").Document(userId);
        Dictionary<string, object> updateData = new Dictionary<string, object>
        {
            { fieldName, true }
        };

        userDocRef.UpdateAsync(updateData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                HeroInventory[heroId] = true;
                Debug.Log($"[Firestore] Mua thành công Hero ID: {heroId}!");
            }
            else
            {
                Debug.LogError($"[Firestore] Lỗi khi mua Hero ID: {heroId}");
            }
        });
    }
    public void SetHeroSelected(HeroData heroData)
    {
        heroSelected = heroData;
    }
    public HeroData GetHeroSelected()
    {
        return heroSelected;
    }
}