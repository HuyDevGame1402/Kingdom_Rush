using System.Collections;
using System.Collections.Generic;
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
}
