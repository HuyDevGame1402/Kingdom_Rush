using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class GemItem : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private SpriteRenderer gemSprite;
    [SerializeField] private TextMeshPro gemText;

    // Scale chuẩn mặc định của Sprite
    private readonly Vector3 baseSpriteScale = new Vector3(2f, 2f, 1f);
    private readonly Vector3 targetSpriteScale = new Vector3(2.3f, 2.3f, 1f);

    private Sequence currentSequence;

    /// <summary>
    /// Khởi chạy hiệu ứng nảy và bay lên cho Gem
    /// </summary>
    public void SetupAndAnimate(int gemAmount, Vector3 startPos, Action<GemItem> onCompleteCallback)
    {
        // 1. Cập nhật nội dung Text & Đặt lại trạng thái ban đầu
        if (gemText != null)
        {
            gemText.text = "+" + gemAmount;
        }

        transform.position = startPos;

        // Reset scale của Sprite con về chuẩn (2, 2, 1)
        if (gemSprite != null)
        {
            gemSprite.transform.localScale = baseSpriteScale;
        }

        gameObject.SetActive(true);

        // Đặt độ trong suốt (Alpha) ban đầu về 1 (rõ)
        SetAlpha(1f);

        // Hủy animation cũ nếu có
        currentSequence?.Kill();

        // 2. Tính toán điểm nảy ngẫu nhiên xung quanh điểm spawn
        // Đã giảm độ lệch trục Y để không bị văng quá xa theo chiều dọc
        Vector3 randomOffset = new Vector3(
            UnityEngine.Random.Range(-0.3f, 0.3f),
            UnityEngine.Random.Range(-0.05f, 0.05f),
            0f
        );
        Vector3 targetJumpPos = startPos + randomOffset;

        // 3. Tạo Chuỗi Animation bằng DOTween
        currentSequence = DOTween.Sequence();

        // Phase 1: Nảy nhẹ lên vị trí đích
        // 🎯 CHỈNH TẠI ĐÂY: Giảm jumpPower từ 1.2f xuống 0.4f (hoặc 0.3f nếu muốn nảy sát đất hơn)
        currentSequence.Append(transform.DOJump(targetJumpPos, jumpPower: 0.4f, numJumps: 1, duration: 0.35f).SetEase(Ease.OutQuad));

        // Chỉ phóng to Sprite con lên 2.3 trong lúc nảy
        if (gemSprite != null)
        {
            currentSequence.Join(gemSprite.transform.DOScale(targetSpriteScale, 0.35f).SetEase(Ease.OutBack));
            // Trả Sprite con về lại scale gốc (2, 2, 1)
            currentSequence.Append(gemSprite.transform.DOScale(baseSpriteScale, 0.1f));
        }

        // Phase 2: Chờ 0.1s
        currentSequence.AppendInterval(0.1f);

        // Phase 3: Bay chậm lên trên và mờ dần (Fade Out)
        Vector3 flyUpPos = targetJumpPos + Vector3.up * 1.0f;
        currentSequence.Append(transform.DOMove(flyUpPos, 0.6f).SetEase(Ease.InQuad));
        currentSequence.Join(DOVirtual.Float(1f, 0f, 0.6f, SetAlpha));

        // Phase 4: Kết thúc -> Thu hồi về Object Pool
        currentSequence.OnComplete(() =>
        {
            gameObject.SetActive(false);
            onCompleteCallback?.Invoke(this);
        });
    }

    /// <summary>
    /// Hàm phụ trợ chỉnh Alpha đồng thời cả Sprite và TextMeshPro
    /// </summary>
    private void SetAlpha(float alpha)
    {
        if (gemSprite != null)
        {
            Color c = gemSprite.color;
            c.a = alpha;
            gemSprite.color = c;
        }

        if (gemText != null)
        {
            Color c = gemText.color;
            c.a = alpha;
            gemText.color = c;
        }
    }

    private void OnDestroy()
    {
        // Xóa Tween để tránh rò rỉ bộ nhớ khi Destroy Object
        currentSequence?.Kill();
    }
}