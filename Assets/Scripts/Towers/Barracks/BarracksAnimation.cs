using System.Collections;
using UnityEngine;
using System;

public class BarracksAnimation : MonoBehaviour
{
    [Header("Tower")]
    [SerializeField] private GameObject towerSprite;
    [SerializeField] private string nameTowerAnimation;

    [Header("Door")]
    [SerializeField] private GameObject doorSprite;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private float spawnCharacterTime = 1.5f;
    public event Action OnSpawnHeroEvent;
    [SerializeField] private float timeOpenDoor = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CoroutineCreateTower());
    }

    private void InitTower()
    {
        SpriteSheetAnimator.Instance.PlayAnimation(towerSprite,
            nameTowerAnimation);
    }
    
    public void IdleDoor()
    {
        doorAnimator.SetTrigger("Idle");
    }

    public void OpenDoor()
    {
        doorAnimator.SetTrigger("Open");
    }

    public void CloseDoor()
    {
        doorAnimator.SetTrigger("Close");
    }

    private IEnumerator CoroutineCreateTower()
    {
        yield return new WaitForSeconds(1.5f);
        InitTower();
        AnimationDoorInit();
    }

    private void AnimationDoorInit()
    {
        doorSprite.GetComponent<SpriteRenderer>().enabled = true;
        IdleDoor();
        OpenDoor();
        StartCoroutine(CoroutineAwaitOpenDoor());
        StartCoroutine(CoroutineTimeSpawnCharacter());   
    }
    private IEnumerator CoroutineTimeSpawnCharacter()
    {
        yield return new WaitForSeconds(spawnCharacterTime);
        CloseDoor();
    }
    private IEnumerator CoroutineAwaitOpenDoor()
    {
       yield return new WaitForSeconds(timeOpenDoor);
        OnSpawnHeroEvent?.Invoke();
    }
}
