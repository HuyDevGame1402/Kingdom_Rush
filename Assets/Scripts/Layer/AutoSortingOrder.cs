using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AutoSortingOrder : MonoBehaviour
{
    [SerializeField] private int baseOrder = 10000;
    private int multiplier = 1000;

    private SpriteRenderer spriteRenderer;

    private int tieBreaker;

    private static int spawnCounter = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        tieBreaker = spawnCounter % 50; // giới hạn biên độ nhỏ để không ảnh hưởng thứ tự Y thật
        spawnCounter++;
    }

    private void LateUpdate()
    {
        spriteRenderer.sortingOrder = baseOrder - Mathf.RoundToInt(transform.position.y * multiplier) + tieBreaker;
    }
}