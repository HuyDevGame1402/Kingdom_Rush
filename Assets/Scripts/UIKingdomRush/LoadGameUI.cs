using UnityEngine;

public class LoadGameUI : MonoBehaviour
{
    public static LoadGameUI Instance;
    public Animator animator;

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    public void DoorClose()
    {
        animator.SetTrigger("Close");
    }
    public void DoorOpen()
    {
       animator.SetTrigger("Open");
    }

    public void PlayAudioTransitionOpen()
    {
        if(SoundMenuGameManager.Instance != null)
        {
            SoundMenuGameManager.Instance.PlayAudioSourceTransitionOpen();
        }
    }
    public void PlayAudioTransitionClose()
    {
        if (SoundMenuGameManager.Instance != null)
        {
            SoundMenuGameManager.Instance.PlayAudioSourceTransitionClose();
        }
    }
}
