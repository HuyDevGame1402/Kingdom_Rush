using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyAnimatorTest : MonoBehaviour
{
    [System.Serializable]
    public class FramePivotDebug
    {
        [Tooltip("Số frame muốn chỉnh (Ví dụ: 68, 69)")]
        public int frameIndex;
        [Tooltip("Thay đổi Pivot Y của ảnh (Kéo lên hoặc xuống để chân chạm đất)")]
        [Range(-5f, 1f)]
        public float pivotYOffset = 0f;
    }

    [Header("Test Target")]
    public GameObject targetEnemy;

    [Header("Enemy Configuration")]
    public string enemyId = "go_enemies_grass";
    public string animPrefix = "goblin_";

    [Header("Manual Frame Control")]
    public int currentFrameIndex = 1;

    [Header("Manual Pivot Configuration")]
    [Tooltip("Bấm dấu (+) để thêm cấu hình Pivot Offset cho từng frame cụ thể.")]
    public List<FramePivotDebug> manualPivotOffsets = new List<FramePivotDebug>();

    public UnitDataSO test;

    void Start()
    {
        if (targetEnemy == null) targetEnemy = gameObject;

        Debug.Log("=========================================================");
        Debug.Log("🔍 CHẾ ĐỘ SOI TỪNG FRAME & CHỈNH PIVOT TRỰC TIẾP");
        Debug.Log("👉 MŨI TÊN PHẢI: Tăng 1 frame | 👈 MŨI TÊN TRÁI: Giảm 1 frame");
        Debug.Log("⌨️ Bấm SPACE (Dấu cách) hoặc kéo thanh trượt khi đang Play để cập nhật");
        Debug.Log("=========================================================");

        ShowSingleFrame(currentFrameIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentFrameIndex++;
            ShowSingleFrame(currentFrameIndex);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentFrameIndex > 1)
            {
                currentFrameIndex--;
                ShowSingleFrame(currentFrameIndex);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowSingleFrame(currentFrameIndex);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Test Animation: Idle");
            TestAnimationEnemy(test.animations.idle);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Test Animation: Run");
            TestAnimationEnemy(test.animations.run);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Test Animation: Run Down");
            TestAnimationEnemy(test.animations.runDown);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Test Animation: Run Up");
            TestAnimationEnemy(test.animations.runUp);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("Test Animation: Attack");
            TestAnimationEnemy(test.animations.attack);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("Test Animation: Death");
            TestAnimationEnemy(test.animations.death);
        }
    }

    // Tự động cập nhật ngay lập tức khi bạn kéo thanh trượt trên Inspector lúc đang chạy game
    private void OnValidate()
    {
        if (Application.isPlaying && targetEnemy != null)
        {
            ShowSingleFrame(currentFrameIndex);
        }
    }

    private void ShowSingleFrame(int frameNumber)
    {
        if (CharacterSpriteAnimator.Instance == null || targetEnemy == null) return;

        string frameKey = $"{animPrefix.Trim().ToLower()}{frameNumber:D4}";

        // Lấy lượng pivot offset đã đăng ký cho frame này
        float currentOffset = GetRegisteredPivotOffset(frameNumber);

        // Truyền thẳng sang hàm DisplaySingleFrame mới của Animator
        bool success = CharacterSpriteAnimator.Instance.DisplaySingleFrame(targetEnemy, enemyId, frameKey, currentOffset);

        if (success)
        {
            // Vị trí transform gốc của bạn Vector3(1.1, 3.38, 0) giờ đây được bảo toàn hoàn hảo!
            Debug.Log($"📸 [Soi Frame] {frameKey} | Pivot Y Offset: {currentOffset}");
        }
    }

    private float GetRegisteredPivotOffset(int frameNumber)
    {
        foreach (var config in manualPivotOffsets)
        {
            if (config.frameIndex == frameNumber)
            {
                return config.pivotYOffset;
            }
        }
        return 0f;
    }

    public void TestAnimationEnemy(AnimationFrameRange animationRange)
    {
        if (CharacterSpriteAnimator.Instance == null || test == null || test.animations == null)
        {
            Debug.LogError("Thiếu Instance Animator hoặc dữ liệu cấu hình 'test'!");
            return;
        }

        CharacterSpriteAnimator.Instance.PlayAnimationByRange(
            gameObject,
            test.unitName,
            test.animations.animPrefix,
            animationRange,
            test.animations.frameRate,
            onComplete: () =>
            {
                // Xử lý sau khi animation chạy xong (nếu cần)
                Debug.Log($"Đã chạy xong animation!");
            }
        );
    }
}