using UnityEngine;
using UnityEngine.UI;

public class ToggleLinkButton : MonoBehaviour
{
    private Button button;

    [SerializeField] private string linkConnect;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
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
