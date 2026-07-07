using UnityEngine;

public class WindmillsAnimation : MonoBehaviour
{
    [Header("Cấu Hình ID Nhóm Decor")]
    public string decorId = "stage_grass"; // Khớp với ID trong DecorSpriteAnimator

    [Header("Cấu Hình Animation")]
    public string animPrefix = "molino_big_";
    public float frameRate = 0.1f;

    void Start()
    {
        // Chờ 1 frame ngắn để đảm bảo Awake bên DecorSpriteAnimator đã khởi tạo DB xong
        StartCoroutine(StartAnimRoutine());
    }

    private System.Collections.IEnumerator StartAnimRoutine()
    {
        yield return null;

        if (DecorSpriteAnimator.Instance != null)
        {
            // Truyền chính xác GameObject này, nhóm ID, và tiền tố animation
            DecorSpriteAnimator.Instance.PlayAnimation(this.gameObject, decorId, animPrefix, frameRate);
        }
        else
        {
            Debug.LogError("[WindmillsAnimation] Không tìm thấy DecorSpriteAnimator Instance trong Scene!");
        }
    }
}