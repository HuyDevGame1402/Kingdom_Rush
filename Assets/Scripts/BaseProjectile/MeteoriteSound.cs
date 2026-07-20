using UnityEngine;

public class MeteoriteSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioLoop;
    [SerializeField] private AudioClip audioHit;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioLoop()
    {
        audioSource.loop = true;
        audioSource.clip = audioLoop;
        audioSource.Play();
    }

    public void PlayAudioHit()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(audioHit);
    }
} 
