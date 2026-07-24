using UnityEngine;
using System;


public class BuildPlot : MonoBehaviour
{
    public bool isOccupied = false;
    public event Action<Transform,bool> OnClickBuildTower;

    private CapsuleCollider2D capsuleCollider;

    private Vector3 positionDefault;

    [SerializeField] private Transform spawnPointHero;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        positionDefault = spawnPointHero.position;
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
        isOccupied = false;
        spawnPointHero.position = positionDefault;
    }
    public void DisableCapsualCollider()
    {
        capsuleCollider.enabled = false;
    }

}
