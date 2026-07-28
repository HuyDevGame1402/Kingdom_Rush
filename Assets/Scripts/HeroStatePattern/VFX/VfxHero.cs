using System.Collections;
using UnityEngine;

public class VfxHero : MonoBehaviour
{
    [Header("hero_barracks_buff")]
    [SerializeField] private GameObject spriteTargetHeroBarracksBuff;
    [SerializeField] private string idAnimation = "go_hero_gerald";
    [SerializeField] private string nameAnimation = "hero_barracks_buff_";
    [SerializeField] private float frameRateBarrackBuff = 0.05f;

    public void PlayAnimationHeroBarrackBuff(float time)
    {
        spriteTargetHeroBarracksBuff.SetActive(true);
        CharacterSpriteAnimator.Instance.PlayAnimation(
            target: spriteTargetHeroBarracksBuff,
            enemyId: idAnimation,
            animPrefix: nameAnimation,
            frameRate: frameRateBarrackBuff);
        StartCoroutine(CoroutineAnimationHeroBarrackBuff(time));
    }

    private IEnumerator CoroutineAnimationHeroBarrackBuff(float time)
    {
        yield return new WaitForSeconds(time);
        CharacterSpriteAnimator.Instance.StopAnimationFor(spriteTargetHeroBarracksBuff);
        spriteTargetHeroBarracksBuff.SetActive(false);
    }
}
