using System;
using UnityEngine;

public abstract class UIView : MonoBehaviour
{
    public bool isOpen;

    public string nameEventOpen;
    public string nameEventClose;
    protected abstract void OpenUI(Action onComplete = null);
    protected abstract void CloseUI(Action onComplete = null);
}