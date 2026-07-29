using System.Collections;
using UnityEngine;

public class WildCatInit : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private GameObject spriteGameObject;
    [SerializeField] private string animationId;
    [SerializeField] private string animationName;
    [SerializeField] private AnimationFrameRange config;

    [Header("GameObject Active WildCat")]
    [SerializeField] private Transform collisionTriggerHero;
    [SerializeField] private BaseUnitStateMachine stateMachine;

    [SerializeField] private HeroEXPManager heroOwnerEXPManager;

    private void OnEnable()
    {
        if(CharacterSpriteAnimator.Instance == null)
        {
            StartCoroutine(CoroutinInit());
        }
        else
        {
            Init();
        }
    }

    private void Init()
    {
        ActiveWildCat(false);
        CharacterSpriteAnimator.Instance.PlayAnimationByRange(
        target: spriteGameObject,
            enemyId: animationId,
            animPrefix: animationName,
            rangeConfig: config,
            frameRate: 0.05f,
            onEventTrigger: () =>
            {
                CharacterSpriteAnimator.Instance.StopAnimationFor(spriteGameObject);
                ActiveWildCat(true);
            },
            onComplete: () =>
            {
                //CharacterSpriteAnimator.Instance.StopAnimationFor(spriteGameObject);
                //ActiveWildCat(true);
            }
        );
    }

    private IEnumerator CoroutinInit()
    {
        yield return new WaitForSeconds(0.1f);
        Init();
    }

    public void ActiveWildCat(bool isActive)
    {
        collisionTriggerHero.gameObject.SetActive(isActive);
        stateMachine.enabled = isActive;
    }

    public void SetHeroOwnerEXPManager(HeroEXPManager heroEXPManager)
    {
        heroOwnerEXPManager = heroEXPManager;
    }

    public HeroEXPManager GetHeroOwerEXPManager() { return heroOwnerEXPManager; }
}
