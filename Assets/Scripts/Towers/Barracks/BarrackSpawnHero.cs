using UnityEngine;

public class BarrackSpawnHero : MonoBehaviour
{
    [SerializeField] private Transform targetSpawn;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject heroTower;
    private int heroCountSpawn = 3;
    private GameObject heroSpawn;

    private void Start()
    {
        transform.GetComponent<BarracksAnimation>().OnSpawnHeroEvent += SpawnHero;
    }

    private void SpawnHero()
    {
        for(int i = 0; i < heroCountSpawn; i++)
        {
            heroSpawn = Instantiate(heroTower, spawnPoint.position, Quaternion.identity);
            heroSpawn.GetComponent<BaseUnitStateMachine>().currentTarget = targetSpawn;
        }
    }
    public void SetTargetSpawn(Transform targetTransform)
    {
        targetSpawn = targetTransform;
    }
}
