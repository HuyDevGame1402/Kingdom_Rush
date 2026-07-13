using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseItemOptionUI : MonoBehaviour
{
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite disableSprite;

    [SerializeField] private Transform buttonAddItem;
    [SerializeField] private TextMeshProUGUI countText;

    private Image imageButton;

    [SerializeField] private ItemType itemType;

    [SerializeField] private BagOptionClick bagOptionClick;

    private void Awake()
    {
        imageButton = GetComponent<Image>();
        bagOptionClick.OnClickBag += BagOptionClick_OnClickBag;
    }

    private void BagOptionClick_OnClickBag()
    {
        if (ItemManager.Instance == null) return;

        if(ItemManager.Instance.GetItemCount(itemType) > 0)
        {
            imageButton.sprite = activeSprite;
            buttonAddItem.gameObject.SetActive(false);
            countText.text = ItemManager.Instance.GetItemCount(itemType).ToString();
            countText.gameObject.SetActive(true);
        }
        else
        {
            imageButton.sprite = disableSprite;
            buttonAddItem.gameObject.SetActive(true);
            countText.gameObject.SetActive(false);
        }
    }
}
