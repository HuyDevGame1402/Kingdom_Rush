using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlatfromAnimation : MonoBehaviour
{
    public void PlatfromAnimation()
    {
        SpriteSheetAnimator.Instance.PlayAnimation(gameObject, 
            "tower_artillery_lvl1_layer1_", startFrame: 1, endFrame: 1);
    }
}
