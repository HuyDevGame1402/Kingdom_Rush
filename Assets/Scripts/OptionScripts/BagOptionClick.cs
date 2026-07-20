using UnityEngine;
using System;

public class BagOptionClick : OptionClick
{
    public event Action OnClickBag;

    protected override void OnClickButton()
    {
        base.OnClickButton();

        if (OptionManager.Instance != null)
        {
            OptionManager.Instance.ResetLogicOption();
            OptionManager.Instance.SetSupportOptions(false);
        }
        OnClickBag?.Invoke();
    }
}
