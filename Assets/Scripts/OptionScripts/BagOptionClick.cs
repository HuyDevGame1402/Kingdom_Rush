using UnityEngine;

public class BagOptionClick : OptionClick
{

    [SerializeField] private Transform background;
    [SerializeField] private Transform bags;

    protected override void OnClickButton()
    {
        base.OnClickButton();
        //ActiveItemsInBag(true);
    }

    private void ActiveItemsInBag(bool isActive)
    {
        background.gameObject.SetActive(isActive);
        bags.gameObject.SetActive(isActive);
    }
}
