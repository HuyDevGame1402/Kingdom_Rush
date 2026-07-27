using UnityEngine;
using System;

public class OnClickTriggerFlag : MonoBehaviour
{
    [SerializeField] private CapsuleCollider2D capsuleCollider2D;

    [SerializeField] private LayerMask pathLayer;
    [SerializeField] private LayerMask triggerFlag;
    private Vector3 mousePos;
    private Collider2D hit;

    private bool justActivated;

    private Vector3 positionFlag;

    public event Action<Vector3> OnMoveToFlagEvent;
    [SerializeField] private TowerSoundBrackTower towerSoundBrackTower;    
    
    private void OnEnable()
    { 
        justActivated = true;
    }

    private void Update()
    {
        if (justActivated)
        {
            justActivated = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            hit = Physics2D.OverlapPoint(mousePos, triggerFlag);

            if(hit != null)
            {
                Debug.LogWarning("Click vào vùng trong pv");
                if(MapPathManager.Instance != null)
                {
                    MapPathManager.Instance.ActivePolygonCollider2D();
                }
                hit = Physics2D.OverlapPoint(mousePos, pathLayer);

                if(hit != null)
                {
                    Debug.LogWarning("Click vào vùng trong pv Đường");
                    MapPathManager.Instance.DisablePolygonCollider2D();
                    ShowAnimationFlag(mousePos);
                    positionFlag = mousePos;
                    OnMoveToFlagEvent?.Invoke(positionFlag);
                    if (SoundGameAttackManager.Instance != null) SoundGameAttackManager.Instance.PlayAudioFlagPoint();
                    towerSoundBrackTower.PlayAudioTowerReady();
                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("Click vào ngoài đường");
                    ShowAnimationError(mousePos);
                }
            }
            else
            {
                Debug.LogWarning("Click ngoài pv");
                ShowAnimationError(mousePos);
            }
        }
    }

    private void ShowAnimationError(Vector3 pos)
    {
        if(DecoAnimationClick.Instance != null)
        {
            DecoAnimationClick.Instance.PlayAnimationErrorFeedback(pos);
        }
    }

    private void ShowAnimationFlag(Vector3 pos)
    {
        if (DecoAnimationClick.Instance != null)
        {
            DecoAnimationClick.Instance.PlayAnimationFlag(pos);
        }
    }
    
}
