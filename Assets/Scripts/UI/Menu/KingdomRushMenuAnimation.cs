using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class KingdomRushMenuAnimation : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RectTransform startPanel;
    [SerializeField] private RectTransform creditsPanel;

    [Header("Y Positions - Hide")]
    [Tooltip("Vị trí ẩn phía trên màn hình (cho cả 2 cái biến mất)")]
    [SerializeField] private float hiddenYPosition = 1200f;

    [Header("Y Positions - Show")]
    [Tooltip("Vị trí dừng của tấm Start (nằm trên)")]
    [SerializeField] private float startShownY = 0f;

    [Tooltip("Vị trí dừng cuối cùng của tấm Credits (nằm dưới)")]
    [SerializeField] private float creditsShownY = -200f;

    [Header("Animation Settings")]
    [SerializeField] private float duration = 0.4f; // Giảm nhẹ time xuống để rơi dứt khoát hơn

    [Header("Custom Bounce Settings (Cho Credits)")]
    [Tooltip("Biên độ nhún xuống qua khỏi vị trí đích ở nhịp đầu tiên (Càng nhỏ nảy càng nhẹ)")]
    [SerializeField] private float bounceOvershoot = 40f;

    [Tooltip("Biên độ nảy ngược lên trên vị trí đích ở nhịp thứ hai")]
    [SerializeField] private float bounceElasticity = 15f;

    [Tooltip("Thời gian của mỗi nhịp nảy phụ")]
    [SerializeField] private float bounceDuration = 0.12f;

    private bool isMenuShown = false;

    void Start()
    {
        InitPanelState(startPanel);
        InitPanelState(creditsPanel);
    }

    void Update()
    {
        // --- CODE TEST BẰNG BÀN PHÍM ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isMenuShown)
            {
                // ---------------- 1. SHOW MENU ----------------

                // Tấm Start: Rơi xuống dứt khoát và dừng khựng lại ngay (KHÔNG nảy)
                startPanel.DOKill();
                startPanel.DOAnchorPosY(startShownY, duration)
                          .SetEase(Ease.OutCubic);

                // Tấm Credits: Tạo chuỗi nảy tưng tửng chuẩn xích (Rơi lố -> Nảy lên -> Về đích)
                creditsPanel.DOKill();

                float lowestY = creditsShownY - bounceOvershoot;      // Nhịp 1: Bị thụt xuống dưới vạch đích
                float highestY = creditsShownY + bounceElasticity;    // Nhịp 2: Nảy tưng ngược lên trên vạch đích

                Sequence creditsSequence = DOTween.Sequence();

                creditsSequence.Append(
                    // 1. Rơi nhanh từ trên cao xuống qua khỏi vị trí đích
                    creditsPanel.DOAnchorPosY(lowestY, duration).SetEase(Ease.OutQuad)
                )
                .Append(
                    // 2. Tưng ngược lên trên vị trí đích một chút do quán tính xích
                    creditsPanel.DOAnchorPosY(highestY, bounceDuration).SetEase(Ease.OutQuad)
                )
                .Append(
                    // 3. Rơi nhẹ về lại đúng vị trí dừng cuối cùng
                    creditsPanel.DOAnchorPosY(creditsShownY, bounceDuration).SetEase(Ease.InQuad)
                );
            }
            else
            {
                // ---------------- 2. HIDE MENU ----------------
                // Khi thu hồi, cả 2 tấm cùng kéo dứt khoát lên trên

                startPanel.DOKill();
                startPanel.DOAnchorPosY(hiddenYPosition, duration)
                          .SetEase(Ease.InQuad);

                creditsPanel.DOKill();
                creditsPanel.DOAnchorPosY(hiddenYPosition, duration)
                            .SetEase(Ease.InQuad);
            }

            isMenuShown = !isMenuShown;
        }
    }

    private void InitPanelState(RectTransform panel)
    {
        if (panel != null)
        {
            panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, hiddenYPosition);
        }
    }
}