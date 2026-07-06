using UnityEngine;

public class TowerSoundBrackTower : TowerSoundBasic
{
    [SerializeField] private AudioClip audioDoorOpen;


    public void PlayAudioDoorOpen()
    {
        if (SettingInGame.Instance == null || SettingInGame.Instance.GetIsSound() == false) return;
        audioSource.Stop();
        audioSource.PlayOneShot(audioDoorOpen);
    }
}
