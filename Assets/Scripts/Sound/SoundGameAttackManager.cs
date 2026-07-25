using UnityEngine;
using System.Collections.Generic;

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

    [Header("Sound Solider Attack")]
    [SerializeField] private List<AudioSource> audioSourceSoliderAttack = new List<AudioSource>();
    [SerializeField] private SoundSO soundSOSoliderAttack;

    [Header("Sound Goblin Death")]
    [SerializeField] private List<AudioSource> audioSourceGoblinDeath = new List<AudioSource>();
    [SerializeField] private SoundSO soundSOGoblinDeath;

    [Header("Sound Orc Death")]
    [SerializeField] private List<AudioSource> audioSourceOrcDeath = new List<AudioSource>();
    [SerializeField] private SoundSO soundSOOrcDeath;

    [Header("Sound Sheep")]
    [SerializeField] private List<AudioSource> audioSourceSheep = new List<AudioSource>();
    [SerializeField] private SoundSO soundSOSheep;

    [Header("Sound Flag")]
    [SerializeField] private List<AudioSource> audioSourceFlagPoint = new List<AudioSource>();
    [SerializeField] private SoundSO soundSOFlagPoint;

    [Header("Sound Live Remove")]
    [SerializeField] private List<AudioSource> audioSourceLosseLife = new List<AudioSource>();
    [SerializeField] private SoundSO soundSOLosseLife;

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

    public void PlayAudioSoliderAttack()
    {
        PlayAudioGame(soundSOSoliderAttack, audioSourceSoliderAttack);
    }

    public void PlayAudioGoblinDeath()
    {
        PlayAudioGame(soundSOGoblinDeath, audioSourceGoblinDeath);
    }

    public void PlayAudioOrcDeath()
    {
        PlayAudioGame(soundSOOrcDeath, audioSourceOrcDeath);
    }

    public void PlayAudioSheep()
    {
        PlayAudioGame(soundSOSheep, audioSourceSheep);
    }

    public void PlayAudioFlagPoint()
    {
        PlayAudioGame(soundSOFlagPoint, audioSourceFlagPoint);
    }
    
    public void PlayAudioLosseLife()
    {
        PlayAudioGame(soundSOLosseLife, audioSourceLosseLife);
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
