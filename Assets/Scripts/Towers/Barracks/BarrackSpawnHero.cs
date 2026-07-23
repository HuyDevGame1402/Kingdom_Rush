using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BarrackSpawnHero : MonoBehaviour
{
    [SerializeField] private Transform targetSpawn;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject heroTower;
    [SerializeField] private BarracksAnimation barracksAnimation;
    private int heroCountSpawn = 3;
    private GameObject heroSpawn;

    public List<GameObject> heroSpawnList = new List<GameObject>();
    [SerializeField] private TowerSoundBrackTower towerSoundBasic;

    [SerializeField] private OnClickTriggerFlag onClickTriggerFlag;

    private void Start()
    {
        if(barracksAnimation == null)
        {
            barracksAnimation = GetComponent<BarracksAnimation>();
        }
        barracksAnimation.OnSpawnHeroEvent += SpawnHero;
        onClickTriggerFlag.OnMoveToFlagEvent += OnClickTriggerFlag_OnMoveToFlagEvent;
    }

    public void OnClickTriggerFlag_OnMoveToFlagEvent(Vector3 pos)
    {
        Debug.LogWarning("Chạy đến cờ!");
        for(int i = 0; i < heroCountSpawn; i++)
        {
            if (heroSpawnList[i].GetComponent<BaseUnitStateMachine>().isDead == false)
            {
                heroSpawnList[i].GetComponent<BaseUnitStateMachine>().positionFlag = pos;
                heroSpawnList[i].GetComponent<BaseUnitStateMachine>().isRunToFlag = true;
                heroSpawnList[i].GetComponent<BaseUnitStateMachine>().TransitionToState(
                    heroSpawnList[i].GetComponent<BaseUnitStateMachine>().RunState);
            }
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < heroSpawnList.Count; i++)
        {
            heroSpawnList[i].gameObject.SetActive(false);
        }
    }

    private void SpawnHero()
    {
        for(int i = 0; i < heroCountSpawn; i++)
        {
            heroSpawn = Instantiate(heroTower, spawnPoint.position, Quaternion.identity);
            heroSpawn.GetComponent<BaseUnitStateMachine>().currentTarget = targetSpawn;
            heroSpawn.GetComponent<BaseUnitStateMachine>().SetParent(transform);
            heroSpawnList.Add(heroSpawn);
        }
        towerSoundBasic.PlayAudioTowerReady();
    }

    public void ResurrectionHero(Transform hero, float time)
    {
        StartCoroutine(CoroutineResurrectionHero(hero, time));  
    }

    private IEnumerator CoroutineResurrectionHero(Transform hero, float time)
    {
        yield return new WaitForSeconds(time);
        if (barracksAnimation.isDoorOpen)
        {
            SpawnHeroResurrection(hero);
        }
        else
        {
            towerSoundBasic.PlayAudioDoorOpen();
            barracksAnimation.OpenDoor();
            SpawnHeroResurrection(hero);
        }
        yield return new WaitForSeconds(barracksAnimation.GetTimeOpenDoor());
        barracksAnimation.CloseDoor();
    }

    private void SpawnHeroResurrection(Transform hero)
    {
        hero.gameObject.SetActive(true);
        if (hero.TryGetComponent(out IResurrection heroResurrection))
        {
            heroResurrection.Resurrection(spawnPoint, targetSpawn);
        }
    }

    public void SetTargetSpawn(Transform targetTransform)
    {
        targetSpawn = targetTransform;
    }
}
