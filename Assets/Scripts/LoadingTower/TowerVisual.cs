using System.Collections.Generic;
using UnityEngine;

public class TowerVisual : MonoBehaviour
{
    [SerializeField] private List<GameObject> towerGameObject;
    [SerializeField] private LoadingTower loadingTower;
    [SerializeField] private Vector3 localScaleTower;
    [SerializeField] private Vector3 localScaleHero;
    [SerializeField] private Vector3 localScaleCannon;

    [Header("Bomb")]
    [SerializeField] private List<GameObject> bombObjectUpdateScale;
    [SerializeField] private Transform cannon;

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
        for (int i = 0; i < bombObjectUpdateScale.Count; i++)
        {
            bombObjectUpdateScale[i].transform.localScale = localScaleHero;
        }
        if (cannon != null)
        {
            cannon.localPosition = transform.GetComponent<TowerStateMachine>()
                .GetDataTower().offsetCannon;
            cannon.transform.localScale = localScaleCannon;
        }
        if(transform.TryGetComponent(out TowerStateMachine towerStateMachine))
        {
            Vector3 pos = transform.position;
            pos.x += towerStateMachine.GetDataTower().offsetXTower;
            transform.position = pos;
        }
    }
    public void UpdateTowerVisual()
    {
        ShowTower();
    }
}
