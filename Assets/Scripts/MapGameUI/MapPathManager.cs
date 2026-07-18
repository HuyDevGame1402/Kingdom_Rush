using UnityEngine;

public class MapPathManager : MonoBehaviour
{
    public static MapPathManager Instance { get; private set; }

    [SerializeField] private PolygonCollider2D polygonCollider2D;
    private void Awake()
    {
        Instance = this;
    }

    public void ActivePolygonCollider2D()
    {
        polygonCollider2D.enabled = true;
    }

    public void DisablePolygonCollider2D()
    {
        polygonCollider2D.enabled = false;
    }
}
