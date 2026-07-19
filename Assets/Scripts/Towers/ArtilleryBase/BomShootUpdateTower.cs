using System.Collections.Generic;
using UnityEngine;

public class BomShootUpdateTower : MonoBehaviour, IHasUpdateTower
{
    [SerializeField] private TowerLevelUp towerLevelUp;
    [SerializeField] private TowerStateMachine towerStateMachine;
    [SerializeField] private TowerVisual towerVisual;
    [SerializeField] private Transform cannon;
    [SerializeField] private Transform bombSprite;
    [SerializeField] private List<BasePlatfromAnimation> basePlatfromAnimations = new List<BasePlatfromAnimation>();

    private void Awake()
    {
        towerStateMachine = GetComponent<TowerStateMachine>();
        towerLevelUp = GetComponent<TowerLevelUp>();
        towerVisual = GetComponent<TowerVisual>();
    }

    public void UpdateTower(TowerUpLevelSO towerLevelUpSO)
    {
        towerLevelUp.SetTowerLevelUpSO(towerLevelUpSO);
        towerLevelUp.SetCurrentBaseTowerSO(towerLevelUpSO.currentBaseTowerSO);
        towerStateMachine.SetCastleData(towerLevelUpSO.currentCastleData);

        for(int i = 0; i < basePlatfromAnimations.Count; i++)
        {
            basePlatfromAnimations[i].AddLevel();
        }
        if(cannon.TryGetComponent(out ArcherTowerSetupAnimation animationTower))
        {
            animationTower.ReloadAnimationHeroList();
        }
        towerVisual.UpdateTowerVisual();
        Vector3 pos = bombSprite.localPosition;
        pos.y += towerLevelUpSO.currentCastleData.offsetBombDeco;
        bombSprite.localPosition = pos;
        cannon.localPosition = towerLevelUpSO.currentCastleData.offsetCannon;
    }
}
