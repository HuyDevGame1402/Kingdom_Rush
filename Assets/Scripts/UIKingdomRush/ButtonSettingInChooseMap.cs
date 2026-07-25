using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSettingInChooseMap : MonoBehaviour
{
    [SerializeField] Transform settingOptions;

    [SerializeField] private bool isFirstOpen;
    private float timeDelay = 0.1f;

    [SerializeField] private ChangeSpriteLightButton changeSpriteLightButton;
    private bool isOpen;
    private Button buttonSettingOptions;

    private void Awake()
    {
        buttonSettingOptions = GetComponent<Button>();
        buttonSettingOptions.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        isOpen = !isOpen;
        if( isOpen )
        {
            OpenUI();
        }
        else
        {
            CloseUI();
        }
    }

    protected void OpenUI()
    {
        PlayAudioClick();
        changeSpriteLightButton.ChangeSprite();
        settingOptions.gameObject.SetActive(isOpen);
        if (isFirstOpen == false)
        {
            isFirstOpen = true;
            StartCoroutine(CoroutineUploadButton());
        }
    }

    private IEnumerator CoroutineUploadButton()
    {
        yield return new WaitForSeconds(timeDelay);
        SoundMenuGameManager.Instance.InvokeEventSound();
        MusicManager.Instance.InvokeEventMusic();
    }

    protected void CloseUI()
    {
        PlayAudioClick();
        changeSpriteLightButton.ChangeSprite();
        settingOptions.gameObject.SetActive(isOpen);
    }

    private void PlayAudioClick()
    {
        if(SoundMenuGameManager.Instance != null)
        {
            SoundMenuGameManager.Instance.PlayAudioSourceClickButton();
        }
    }
}
