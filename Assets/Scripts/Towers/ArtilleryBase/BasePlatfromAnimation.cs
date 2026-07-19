using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AnimationPlatfrom
{
    public string animName;
    public int startFrame;
    public int endFrame;
}

public class BasePlatfromAnimation : MonoBehaviour
{

    [SerializeField] private int level = 0;

    [SerializeField] private List<AnimationPlatfrom> animationPlatfroms = new List<AnimationPlatfrom>();

    public void PlatfromAnimation()
    {
        SpriteSheetAnimator.Instance.PlayAnimation(gameObject,
            animationPlatfroms[level].animName, startFrame: animationPlatfroms[level].startFrame
            , endFrame: animationPlatfroms[level].endFrame);
    }

    public void AddLevel()
    {
        level += 1;
        PlatfromAnimation();
    }
}
