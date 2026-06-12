using UnityEngine;
using System;

public class GemsOptionMenuUI : UIView
{
    [SerializeField] private Animator animator;

    private void Start()
    {
        EventManager.Register(nameEventOpen, () => OpenUI(null));
        EventManager.Register(nameEventClose, () => CloseUI(null));
    }

    private void OnDestroy()
    {
        EventManager.Unregister(nameEventOpen, () => OpenUI(null));
        EventManager.Unregister(nameEventClose, () => CloseUI(null));
    }

    protected override void OpenUI(Action onComplete = null)
    {
        isOpen = true;
        gameObject.SetActive(true);

        if (animator != null)
        {
            animator.SetTrigger("Open");
        }

        onComplete?.Invoke();
    }

    protected override void CloseUI(Action onComplete = null)
    {
        isOpen = false;

        if (animator != null)
        {
            animator.SetTrigger("Close");
        }
        onComplete?.Invoke();
    }
}
