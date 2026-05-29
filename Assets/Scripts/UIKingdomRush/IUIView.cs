using System;

public interface IUIView
{
    bool isOpen { get; }
    public string nameEventOpen { get; }
    public string nameEventClose { get;}
    void OpenUI(Action onComplete = null);
    void CloseUI(Action onComplete = null);
}