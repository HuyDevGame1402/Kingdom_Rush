using System;
using UnityEngine;

public class ButtonAchievementsChooseMap : UIView
{
    [SerializeField] private ChangeSpriteLightButton changeSpriteLightButton;

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
        PlayAudioClick();
        changeSpriteLightButton.ChangeSprite();
        isOpen = true;

        onComplete?.Invoke();
    }

    protected override void CloseUI(Action onComplete = null)
    {
        PlayAudioClick();
        isOpen = false;
        changeSpriteLightButton.ChangeSprite();

        onComplete?.Invoke();
    }

    private void PlayAudioClick()
    {
        if (SoundMenuGameManager.Instance != null)
        {
            SoundMenuGameManager.Instance.PlayAudioSourceClickButton();
        }
    }
}
