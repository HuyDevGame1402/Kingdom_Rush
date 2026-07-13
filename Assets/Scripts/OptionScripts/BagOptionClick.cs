using UnityEngine;
using System;

public class BagOptionClick : OptionClick
{
    public event Action OnClickBag;

    protected override void OnClickButton()
    {
        base.OnClickButton();
        //ActiveItemsInBag(true);
        OnClickBag?.Invoke();
    }
}
