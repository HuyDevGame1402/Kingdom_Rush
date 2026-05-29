using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenClickButton : MonoBehaviour
{
    public Button buttonOpen;
    public GameObject uiObject;
    public IUIView uiView;
    private void Start()
    {
        buttonOpen = GetComponent<Button>();
        uiView = uiObject.GetComponent<IUIView>();
        buttonOpen.onClick.AddListener(OnOpenUI);
    }

    private void OnOpenUI()
    {
        EventManager.Notify(uiView.nameEventOpen);
    }
}
