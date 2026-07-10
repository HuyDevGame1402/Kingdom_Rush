using UnityEngine;

public class LogicOpenOption : MonoBehaviour, IHasLogicAfterEventWave
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Execute()
    {
        animator.SetTrigger("Open");
    }
}
