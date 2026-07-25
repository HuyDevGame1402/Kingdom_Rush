using UnityEngine;

public class SoundMenuGameManager : MonoBehaviour
{
    public static SoundMenuGameManager Instance { get; private set; }

    [Header("Sound Click Button")]
    [SerializeField] private AudioClip soundClick;
    [SerializeField] private AudioSource audioSourceClickButton;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayAudioSourceClickButton()
    {
        audioSourceClickButton.PlayOneShot(soundClick);
    }
}
