using UnityEngine;
using UnityEngine.UI;

public class BaseItemOnClick : MonoBehaviour
{
    private Button button;

    [SerializeField] private ItemType itemType;

    [SerializeField] private Transform bagTransform;
    [SerializeField] private Sprite backgroundItemSelected;
    [SerializeField] private Sprite itemSelected;

    private IHasLogicOption logicOption;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickItem);
        TryGetComponent(out logicOption);
    }

    private void OnClickItem()
    {
        if (ItemManager.Instance == null || ItemManager.Instance.GetItemCount(itemType) == 0) return;

        if(bagTransform.TryGetComponent(out BagOptionUI bagOptionUI))
        {
            bagOptionUI.UpdateSpriteNormal();
        }

        if(bagTransform.TryGetComponent(out Image imageBagButton))
        {
            imageBagButton.sprite = backgroundItemSelected;
        }
        bagTransform.GetChild(0).GetComponent<Image>().sprite = itemSelected;
        bagTransform.GetChild(0).gameObject.SetActive(true);
        if (OptionManager.Instance != null && logicOption != null)
        {
            OptionManager.Instance.SetLogicOption(logicOption);
            OptionManager.Instance.SetSupportOptions(true);
        }
    }
}
