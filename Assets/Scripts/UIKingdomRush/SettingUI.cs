using System;
using UnityEngine;

public class SettingUI : MonoBehaviour, IUIView
{
    [SerializeField] private Animator animator;

    public bool isOpen { get; set; }

    public string nameEventOpen => "SETTINGUIOPEN";
    public string nameEventClose => "SETTINGUICLOSE";

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

    public void OpenUI(Action onComplete = null)
    {
        isOpen = true;
        gameObject.SetActive(true);

        if (animator != null)
        {
            animator.SetTrigger("Open");
        }

        onComplete?.Invoke();
    }

    public void CloseUI(Action onComplete = null)
    {
        isOpen = false;

        if (animator != null)
        {
            animator.SetTrigger("Close");
        }
        onComplete?.Invoke();
    }
}