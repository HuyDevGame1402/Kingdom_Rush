using UnityEngine;

public class SoundInGameManager : MonoBehaviour
{
    public static SoundInGameManager Instance { get; private set; }
    [SerializeField] private AudioClip selectGroundTower;
    [SerializeField] private AudioClip mouseOverTowerIcon;
    [SerializeField] private AudioClip tickWood;
    [SerializeField] private AudioClip clickNextInstruction;
    [SerializeField] private AudioClip levelUp;
    [SerializeField] private AudioClip towerSell;
    [SerializeField] private AudioClip waveComming;

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
    public void PlayTowerSell()
    {
        if (CheckSoundEnabled() == false) return;
        audioSource.Stop();
        audioSource.PlayOneShot(towerSell);
    }
    public void PlayWaveComming()
    {
        if (CheckSoundEnabled() == false) return;
        audioSource.Stop();
        audioSource.PlayOneShot(waveComming);
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
