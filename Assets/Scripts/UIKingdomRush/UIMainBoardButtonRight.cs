using UnityEngine;
using System;

public class UIMainBoardButtonRight : UIView
{
    [SerializeField] private GameObject shopUI;

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
        shopUI.SetActive(isOpen);
        onComplete?.Invoke();
    }

    protected override void CloseUI(Action onComplete = null)
    {
        isOpen = false;
        shopUI.SetActive(isOpen);
        onComplete?.Invoke();
    }
}
