using UnityEngine;

public class IceStaffEndSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;    

    private void Awake()
    {
        transform.GetComponent<IceUI>().OnDisableGameObjectIce += IceStaffEndSound_OnDisableGameObjectIce;
    }

    private void IceStaffEndSound_OnDisableGameObjectIce()
    {
        audioSource.PlayOneShot(clip);
    }
}
