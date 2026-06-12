using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource musicAudioSource;

    private void Awake()
    {
        Instance = this;
        PlayeMusicGame(musicAudioSource.clip);
    }

    public void PlayeMusicGame(AudioClip clip)
    {
        if(musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
        }
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }

}
