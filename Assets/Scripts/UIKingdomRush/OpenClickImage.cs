using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenClickImage : MonoBehaviour, IPointerClickHandler
{
    public UIView uIView;
    public GameObject settingGameObject;

    private void Start()
    {
        uIView = settingGameObject.GetComponent<UIView>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            EventManager.Notify(uIView.nameEventOpen);

            if (SoundMenuGameManager.Instance != null)
            {
                SoundMenuGameManager.Instance.PlayAudioSourceClickButton();
            }
        }
    }
}
