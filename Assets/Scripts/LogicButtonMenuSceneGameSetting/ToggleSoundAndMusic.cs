using UnityEngine;
using UnityEngine.UI;

public class ToggleSoundAndMusic : MonoBehaviour
{
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite disableSprite;

    private Button button;
    private Image image;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
        image = gameObject.GetComponent<Image>();
        button.onClick.AddListener(OnClickButton);
    }
    private void OnClickButton()
    {
        if (SoundMenuGameManager.Instance != null)
        {
            SoundMenuGameManager.Instance.PlayAudioSourceClickButton();
        }
    }
}
