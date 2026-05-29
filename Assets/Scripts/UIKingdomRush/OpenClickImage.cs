using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenClickImage : MonoBehaviour, IPointerClickHandler
{
    public IUIView uIView;
    public GameObject settingGameObject;

    private void Start()
    {
        uIView = settingGameObject.GetComponent<IUIView>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            EventManager.Notify(uIView.nameEventOpen);
        }
    }
}
