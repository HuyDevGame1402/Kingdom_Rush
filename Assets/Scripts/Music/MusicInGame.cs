using System.Collections;
using UnityEngine;

public class MusicInGame : MonoBehaviour
{
    public static MusicInGame Instance { get; private set; }
    private AudioSource audioSource;
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private float timeDelay;

    [Range(0f,1f)]
    [SerializeField] private float volume;    
    

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(CoroutineMusicGame());
    }

    private void PlayMusicGame()
    {
        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private IEnumerator CoroutineMusicGame()
    {
        yield return new WaitForSeconds(timeDelay);
        PlayMusicGame();
    }

    public void SetVolume(bool isMusic)
    {
        if (isMusic)
        {
            audioSource.volume = volume;
        }
        else
        {
            audioSource.volume = 0f;
        }
    }
}
