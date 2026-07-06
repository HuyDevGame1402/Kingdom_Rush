using UnityEngine;

public class SoundInGameManager : MonoBehaviour
{
    public static SoundInGameManager Instance { get; private set; }
    [SerializeField] private AudioClip selectGroundTower;
    [SerializeField] private AudioClip mouseOverTowerIcon;
    [SerializeField] private AudioClip tickWood;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundOpenTowerMenu()
    {
        if (CheckSoundEnabled() == false) return;
        audioSource.Stop();
        audioSource.PlayOneShot(selectGroundTower);
    }

    public void PlayMouseOverTowerIcon()
    {
        if (CheckSoundEnabled() == false) return;
        audioSource.Stop();
        audioSource.PlayOneShot(mouseOverTowerIcon);
    }
    public void PlayTickWood()
    {
        if (CheckSoundEnabled() == false) return;
        audioSource.Stop();
        audioSource.PlayOneShot(tickWood);
    }
    private bool CheckSoundEnabled()
    {
        if(SettingInGame.Instance != null && SettingInGame.Instance.GetIsSound() == true)
        {
            return true;
        }
        return false; 
    }
}
