using UnityEngine;

public class MusicGameUI : BaseToggleButton
{
    protected override void Start()
    {
        base.Start();
        GameEvents.Music.OnMusicToggled += UpdateUI;
    }

    protected override void OnToggle(bool isOn)
    {
        GameEvents.Music.OnMusicToggled?.Invoke(isOn);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameEvents.Music.OnMusicToggled -= UpdateUI;
    }

    protected override bool GetInitialState()
    {
        return PlayerPrefs.GetInt("MusicMuted", 0) == 0;
    }
}
