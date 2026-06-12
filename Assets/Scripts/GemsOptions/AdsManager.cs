using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdsManager Instance { get; private set; }
    private int gemsAdd = 0;

    [Header("--- Cấu hình Test Chay (Điền bừa số vẫn chạy) ---")]
    [SerializeField] string _androidGameId = "1234567"; // Để số giả lập thoải mái khi bật Test Mode
    [SerializeField] string _iOSGameId = "1234568";
    [SerializeField] bool _testMode = true; // Bắt buộc để TRUE để test chay không cần web dashboard

    private string _gameId;
    private string _rewardedPlacementId = "Rewarded_Android"; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeAds()
    {
#if UNITY_IOS
        _gameId = _iOSGameId;
        _rewardedPlacementId = "Rewarded_iOS";
#elif UNITY_ANDROID
        _gameId = _androidGameId;
        _rewardedPlacementId = "Rewarded_Android";
#else
        // Khi chạy trên Unity Editor máy tính, bắt buộc đồng bộ ID này để test
        _gameId = _androidGameId; 
        _rewardedPlacementId = "Rewarded_Android"; 
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("[Ads] Unity Ads khởi tạo TEST MODE thành công.");
        LoadRewardedAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"[Ads] Khởi tạo thất bại: {error.ToString()} - {message}");
    }

    public void LoadRewardedAd()
    {
        Debug.Log($"[Ads] Đang tải quảng cáo test cho vị trí: {_rewardedPlacementId}");
        Advertisement.Load(_rewardedPlacementId, this);
    }

    public void ShowRewardedAd(int gems)
    {
        Debug.Log($"[Ads] Gọi hiển thị quảng cáo. Phần thưởng chuẩn bị trao: {gems} Gems");
        gemsAdd = gems; // Ghi nhớ số lượng gem của lượt xem này
        Advertisement.Show(_rewardedPlacementId, this);
    }

    // --- Thực thi các Interface bắt buộc ---
    public void OnUnityAdsAdLoaded(string placementId) { Debug.Log($"[Ads] Đã tải xong dữ liệu quảng cáo: {placementId}"); }
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) { Debug.LogWarning($"[Ads] Load thất bại {placementId}: {message}"); }
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { Debug.LogError($"[Ads] Không thể hiển thị {placementId}: {message}"); }
    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }

    // Xử lý trao thưởng khi xem hết
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        // Sửa lỗi: Trên Editor, đôi khi placementId trả về bị rỗng hoặc khác biệt, cấu trúc so sánh an toàn cho việc TEST:
        if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("[Ads] Trình giả lập: Người chơi đã xem hết 100% video! Tiến hành xử lý Firebase...");

            if (PlayerManager.Instance != null)
            {
                // Gọi hàm cộng số gemsAdd linh hoạt bạn truyền vào
                PlayerManager.Instance.AddGems(gemsAdd, success => {
                    if (success)
                    {
                        Debug.Log($"[Ads] THÀNH CÔNG: Đã cộng {gemsAdd} Gems vào Local và đồng bộ Cloud Firebase!");
                    }
                    else
                    {
                        Debug.LogError("[Ads] THẤT BẠI: Không thể đồng bộ số Gem mới lên Firebase.");
                    }
                });
            }
            else
            {
                Debug.LogError("[Ads] Lỗi: Không tìm thấy PlayerManager Instance để cộng tiền!");
            }

            // Tải lượt quảng cáo mới cho lần bấm tiếp theo
            LoadRewardedAd();
        }
    }
}