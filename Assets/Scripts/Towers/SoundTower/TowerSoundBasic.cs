using UnityEngine;
using System.Collections.Generic;

public class TowerSoundBasic : MonoBehaviour
{
    protected AudioSource audioSource;

    [SerializeField] private List<AudioClip> audioTowerReadys = new List<AudioClip>();
    [SerializeField] private AudioClip audioTowerBuild;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioTowerBuild()
    {
        if (SettingInGame.Instance == null || SettingInGame.Instance.GetIsSound() == false) return;
        audioSource.loop = true;
        audioSource.PlayOneShot(audioTowerBuild);
    }    

    public void PlayAudioTowerReady()
    {
        if (SettingInGame.Instance == null || SettingInGame.Instance.GetIsSound() == false) return;
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.PlayOneShot(audioTowerReadys[Random.Range(0, audioTowerReadys.Count)]);
    }
}
