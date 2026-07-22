using Unity.VisualScripting;
using UnityEngine;

public class StarAnimationWinGame : MonoBehaviour
{
    [SerializeField] private GameObject startImage;

    private AnimationVictory animationVictory;

    private void Awake()
    {
        animationVictory = GetComponent<AnimationVictory>();    
    }

    public void PlayAnimationStar(int star)
    {
        startImage.gameObject.SetActive(true);  
        switch (star)
        {
            case 0:
                return;
            case 1:
                PlayAimationByFrame(0,18);
                return;

            case 2:
                PlayAimationByFrame(0, 37);
                return;

            case 3:
                PlayAimationByFrame(0, 53);
                return;
        }
    }

    private void PlayAimationByFrame(int startFrame, int endFrame)
    {
        DecorSpriteAnimator.Instance.PlayAnimationUI(
            startImage,
            "gui_common",
            "victoryStars",
            0.03f,
            startFrame,
            endFrame,
            () =>
            {
                if (animationVictory != null)
                {
                    animationVictory.PlayAnimationShowButtonAndGems();
                }
            }
        );
    }
}
