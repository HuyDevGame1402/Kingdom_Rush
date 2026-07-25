using UnityEngine;

public class ToggleMusic : ToggleSoundAndMusic
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.IsMusicOnChange += ChangeSprite;
        }
    }

    protected override void OnClickButton()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.MusicChange();
        }
        base.OnClickButton();
    }
}
