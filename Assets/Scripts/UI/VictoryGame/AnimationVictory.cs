using System.Collections;
using UnityEngine;

public class AnimationVictory : MonoBehaviour
{
    private const string TRIGGERNAMESHOWVICTORY = "ShowVictory";
    private const string TRIGGERNAMESHOWBUTTONANDGEMS = "ShowButtonAndGems";

    private Animator animator;

    [SerializeField] private StarAnimationWinGame starAnimationWinGame;
    [SerializeField] private AnimationStarVictory animationStarVictory;
    private float timeDelay = 1f;

    [SerializeField] private VictorySound victorySound;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        starAnimationWinGame = GetComponent<StarAnimationWinGame>();
        animationStarVictory = GetComponent<AnimationStarVictory>();
        victorySound = GetComponent<VictorySound>();    
    }

    private void Start()
    {
        if(FinnalGameKingdomRush.Instance != null)
        {
            FinnalGameKingdomRush.Instance.OnCompleteCalculatorStarGame += Instance_OnCompleteCalculatorStarGame;
        }
    }

    private void Instance_OnCompleteCalculatorStarGame()
    {
        StartCoroutine(CoroutineVictoryVisual());
    }

    private IEnumerator CoroutineVictoryVisual()
    {
        yield return new WaitForSeconds(timeDelay);
        PlayAnimationShowVictory();
        victorySound.PlaySoundVictoryGame();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        PlayAnimationShowVictory();
    //    }
    //}

    private void PlayAnimationShowVictory()
    {
        animationStarVictory.PlayVictoryStarAnimation();
        animator.SetTrigger(TRIGGERNAMESHOWVICTORY);
    }

    public void PlayAnimationShowButtonAndGems()
    {
        animator.SetTrigger(TRIGGERNAMESHOWBUTTONANDGEMS);
        victorySound.PlaySoundMerchant();
    }

    public void OnTriggerFunctionStarAnimation()
    {
        if (FinnalGameKingdomRush.Instance == null) return;
        starAnimationWinGame.PlayAnimationStar(FinnalGameKingdomRush.Instance.GetStarWinGame());
    }
}
