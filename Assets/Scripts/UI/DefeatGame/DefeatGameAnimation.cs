using UnityEngine;

public class DefeatGameAnimation : MonoBehaviour
{
    private const string TRIGGERANIMATION = "Show";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void ShowDefeatUI()
    {
        animator.SetTrigger(TRIGGERANIMATION);
    }
}
