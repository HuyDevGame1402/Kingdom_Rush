using System.Collections.Generic;
using UnityEngine;

public class TowerVisual : MonoBehaviour
{
    [SerializeField] private List<GameObject> towerGameObject;
    [SerializeField] private LoadingTower loadingTower;
    [SerializeField] private Vector3 localScaleTower;

    private void Awake()
    {
        loadingTower.OnCompletedInitTower += LoadingTower_OnCompletedInitTower;
    }

    private void LoadingTower_OnCompletedInitTower(object sender, System.EventArgs e)
    {
        ShowTower();
    }
    private void ShowTower()
    {
        Debug.LogWarning("Show Tower");
        for(int i = 0; i < towerGameObject.Count; i++)
        {
            towerGameObject[i].transform.localScale = localScaleTower;
        }
    }
}
