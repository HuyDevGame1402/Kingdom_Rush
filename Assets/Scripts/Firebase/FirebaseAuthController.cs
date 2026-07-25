using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

public class FirebaseAuthController : MonoBehaviour
{
    [Header("--- UI References ---")]
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TextMeshProUGUI statusText;

    private FirebaseAuth auth;
    private FirebaseFirestore db;
    private FirebaseUser user;

    void Start()
    {
        statusText.text = "Đang khởi tạo Firebase...";
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                db = FirebaseFirestore.DefaultInstance;
                statusText.text = "<color=green>Firebase đã sẵn sàng!</color>";
            }
            else
            {
                statusText.text = "<color=red>Lỗi kết nối Firebase!</color>";
            }
        });
    }

    public void RegisterButton()
    {

        if (auth == null)
        {
            statusText.text = "<color=yellow>Firebase chưa sẵn sàng, vui lòng đợi giây lát...</color>";
            return;
        }

        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            statusText.text = "<color=yellow>Vui lòng điền đầy đủ thông tin!</color>";
            return;
        }

        statusText.text = "Đang tiến hành đăng ký...";
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                statusText.text = "<color=red>Lỗi đăng ký tài khoản!</color>";
                return;
            }

            user = task.Result.User;
            InitializeNewUserData(user.UserId);
        });
    }

    private void InitializeNewUserData(string userId)
    {
        Dictionary<string, object> initialGameData = new Dictionary<string, object>
        {
            { "PlayerID", userId },
            { "TotalStars", 0 },
            { "Gems", 5000 },
            { "CurrentLevel", 1 }
        };

        // Khởi tạo các loại vật phẩm Shop mặc định = 0
        foreach (int id in ShopItemManager.Instance.ItemIDs)
        {
            initialGameData[$"Item_{id}"] = 0;
        }

        // KHỞI TẠO 13 HERO TRÊN FIRESTORE MẶC ĐỊNH LÀ FALSE
        foreach (int id in HeroManager.Instance.HeroIDs)
        {
            initialGameData[$"Hero_{id}"] = false;
        }

        // ================= THÊM MỚI: KHỞI TẠO 30 UPGRADES TRÊN FIRESTORE LÀ FALSE =================
        if (UpgradesManager.Instance != null)
        {
            foreach (int id in UpgradesManager.Instance.UpgradeIDs)
            {
                initialGameData[$"Upgrade_{id}"] = false;
            }
        }

        db.Collection("Players").Document(userId).SetAsync(initialGameData).ContinueWithOnMainThread(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                statusText.text = "<color=red>Lỗi tạo dữ liệu game!</color>";
            }
            else
            {
                statusText.text = "<color=green>Đăng ký THÀNH CÔNG! Đã khởi tạo dữ liệu.</color>";

                // Đồng bộ Shop local
                foreach (int id in ShopItemManager.Instance.ItemIDs)
                {
                    ShopItemManager.Instance.ShopInventory[id] = 0;
                }

                // ĐỒNG BỘ 13 HERO VÀO LOCAL LÀ FALSE
                foreach (int id in HeroManager.Instance.HeroIDs)
                {
                    HeroManager.Instance.HeroInventory[id] = false;
                }

                // ================= THÊM MỚI: ĐỒNG BỘ 30 UPGRADES VÀO LOCAL LÀ FALSE =================
                if (UpgradesManager.Instance != null)
                {
                    foreach (int id in UpgradesManager.Instance.UpgradeIDs)
                    {
                        UpgradesManager.Instance.UpgradesInventory[id] = false;
                    }
                }

                if (PlayerManager.Instance != null)
                {
                    PlayerManager.Instance.PlayerID = userId;
                    PlayerManager.Instance.Gems = 5000;
                    PlayerManager.Instance.TotalStars = 0;
                    PlayerManager.Instance.CurrentLevel = 1;
                }

                if (LoadSceneManager.Instance != null)
                {
                    LoadSceneManager.Instance.LoadMenuSceneGame();
                }
                else
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(LoadSceneManager.MAIN_MENU_SCENE);
                }
            }
        });
    }

    public void LoginButton()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            statusText.text = "<color=yellow>Vui lòng điền đầy đủ thông tin!</color>";
            return;
        }

        statusText.text = "Đang đăng nhập...";
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                statusText.text = "<color=red>Sai tài khoản hoặc mật khẩu!</color>";
                return;
            }

            user = task.Result.User;
            LoadAndCheckPlayerData(user.UserId);
        });
    }

    private void LoadAndCheckPlayerData(string userId)
    {
        DocumentReference userDocRef = db.Collection("Players").Document(userId);

        userDocRef.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted && task.Result.Exists)
            {
                DocumentSnapshot snapshot = task.Result;
                Dictionary<string, object> updatesForOldUser = new Dictionary<string, object>();

                // 1. Kiểm tra Gems
                int gems = 0;
                if (!snapshot.ContainsField("Gems") || snapshot.GetValue<int>("Gems") == 0)
                {
                    gems = 5000;
                    updatesForOldUser["Gems"] = 5000;
                }
                else
                {
                    gems = snapshot.GetValue<int>("Gems");
                }

                // 2. Kiểm tra các ID vật phẩm Shop bị thiếu
                foreach (int id in ShopItemManager.Instance.ItemIDs)
                {
                    string fieldName = $"Item_{id}";
                    if (!snapshot.ContainsField(fieldName))
                    {
                        updatesForOldUser[fieldName] = 0;
                    }
                }

                // 3. KIỂM TRA TỰ ĐỘNG BÙ ĐỦ 13 TRƯỜNG HERO
                foreach (int id in HeroManager.Instance.HeroIDs)
                {
                    string fieldName = $"Hero_{id}";
                    if (!snapshot.ContainsField(fieldName))
                    {
                        updatesForOldUser[fieldName] = false;
                        Debug.Log($"[Hệ thống] Bổ sung trường {fieldName} = false cho tài khoản cũ.");
                    }
                }

                // ================= THÊM MỚI: TỰ ĐỘNG BÙ ĐỦ 30 TRƯỜNG UPGRADE (Dành cho tài khoản cũ) =================
                if (UpgradesManager.Instance != null)
                {
                    foreach (int id in UpgradesManager.Instance.UpgradeIDs)
                    {
                        string fieldName = $"Upgrade_{id}";
                        if (!snapshot.ContainsField(fieldName))
                        {
                            updatesForOldUser[fieldName] = false; // Bù thiếu mặc định là false
                            Debug.Log($"[Hệ thống] Bổ sung trường {fieldName} = false cho tài khoản cũ.");
                        }
                    }
                }

                // Nếu phát hiện tài khoản cũ bị thiếu trường, cập nhật gộp lên Firestore ngầm
                if (updatesForOldUser.Count > 0)
                {
                    userDocRef.UpdateAsync(updatesForOldUser);
                }

                // 4. Đồng bộ PlayerManager
                if (PlayerManager.Instance != null)
                {
                    PlayerManager.Instance.PlayerID = userId;
                    PlayerManager.Instance.Gems = gems;
                    PlayerManager.Instance.TotalStars = snapshot.ContainsField("TotalStars") ? snapshot.GetValue<int>("TotalStars") : 0;
                    PlayerManager.Instance.CurrentLevel = snapshot.ContainsField("CurrentLevel") ? snapshot.GetValue<int>("CurrentLevel") : 1;
                }

                // 5. Đồng bộ ShopInventory local
                foreach (int id in ShopItemManager.Instance.ItemIDs)
                {
                    string fieldName = $"Item_{id}";
                    int count = snapshot.ContainsField(fieldName) ? snapshot.GetValue<int>(fieldName) : 0;
                    ShopItemManager.Instance.ShopInventory[id] = count;
                }

                // 6. ĐỒNG BỘ TOÀN BỘ 13 TRẠNG THÁI HERO TỪ CLOUD VÀO LOCAL
                foreach (int id in HeroManager.Instance.HeroIDs)
                {
                    string fieldName = $"Hero_{id}";
                    bool isOwned = snapshot.ContainsField(fieldName) ? snapshot.GetValue<bool>(fieldName) : false;
                    HeroManager.Instance.HeroInventory[id] = isOwned;
                }

                // ================= THÊM MỚI: ĐỒNG BỘ TOÀN BỘ 30 TRẠNG THÁI UPGRADE TỪ CLOUD VÀO LOCAL =================
                if (UpgradesManager.Instance != null)
                {
                    foreach (int id in UpgradesManager.Instance.UpgradeIDs)
                    {
                        string fieldName = $"Upgrade_{id}";
                        bool isUpgraded = snapshot.ContainsField(fieldName) ? snapshot.GetValue<bool>(fieldName) : false;
                        UpgradesManager.Instance.UpgradesInventory[id] = isUpgraded;
                    }
                }

                statusText.text = "<color=green>Đăng nhập thành công!</color>";

                if (LoadSceneManager.Instance != null)
                {
                    LoadSceneManager.Instance.LoadMenuSceneGame();
                }
                else
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(LoadSceneManager.MAIN_MENU_SCENE);
                }
            }
            else
            {
                statusText.text = "<color=yellow>Không tìm thấy data, đang khởi tạo mới...</color>";
                InitializeNewUserData(userId);
            }
        });
    }
}