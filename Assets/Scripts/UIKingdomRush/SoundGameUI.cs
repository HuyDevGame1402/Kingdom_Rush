using UnityEngine;
using UnityEngine.UI;

public class SoundGameUI : BaseToggleButton
{

    protected override void Start()
    {
        base.Start();
        GameEvents.Sound.OnSoundToggled += UpdateUI;
    }

    protected override void OnToggle(bool isOn)
    {
        // Bắn sự kiện của riêng Sound
        GameEvents.Sound.OnSoundToggled?.Invoke(isOn);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        // HỦY ĐĂNG KÝ Ở ĐÂY - An toàn chống tràn bộ nhớ!
        GameEvents.Sound.OnSoundToggled -= UpdateUI;
    }

    protected override bool GetInitialState()
    {
        // Đọc từ PlayerPrefs (0 là bật, 1 là tắt, tùy bạn quy định)
        return PlayerPrefs.GetInt("SoundMuted", 0) == 0;
    }
}
