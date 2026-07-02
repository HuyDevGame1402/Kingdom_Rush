using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AutoSortingOrder : MonoBehaviour
{
    [SerializeField] private int baseOrder = 10000;
    [SerializeField] private int multiplier = 100;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        spriteRenderer.sortingOrder =
            baseOrder - Mathf.RoundToInt(transform.position.y * multiplier);
    }
}