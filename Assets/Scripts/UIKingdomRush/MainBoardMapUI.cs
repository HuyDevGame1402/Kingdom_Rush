using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainBoardMapUI : MonoBehaviour, IUIView
{
    public List<GameObject> gameObjects = new List<GameObject>();
    public SmokeDecoAnimation smokeDecoAnimation;
    public bool isOpen { get; set; }

    public string nameEventOpen => "MAINBOARDMAPUIOPEN";
    public string nameEventClose => "MAINBOARDMAPUICLOSE";
    public CloseButton closeButton;

    private void Start()
    {
        EventManager.Register(nameEventOpen, () => OpenUI(null));
        EventManager.Register(nameEventClose, () => CloseUI(null));
    }

    private void OnDestroy()
    {
        EventManager.Unregister(nameEventOpen, () => OpenUI(null));
        EventManager.Unregister(nameEventClose, () => CloseUI(null));
    }

    public void OpenUI(Action onComplete = null)
    {
        closeButton.OnCloseUI();
        LoadGameUI.Instance.DoorClose();
        isOpen = true;
        StartCoroutine(WaitTimeForLoadCredits(true));
        onComplete?.Invoke();
    }

    private IEnumerator WaitTimeForLoadCredits(bool isActive)
    {
        yield return new WaitForSeconds(1.5f);
        if (isActive == false) smokeDecoAnimation.gameObject.SetActive(false);
        ActiveObjects(isActive);
        if(isActive == true) smokeDecoAnimation.gameObject.SetActive(true);
        LoadGameUI.Instance.DoorOpen();
    }

    public void CloseUI(Action onComplete = null)
    {
        LoadGameUI.Instance.DoorClose();
        isOpen = false;
        StartCoroutine(WaitTimeForLoadCredits(false));
        onComplete?.Invoke();
    }
    private void ActiveObjects(bool isActive)
    {
        for(int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].SetActive(isActive);
        }
    }
}
