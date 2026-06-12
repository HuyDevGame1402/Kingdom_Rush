using UnityEngine;

public class LoadingVisual : MonoBehaviour
{
    [SerializeField] private LoadingTower loadingTower;
    [SerializeField] private Transform loadingValue;
    private float scaleXLoadingValue = 0f;
    private float scaleYLoadingValue = 0f;
    private float currentScaleX = 0;
    private Vector3 localScaleLoadingValue;

    private void Awake()
    {
        loadingTower = GetComponent<LoadingTower>();
        scaleYLoadingValue = loadingValue.localScale.y;
        scaleXLoadingValue = loadingValue.localScale.x;
        localScaleLoadingValue.z = 1f;
    }

    private void Start()
    {
        
        loadingTower.OnInitTower += LoadingTower_OnInitTower;
    }

    private void LoadingTower_OnInitTower(float persentInitTower)
    {
        currentScaleX = persentInitTower * scaleXLoadingValue; 
        localScaleLoadingValue.x = currentScaleX;
        localScaleLoadingValue.y = scaleYLoadingValue;
        loadingValue.localScale = localScaleLoadingValue;
    }
}
