using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClickAnimation : MonoBehaviour
{
    // Khởi tạo Singleton
    public static ClickAnimation Instance { get; private set; }

    [Header("Click Animation Settings")]
    [Tooltip("Tỉ lệ phóng to khi click (1.1 = phóng to thêm 10%)")]
    [SerializeField] private float clickScaleMultiplier = 1.15f;

    [Tooltip("Thời gian của hiệu ứng click (phóng to rồi thu nhỏ về cũ)")]
    [SerializeField] private float clickDuration = 0.15f;

    private void Awake()
    {
        // Kiểm tra và thiết lập Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Giữ Manager này không bị xóa khi chuyển Scene (nếu cần)
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Hiệu ứng click phóng to thu nhỏ truyền vào Image
    /// </summary>
    public void PlayClickAnimation(Image targetImage)
    {
        if (targetImage == null) return;
        PlayScaleAnimation(targetImage.rectTransform);
    }

    /// <summary>
    /// Hiệu ứng click phóng to thu nhỏ truyền vào GameObject chứa Image (hoặc chứa RectTransform)
    /// </summary>
    public void PlayClickAnimation(GameObject targetObj)
    {
        if (targetObj == null) return;

        // Lấy RectTransform của UI Object
        RectTransform rectTransform = targetObj.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            PlayScaleAnimation(rectTransform);
        }
    }

    // Hàm xử lý core animation phóng to -> thu nhỏ của DOTween
    private void PlayScaleAnimation(RectTransform rectTransform)
    {
        // 1. Xóa các Tween scale cũ trên object này để tránh lỗi nếu người chơi spam click liên tục
        rectTransform.DOKill(true); // Tham số true để nó hoàn thành ngay lập tức tween cũ trước khi chạy cái mới

        // Lưu lại scale gốc ban đầu của nút (đề phòng nút đó vốn có scale khác 1)
        Vector3 originalScale = Vector3.one;

        // 2. Sử dụng Sequence để tạo chuỗi phóng to rồi thu nhỏ liền mạch
        Sequence clickSequence = DOTween.Sequence();

        clickSequence.Append(
            // Phóng to lên bằng hệ số clickScaleMultiplier trong 1/2 thời gian
            rectTransform.DOScale(originalScale * clickScaleMultiplier, clickDuration * 0.5f).SetEase(Ease.OutQuad)
        )
        .Append(
            // Thu nhỏ mượt mà về lại kích thước ban đầu trong 1/2 thời gian còn lại
            rectTransform.DOScale(originalScale, clickDuration * 0.5f).SetEase(Ease.InQuad)
        );
    }
}
