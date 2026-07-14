using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class CharacterFreezing : MonoBehaviour
{
    [SerializeField] private GameObject freezecreepOb;
    private EnemyController enemyController;

    [Header("Freeze Settings")]
    [SerializeField] private string animationFreezeName;
    [SerializeField] private float frameRateFreeze;
    [SerializeField] private int startFrameFreeze;
    [SerializeField] private int endFrameFreeze;
    [SerializeField] private List<EnemyAnimConfig> animationFreezeConfigOffset;

    [Header("Thaw Settings")]
    [SerializeField] private string animationThawName;
    [SerializeField] private float frameRateThaw;
    [SerializeField] private int startFrameThaw;
    [SerializeField] private int endFrameThaw;
    [SerializeField] private List<EnemyAnimConfig> animationThawConfigOffset;

    private Coroutine freezeRoutine;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    // Hàm chính thức để bên ngoài (Ví dụ: Trụ băng, Phép thuật) gọi vào quái này
    public void StartFreezeStatus(float duration = 5f)
    {
        if (enemyController == null || enemyController.isDead) return;

        // Nếu đang bị đóng băng sẵn rồi thì reset lại Coroutine để tính lại từ đầu (đóng băng đè thời gian)
        if (freezeRoutine != null)
        {
            StopCoroutine(freezeRoutine);
        }

        freezeRoutine = StartCoroutine(FreezeSequenceRoutine(duration));
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        StartFreezeStatus();
    //    }
    //}

    private IEnumerator FreezeSequenceRoutine(float duration)
    {
        // 1. Dừng di chuyển và AI của Enemy (Quan trọng: Trong EnemyController.FreezeEnemy() 
        // bạn cần bật 1 biến flag như isFrozen = true để chặn không cho các lệnh PlayAnimation khác chạy đè lên)
        enemyController.FreezeEnemy();

        // 2. Lấy CHÍNH XÁC frame hiện tại của Enemy trước khi đứng im
        string enemyPrefix = enemyController.unitData.animations.animPrefix;
        int currentEnemyFrame = SpriteSheetAnimator.Instance.GetCurrentFrameNumber(gameObject);

        // Dừng hoạt ảnh của Enemy tại đúng frame đó và giữ nguyên hình ảnh
        SpriteSheetAnimator.Instance.DisplaySingleFrame(gameObject, enemyPrefix, currentEnemyFrame);

        // 3. Bật lớp băng bên ngoài lên và cho chạy hiệu ứng đóng băng (CHỈ CHẠY 1 LẦN, KHÔNG LOOP)
        freezecreepOb.SetActive(true);
        PlayAnimationFreeze(); // Hàm này đã được sửa bên dưới để không loop nữa

        // 4. Chờ 5 giây (hoặc duration truyền vào)
        yield return new WaitForSeconds(duration);

        // 5. Chạy hoạt ảnh vỡ băng (Thaw)
        bool thawCompleted = false;
        PlayAnimation(freezecreepOb, animationThawName, startFrameThaw, endFrameThaw, frameRateThaw, animationThawConfigOffset, () =>
        {
            thawCompleted = true; // Đánh dấu khi chạy xong hoạt ảnh vỡ băng
        });

        // Đợi cho đến khi hoạt ảnh vỡ băng thực sự chạy xong xuôi
        yield return new WaitUntil(() => thawCompleted);

        // 6. Dọn dẹp: Tắt hiệu ứng băng, trả lại quyền hoạt động cho Enemy
        freezecreepOb.SetActive(false);
        enemyController.ThawEnemy(); // Trong này nhớ set isFrozen = false để Enemy có thể tiếp tục chơi anim di chuyển bình thường

        freezeRoutine = null;
    }

    // Wrapper chơi animation có bổ sung thêm callback onComplete
    private void PlayAnimation(GameObject targetGameObject, string animationName, int startFrame,
        int endFrame, float frameRate, List<EnemyAnimConfig> configOffset, Action onComplete = null)
    {
        targetGameObject.SetActive(true);
        SpriteSheetAnimator.Instance.PlayAnimation(
            target: targetGameObject,
            animPrefix: animationName,
            startFrame: startFrame,
            endFrame: endFrame,
            eventFrame: -1,
            onEventTrigger: () => { },
            offsetConfigs: configOffset,
            frameRate: frameRate,
            onComplete: onComplete // Truyền callback xuống hệ thống để biết khi nào hoạt ảnh chạy xong
        );
    }

    public void PlayAnimationFreeze()
    {
        // GIẢI QUYẾT VẤN ĐỀ 2: 
        // Thay vì truyền null (làm anim bị loop vô tận), ta truyền một callback trống () => {}
        // SpriteSheetAnimator thấy onComplete != null sẽ CHỈ CHẠY ĐÚNG 1 LẦN rồi dừng lại ở frame cuối cùng.
        PlayAnimation(freezecreepOb, animationFreezeName, startFrameFreeze, endFrameFreeze, frameRateFreeze, animationFreezeConfigOffset, () => {
            // Hành động sau khi đóng băng hoàn thành (nếu có, ví dụ: giữ nguyên tảng băng)
        });
    }

    public void PlayAnimationThaw()
    {
        PlayAnimation(freezecreepOb, animationThawName, startFrameThaw, endFrameThaw, frameRateThaw, animationThawConfigOffset, null);
    }
}