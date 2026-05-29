using System;
using System.Collections;
using UnityEngine;

public class CreditsUI : MonoBehaviour, IUIView
{
    public GameObject creditsUI;
    public bool isOpen { get; set; }

    public string nameEventOpen => "CREDITSUIOPEN";
    public string nameEventClose => "CREDITSUICLOSE";

    private float timeDelay = 1.5f;

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
        LoadGameUI.Instance.DoorClose();
        isOpen = true;
        StartCoroutine(WaitTimeForLoadCredits(true));
        onComplete?.Invoke();
    }

    private IEnumerator WaitTimeForLoadCredits(bool isActive)
    {
        yield return new WaitForSeconds(timeDelay);
        creditsUI.SetActive(isActive);
        LoadGameUI.Instance.DoorOpen();
    }

    public void CloseUI(Action onComplete = null)
    {
        LoadGameUI.Instance.DoorClose();
        isOpen = false;
        StartCoroutine(WaitTimeForLoadCredits(false));
        onComplete?.Invoke();
    }
}
