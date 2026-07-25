using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening.Plugins.Options;

public class MainBoardMapUI : UIView
{
    public List<GameObject> gameObjects = new List<GameObject>();
    public SmokeDecoAnimation smokeDecoAnimation;
    public CloseButton closeButton;

    public AudioClip musicMainMenu;
    public AudioClip musicMap;

    [SerializeField] private Transform settingOptions;

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

    protected override void OpenUI(Action onComplete = null)
    {
        closeButton.OnCloseUI();
        LoadGameUI.Instance.DoorClose();
        isOpen = true;
        StartCoroutine(WaitTimeForLoadCredits(true));
        MusicManager.Instance.PlayeMusicGame(musicMap);
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

    protected override void CloseUI(Action onComplete = null)
    {
        LoadGameUI.Instance.DoorClose();
        isOpen = false;
        StartCoroutine(WaitTimeForLoadCredits(false));
        MusicManager.Instance.PlayeMusicGame(musicMainMenu);
        settingOptions.gameObject.SetActive(false);
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
