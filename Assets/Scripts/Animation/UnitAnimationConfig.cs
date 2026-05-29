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

    [Header("Optional States (Cho Hero hoặc Quái đặc biệt)")]
    public AnimationFrameRange skill;
}