using UnityEngine;

public class ToggleSound : ToggleSoundAndMusic
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (SoundMenuGameManager.Instance != null)
        {
            SoundMenuGameManager.Instance.IsSoundOnChange += ChangeSprite;
        }
    }

    protected override void OnClickButton()
    {
        if(SoundMenuGameManager.Instance != null)
        {
            SoundMenuGameManager.Instance.SoundChange();
        }
        base.OnClickButton();
    }
}
