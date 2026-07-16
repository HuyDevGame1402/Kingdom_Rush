using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFatBoyDeco : MonoBehaviour
{
    public static ExplosionFatBoyDeco Instance { get; private set; }

    [SerializeField] private List<Transform> positionListSpawn = new();

    [SerializeField] private List<Transform> explosionPool = new();

    [SerializeField] private int preloadCount = 10;

    [SerializeField] private int count = 6;

    [SerializeField] private float timeSpawn = 2.5f;

    [SerializeField] private Transform explosionPrefab;

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        while (explosionPool.Count < preloadCount)
        {
            Transform obj = Instantiate(explosionPrefab, transform);

            obj.gameObject.SetActive(false);

            explosionPool.Add(obj);
        }
    }

    public void PlayExplosion()
    {
        StopAllCoroutines();
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        if (positionListSpawn.Count == 0)
            yield break;

        float delay = timeSpawn / count;

        List<int> availableIndex = new();

        for (int i = 0; i < positionListSpawn.Count; i++)
            availableIndex.Add(i);

        int spawnCount = Mathf.Min(count, availableIndex.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            int random = Random.Range(0, availableIndex.Count);

            int index = availableIndex[random];

            availableIndex.RemoveAt(random);

            Transform explosion = GetExplosion();

            explosion.position = positionListSpawn[index].position;

            explosion.rotation = Quaternion.identity;

            explosion.gameObject.SetActive(true);

            if(explosion.TryGetComponent(out ExplosionDecoScript explosionDecoScript))
            {
                explosionDecoScript.ShowAnimation();
            }

            yield return new WaitForSeconds(delay);
        }
    }

    private Transform GetExplosion()
    {
        foreach (Transform item in explosionPool)
        {
            if (!item.gameObject.activeSelf)
                return item;
        }

        Transform obj = Instantiate(explosionPrefab, transform);

        obj.gameObject.SetActive(false);

        explosionPool.Add(obj);

        return obj;
    }
}