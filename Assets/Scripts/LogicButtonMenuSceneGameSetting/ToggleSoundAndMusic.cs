using UnityEngine;
using UnityEngine.UI;

public class ToggleSoundAndMusic : MonoBehaviour
{
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite disableSprite;

    private Button button;
    private Image image;

    protected virtual void Awake()
    {
        button = gameObject.GetComponent<Button>();
        image = gameObject.GetComponent<Image>();
        button.onClick.AddListener(OnClickButton);
    }
    protected virtual void OnClickButton()
    {
        if (SoundMenuGameManager.Instance != null)
        {
            SoundMenuGameManager.Instance.PlayAudioSourceClickButton();
        }
    }

    public void ChangeSprite(bool isOn)
    {
        if (isOn)
        {
            image.sprite = activeSprite;
            return;
        }
        image.sprite = disableSprite;
    }
}
