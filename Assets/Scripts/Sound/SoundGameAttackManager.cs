using UnityEngine;
using System.Collections.Generic;
using static GameEvents;

public class SoundGameAttackManager : MonoBehaviour
{
    public static SoundGameAttackManager Instance { get; private set; }
    [Header("Sound Arrow Fly And Hit")]
    [SerializeField] private List<AudioSource> audioSourcesArrowFly = new List<AudioSource>();
    [SerializeField] private SoundSO soundSOArrowFly;
    [SerializeField] private List<AudioSource> audioSourcesArrowHit = new List<AudioSource>();
    [SerializeField] private SoundSO soundSOArrowHit;

    [Header("Sound Mage Shoot")]
    [SerializeField] private List<AudioSource> audioSourcesMageShoot = new List<AudioSource>();
    [SerializeField] private SoundSO soundSOMageShoot;

    [Header("Sound Bomb")]
    [SerializeField] private List<AudioSource> audioSourcesBomb = new List<AudioSource>();
    [SerializeField] private SoundSO soundSOBomb;

    [Header("Sound Barrack")]
    [SerializeField] private List<AudioSource> audioSourcesBarrackMove = new List<AudioSource>();
    [SerializeField] private SoundSO soundSOBarrackMove;

    private void Awake()
    {
        Instance = this;
    }

    private void PlayAudioGame(SoundSO soundSO, List<AudioSource> audioSources)
    {
        if (SettingInGame.Instance == null || SettingInGame.Instance.GetIsSound() == false)
        {
            return;
        }
        if (CheckSoundEmpty(soundSO, audioSources))
        {
            CheckSoundEmpty(soundSO, audioSources).PlayOneShot(soundSO.audioClips[
                Random.Range(0, soundSO.audioClips.Count)]);
        }
    }

    public void PlayAudioArrowFly()
    {
        PlayAudioGame(soundSOArrowFly, audioSourcesArrowFly);
    }
    public void PlayAudioArrowHit()
    {
        PlayAudioGame(soundSOArrowHit, audioSourcesArrowHit);
    }
    public void PlayAudioMageShoot()
    {
        PlayAudioGame(soundSOMageShoot, audioSourcesMageShoot);
    }
    public void PlayAudioBomb()
    {
        PlayAudioGame(soundSOBomb, audioSourcesBomb);
    }
    public void PlayAudioBarrackMove()
    {
        PlayAudioGame(soundSOBarrackMove, audioSourcesBarrackMove);
    }

    private AudioSource CheckSoundEmpty(SoundSO soundSO, List<AudioSource> audioSources)
    {
        foreach(AudioSource audioSource in audioSources)
        {
            if(audioSource.isPlaying == false)
            {
                return audioSource;
            }
        }
        return null;
    }


}
