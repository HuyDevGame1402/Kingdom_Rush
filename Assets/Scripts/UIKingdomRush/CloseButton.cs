using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    public Button buttonClose;
    public GameObject uiObject;
    public IUIView uiView;
    private void Start()
    {
        buttonClose = GetComponent<Button>();
        uiView = uiObject.GetComponent<IUIView>();
        buttonClose.onClick.AddListener(OnCloseUI);
    }

    public void OnCloseUI()
    {
        EventManager.Notify(uiView.nameEventClose);
    }
}
