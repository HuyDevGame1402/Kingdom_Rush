using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CloseButtonSetting : MonoBehaviour
{
    private Button buttonCloseSetting;
    private float timeDelayCloseSetting = 0.1f;
    [SerializeField] private Sprite spriteNormal;
    [SerializeField] private Sprite spriteClicked;
    [SerializeField] private SettingInGame settingInGame;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        buttonCloseSetting = GetComponent<Button>();
        buttonCloseSetting.onClick.AddListener(CloseSetting);
    }

    private void CloseSetting()
    {
        StartCoroutine(CoroutineDelayClose());
        if (SoundInGameManager.Instance != null)
        {
            SoundInGameManager.Instance.PlayTickWood();
        }
    }
    private IEnumerator CoroutineDelayClose()
    {
        image.sprite = spriteClicked;
        yield return new WaitForSeconds(timeDelayCloseSetting);
        image.sprite = spriteNormal;
        settingInGame.CloseSetting();
    }
}
