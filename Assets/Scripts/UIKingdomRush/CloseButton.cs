using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    public Button buttonClose;
    public GameObject uiObject;
    public UIView uiView;
    private void Start()
    {
        buttonClose = GetComponent<Button>();
        uiView = uiObject.GetComponent<UIView>();
        buttonClose.onClick.AddListener(OnCloseUI);
    }

    public void OnCloseUI()
    {
        EventManager.Notify(uiView.nameEventClose);
    }
}
