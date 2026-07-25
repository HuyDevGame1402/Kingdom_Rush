using UnityEngine;

public class DefeatSound : MonoBehaviour
{
    [SerializeField] private AudioClip defeatSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if(LiveManager.Instance != null)
        {
            LiveManager.Instance.OnGameDefeat += PlayDefeatSound;
        }
    }

    public void PlayDefeatSound()
    {
        audioSource.PlayOneShot(defeatSound);
    }
}
