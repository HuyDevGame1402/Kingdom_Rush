using UnityEngine;

public class VictorySound : MonoBehaviour
{
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip soundMerchant;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundVictoryGame()
    {
        audioSource.PlayOneShot(victorySound);
    }

    public void PlaySoundMerchant()
    {
        audioSource.PlayOneShot(soundMerchant);
    }
}
