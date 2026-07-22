using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class CharacterFreezing : MonoBehaviour
{
    [SerializeField] private GameObject freezecreepOb;
    private EnemyController enemyController;

    [Header("Freeze Settings")]
    [SerializeField] private string animationFreezeName;
    [SerializeField] private float frameRateFreeze;
    [SerializeField] private int startFrameFreeze;
    [SerializeField] private int endFrameFreeze;
    [SerializeField] private List<EnemyAnimConfig> animationFreezeConfigOffset;

    [Header("Thaw Settings")]
    [SerializeField] private string animationThawName;
    [SerializeField] private float frameRateThaw;
    [SerializeField] private int startFrameThaw;
    [SerializeField] private int endFrameThaw;
    [SerializeField] private List<EnemyAnimConfig> animationThawConfigOffset;

    private Coroutine freezeRoutine;

    [SerializeField] private FreezeEffect freezeEffect;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        enemyController.OnEnemyDead += InstantBreakIce;
        if (freezeEffect == null) freezeEffect = GetComponent<FreezeEffect>();
    }


    public void StartFreezeStatus(float duration = 5f)
    {
        if (enemyController == null || enemyController.isDead) return;
        if (freezeRoutine != null)
        {
            StopCoroutine(freezeRoutine);
        }
        freezeRoutine = StartCoroutine(FreezeSequenceRoutine(duration));
        freezeEffect.ApplyFreeze(duration);
    }

    private IEnumerator FreezeSequenceRoutine(float duration)
    {
        enemyController.FreezeEnemy();

        string enemyPrefix = enemyController.unitData.animations.animPrefix;
        int currentEnemyFrame = SpriteSheetAnimator.Instance.GetCurrentFrameNumber(gameObject);
      
        SpriteSheetAnimator.Instance.DisplaySingleFrame(gameObject, enemyPrefix, currentEnemyFrame);
        freezecreepOb.SetActive(true);
        PlayAnimationFreeze();
 
        yield return new WaitForSeconds(duration);

        bool thawCompleted = false;
        PlayAnimation(freezecreepOb, animationThawName, startFrameThaw, endFrameThaw, frameRateThaw, animationThawConfigOffset, () =>
        {
            thawCompleted = true; 
        });

        yield return new WaitUntil(() => thawCompleted);

        freezecreepOb.SetActive(false);
        enemyController.ThawEnemy();

        freezeRoutine = null;
    }


    private void PlayAnimation(GameObject targetGameObject, string animationName, int startFrame,
        int endFrame, float frameRate, List<EnemyAnimConfig> configOffset, Action onComplete = null)
    {
        targetGameObject.SetActive(true);
        SpriteSheetAnimator.Instance.PlayAnimation(
            target: targetGameObject,
            animPrefix: animationName,
            startFrame: startFrame,
            endFrame: endFrame,
            eventFrame: -1,
            onEventTrigger: () => { },
            offsetConfigs: configOffset,
            frameRate: frameRate,
            onComplete: onComplete 
        );
    }

    public void PlayAnimationFreeze()
    {
        PlayAnimation(freezecreepOb, animationFreezeName, startFrameFreeze, endFrameFreeze, frameRateFreeze, animationFreezeConfigOffset, () => {
            
        });
    }

    public void PlayAnimationThaw()
    {
        PlayAnimation(freezecreepOb, animationThawName, startFrameThaw, endFrameThaw, frameRateThaw, animationThawConfigOffset, null);
    }
    private void InstantBreakIce()
    {
        if (enemyController.isFrozen == false) return;
        if (freezeRoutine != null)
        {
            StopCoroutine(freezeRoutine);
            freezeRoutine = null;
        }
        PlayAnimation(freezecreepOb, animationThawName, startFrameThaw, endFrameThaw, frameRateThaw, animationThawConfigOffset, () =>
        {
            freezecreepOb.SetActive(false);
        });

        freezeEffect.RemoveFreeze();

        if (enemyController != null)
        {
            enemyController.ThawEnemy();
        }
    }
}