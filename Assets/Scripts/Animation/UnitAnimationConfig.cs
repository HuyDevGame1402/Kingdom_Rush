using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class AnimationFrameRange
{
    public int startFrame = 1;
    public int endFrame = 1;

    [Header("Event Configuration (If needed)")]
    public bool hasEvent = false;
    public int eventFrame = -1;
    public List<EnemyAnimConfig> animationConfigOffset;
}

[System.Serializable]
public class EnemyAnimConfig
{
    public int frameOffset;
    public float offsetY = 0f;
}

[Serializable]
public class AnimationFrameRangeUpdate
{
    public string nameAnimation;
    public int startFrame = 1;
    public int endFrame = 1;
}

[Serializable]
public class UnitAnimationConfig
{
    [Tooltip("Tiền tố của hoạt ảnh trong file dữ liệu, ví dụ: soldier_lvl1_")]
    public string animPrefix;

    public float frameRate;

    [Header("States")]
    public AnimationFrameRange idle;
    public AnimationFrameRange run;
    public AnimationFrameRange runDown;
    public AnimationFrameRange runUp;
    public AnimationFrameRange attack;
    public AnimationFrameRange death;

    [Header("Attacks (Hỗ trợ 1 hoặc nhiều dáng đánh)")]
    // Dùng List giúp Hero khai báo Attack 1, Attack 2... Lính thường chỉ cần 1 phần tử
    public List<AnimationFrameRange> attacks = new List<AnimationFrameRange>();

    // Hàm tiện ích: Lấy ngẫu nhiên 1 animation đánh thường
    public AnimationFrameRange GetRandomAttack()
    {
        if (attacks == null || attacks.Count == 0) return null;
        return attacks[UnityEngine.Random.Range(0, attacks.Count)];
    }
}