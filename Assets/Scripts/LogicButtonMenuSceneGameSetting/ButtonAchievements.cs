using UnityEngine;
using UnityEngine.UI;

public class ButtonAchievements : MonoBehaviour
{
    private Button button;

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
