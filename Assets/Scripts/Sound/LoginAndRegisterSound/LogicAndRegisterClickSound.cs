using UnityEngine;

public class LogicAndRegisterClickSound : MonoBehaviour
{
    [SerializeField] private AudioClip audioClick;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClick()
    {
        audioSource.PlayOneShot(audioClick);
    }
}
