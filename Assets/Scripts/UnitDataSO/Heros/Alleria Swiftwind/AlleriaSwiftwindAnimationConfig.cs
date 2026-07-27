using System;
using UnityEngine;

[Serializable]
public class AlleriaSwiftwindAnimationConfig
{
    [Header("Hero Special Animations")]
    [Tooltip("Animation ăn mừng khi lên cấp")]
    public AnimationFrameRange levelUp;

    [Tooltip("Animation bắn nhiều mũi tên liên tiếp (Chiêu Multishot)")]
    public AnimationFrameRange multishotSkill;

    [Tooltip("Animation dậm/gõ xuống đất gọi Linh Miêu (Chiêu Call of the Wild)")]
    public AnimationFrameRange callOfTheWildSkill;
}