using UnityEngine;
using System;
using System.Collections;

public class SoundMenuGameManager : MonoBehaviour
{
    public static SoundMenuGameManager Instance { get; private set; }

    [Header("Sound Click Button")]
    [SerializeField] private AudioClip soundClick;
    [SerializeField] private AudioSource audioSourceClickButton;

    [Header("Sound and Music Value On")]
    private const string SOUND_KEY = "SoundOn";
    [SerializeField] private bool isSoundOn;

    public event Action<bool> IsSoundOnChange;
    private float timeDelay = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitSoundValue();
    }

    private void InitSoundValue()
    {
        isSoundOn = PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
        StartCoroutine(CoroutineDelayEvent());
    }

    private IEnumerator CoroutineDelayEvent()
    {
        yield return new WaitForSeconds(timeDelay);
        IsSoundOnChange?.Invoke(isSoundOn);
    }

    public void SoundChange()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt(SOUND_KEY, isSoundOn ? 1 : 0);
        PlayerPrefs.Save();
        IsSoundOnChange?.Invoke(isSoundOn);
    }

    public void PlayAudioSourceClickButton()
    {
        if (isSoundOn == false) return;
        audioSourceClickButton.PlayOneShot(soundClick);
    }
}
