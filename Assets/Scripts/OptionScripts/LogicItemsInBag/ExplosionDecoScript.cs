using System.Collections.Generic;
using UnityEngine;

public class ExplosionDecoScript : MonoBehaviour
{
    [SerializeField] private string animationName;
    [SerializeField] private float frameRate;
    [SerializeField] private int startFrame;
    [SerializeField] private int endFrame;
    [SerializeField] private List<EnemyAnimConfig> configsAnimation = new List<EnemyAnimConfig>();

    public void ShowAnimation()
    {
        SpriteSheetAnimator.Instance.PlayAnimation(
        target: gameObject,
        animPrefix: animationName,
        startFrame: startFrame,
        endFrame: endFrame,
        eventFrame: -1,
        onEventTrigger: () => {
            // Gây sát thương ngay tại event frame (ví dụ frame 11)
        },
        offsetConfigs: configsAnimation,
        frameRate: frameRate,
            onComplete: () => {
                gameObject.SetActive(false);
            }
        );
    }
}
