using UnityEngine;
using System;

public class OnClickUseOption : MonoBehaviour
{
    [SerializeField] private LayerMask pathLayer;
    private Vector2 mousePos;
    private Collider2D hit;

    public event Action<Vector3> OnClick;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            hit = Physics2D.OverlapPoint(mousePos, pathLayer);

            if (hit != null)
            {
                OnClick?.Invoke(mousePos);
            }
            else
            {
                Debug.Log("Không phải đường");
            }
        }
    }
}
