using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StarItem : MonoBehaviour
{
    private RectTransform rectTransform;
    private Image starImage;
    private Sequence starSequence;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        starImage = GetComponent<Image>();
    }

    public void LaunchParabola(Vector2 startPos, Vector2 apexPos, Vector2 landingPos, float duration)
    {
        gameObject.SetActive(true);
        rectTransform.anchoredPosition = startPos;

        // Reset độ trong suốt của sao
        if (starImage != null)
        {
            Color c = starImage.color;
            c.a = 1f;
            starImage.color = c;
        }

        // Hủy Sequence cũ nếu đang chạy
        starSequence?.Kill();

        // Tỷ lệ thời gian: 35% cho đoạn bay vồng lên đỉnh, 65% cho đoạn rơi xuống
        float upDuration = duration * 0.35f;
        float fallDuration = duration * 0.65f;

        starSequence = DOTween.Sequence();

        // === PHA 1: BAY VỒNG LÊN ĐỈNH ===
        // Trục X và Y cùng tiến về Apex Position (Gia tốc giảm dần - OutQuad)
        starSequence.Append(rectTransform.DOAnchorPos(apexPos, upDuration).SetEase(Ease.OutQuad));

        // === PHA 2: RƠI THEO ĐƯỜNG CONG PARABOL ===
        // Để tạo đường cong chuẩn: Trục X tiếp tục trôi đều (Linear/OutQuad nhẹ) còn Trục Y rơi nhanh dần (InQuad)
        starSequence.Append(rectTransform.DOAnchorPosY(landingPos.y, fallDuration).SetEase(Ease.InQuad));
        starSequence.Join(rectTransform.DOAnchorPosX(landingPos.x, fallDuration).SetEase(Ease.OutQuad));

        // === FADE OUT (Mờ dần khi gần rơi xong) ===
        if (starImage != null)
        {
            starSequence.Insert(upDuration + (fallDuration * 0.3f), starImage.DOFade(0f, fallDuration * 0.7f));
        }

        // Xoay nhẹ ngôi sao trên không trung
        float randomRotation = Random.Range(-270f, 270f);
        starSequence.Insert(0, rectTransform.DORotate(new Vector3(0, 0, randomRotation), duration, RotateMode.FastBeyond360));

        // Tắt GameObject khi hoàn thành
        starSequence.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OnDisable()
    {
        starSequence?.Kill();
    }
}