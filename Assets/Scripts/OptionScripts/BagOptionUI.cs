using UnityEngine;

public class BagOptionUI : OptionUI
{
    [SerializeField] private Transform background;
    [SerializeField] private Transform bags;

    [SerializeField] private Transform imageItemSelected;

    public override void UpdateSpriteNormal()
    {
        base.UpdateSpriteNormal();
        background.gameObject.SetActive(false);
        bags.gameObject.SetActive(false);
        imageItemSelected.gameObject.SetActive(false);
    }

    public override void UpdateSpriteSelected()
    {
        base.UpdateSpriteSelected();
        background.gameObject.SetActive(true);
        bags.gameObject.SetActive(true);
        imageItemSelected.gameObject.SetActive(false);
    }
}
