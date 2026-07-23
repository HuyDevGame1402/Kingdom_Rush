using UnityEngine;

public class AnimationVictory : MonoBehaviour
{
    private const string TRIGGERNAMESHOWVICTORY = "ShowVictory";
    private const string TRIGGERNAMESHOWBUTTONANDGEMS = "ShowButtonAndGems";

    private Animator animator;

    [SerializeField] private StarAnimationWinGame starAnimationWinGame;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        starAnimationWinGame = GetComponent<StarAnimationWinGame>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayAnimationShowVictory();
        }
    }

    public void PlayAnimationShowVictory()
    {
        animator.SetTrigger(TRIGGERNAMESHOWVICTORY);
    }

    public void PlayAnimationShowButtonAndGems()
    {
        animator.SetTrigger(TRIGGERNAMESHOWBUTTONANDGEMS);
    }

    public void OnTriggerFunctionStarAnimation()
    {
        if (FinnalGameKingdomRush.Instance == null) return;
        starAnimationWinGame.PlayAnimationStar(FinnalGameKingdomRush.Instance.GetStarWinGame());
    }
}
