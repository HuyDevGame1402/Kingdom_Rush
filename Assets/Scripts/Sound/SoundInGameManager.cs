using UnityEngine;

public class SoundInGameManager : MonoBehaviour
{
    public static SoundInGameManager Instance { get; private set; }
    [SerializeField] private AudioClip selectGroundTower;
    [SerializeField] private AudioClip mouseOverTowerIcon;
    [SerializeField] private AudioClip tickWood;
    [SerializeField] private AudioClip clickNextInstruction;
    [SerializeField] private AudioClip levelUp;

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
    public void PlayClickNextInstruction()
    {
        if (CheckSoundEnabled() == false) return;
        audioSource.Stop();
        audioSource.PlayOneShot(clickNextInstruction);
    }
    public void PlayLevelUp()
    {
        if (CheckSoundEnabled() == false) return;
        audioSource.Stop();
        audioSource.PlayOneShot(levelUp);
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
