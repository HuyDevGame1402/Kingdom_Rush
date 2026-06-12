using UnityEngine;
using UnityEngine.UI;

public class OnButtonGemsAds : MonoBehaviour
{
    [SerializeField] private int gemsAds = 100;

    private void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(OnClickWatchAd);
    }

    void OnClickWatchAd()
    {
        if (AdsManager.Instance != null)
        {
            // Gọi lệnh hiển thị quảng cáo từ AdsManager
            AdsManager.Instance.ShowRewardedAd(gemsAds);
        }
        else
        {
            Debug.LogError("Không tìm thấy AdsManager trong Scene!");
        }
    }

}
