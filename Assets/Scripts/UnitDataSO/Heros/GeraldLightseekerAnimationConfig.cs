using System;
using UnityEngine;

[Serializable]
public class GeraldLightseekerAnimationConfig
{
    [Header("Gerald / Hero Special Animations")]
    [Tooltip("Animation giơ kiếm mừng khi lên cấp")]
    public AnimationFrameRange levelUp;

    [Tooltip("Animation gõ vào khiên (Chiêu Courage - Buff đồng đội)")]
    public AnimationFrameRange courageSkill;

    [Tooltip("Animation giơ khiên đỡ/chặn đòn (Chiêu Shield of Retribution)")]
    public AnimationFrameRange shieldBlock;

    [Header("VFX đi kèm (Như vòng sáng dưới chân)")]
    public string vfxPrefix; // Ví dụ: "hero_barracks_buff_"
    public AnimationFrameRange vfxBuffAura;
}
