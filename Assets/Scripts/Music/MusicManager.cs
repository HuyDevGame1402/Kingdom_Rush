using System;
using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource musicAudioSource;

    [Header("Music Value On")]
    private const string MUSIC_KEY = "MusicOn";
    [SerializeField] private bool isMusicOn;

    public event Action<bool> IsMusicOnChange;
    private float timeDelay = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitSoundValue();
        PlayeMusicGame(musicAudioSource.clip);
    }

    private void InitSoundValue()
    {
        isMusicOn = PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
        StartCoroutine(CoroutineDelayEvent());
    }

    private IEnumerator CoroutineDelayEvent()
    {
        yield return new WaitForSeconds(timeDelay);
        IsMusicOnChange?.Invoke(isMusicOn);
    }

    public void PlayeMusicGame(AudioClip clip)
    {
        if(musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
        }
        musicAudioSource.clip = clip;

        if(isMusicOn)
        {
            musicAudioSource.Play();
        }
    }
    public void MusicChange()
    {
        isMusicOn = !isMusicOn;
        PlayerPrefs.SetInt(MUSIC_KEY, isMusicOn ? 1 : 0);
        PlayerPrefs.Save();
        IsMusicOnChange?.Invoke(isMusicOn);

        if(isMusicOn)
        {
            musicAudioSource.Play();
        }
        else
        {
            musicAudioSource.Stop();
        }
    }
}
