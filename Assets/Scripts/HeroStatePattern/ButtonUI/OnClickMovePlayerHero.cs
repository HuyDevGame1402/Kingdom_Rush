using System;
using UnityEngine;

public class OnClickMovePlayerHero : MonoBehaviour
{
    private bool justActivated;

    [SerializeField] private LayerMask pathLayer;
    private Vector3 mousePos;
    private Collider2D hit;

    public Action<Vector3> OnClickMoveToFlag;

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

            hit = Physics2D.OverlapPoint(mousePos, pathLayer);

            if (hit != null)
            {
                Debug.LogWarning("Click vào vùng trong pv Đường");
                MapPathManager.Instance.DisablePolygonCollider2D();
                ShowAnimationFlag(mousePos);
                OnClickMoveToFlag?.Invoke(mousePos);
                if (SoundGameAttackManager.Instance != null) SoundGameAttackManager.Instance.PlayAudioFlagPoint();
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
        if (DecoAnimationClick.Instance != null)
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
