using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Construction : MonoBehaviour
{
    private Animator animator;
    private bool hitAttack = false;
    [SerializeField] private float timeAnimation = 0.4f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            hitAttack = true;
            animator.SetBool("attack", hitAttack);
            StartCoroutine(WaitTimeAnimation());
        }
    }

    private IEnumerator WaitTimeAnimation()
    {
        yield return new WaitForSeconds(timeAnimation);
        hitAttack = false;
        animator.SetBool("attack", hitAttack);
    }

}
