using UnityEngine;
using System;


public class BuildPlot : MonoBehaviour
{
    public bool isOccupied = false;
    public event Action<Transform,bool> OnClickBuildTower;

    private CapsuleCollider2D capsuleCollider;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void OnMouseDown()
    {
        OnClickBuildTower?.Invoke(transform,isOccupied);
        if(SoundInGameManager.Instance != null)
        {
            SoundInGameManager.Instance.PlaySoundOpenTowerMenu();
        }
    }

    public void EnableCapsualCollider()
    {
        capsuleCollider.enabled = true;
    }
    public void DisableCapsualCollider()
    {
        capsuleCollider.enabled = false;
    }

}
