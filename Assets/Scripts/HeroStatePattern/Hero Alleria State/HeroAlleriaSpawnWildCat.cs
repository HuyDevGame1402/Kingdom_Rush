using System.Collections;
using UnityEngine;

public class HeroAlleriaSpawnWildCat : MonoBehaviour
{
    [SerializeField] HeroAlleriaController heroAlleriaController;
    [SerializeField] private GameObject wildCatPrefab;
    [SerializeField] private float radiusSpawn;
    [SerializeField] private float wildcatRespawnTime = 20f;
    [SerializeField] private bool isReadySpawn = true;
    private GameObject wildCatPrefabSpawn;

    public void CreateWildCat()
    {
        wildCatPrefabSpawn = Instantiate(wildCatPrefab, (Vector2)transform.position + Random.insideUnitCircle * radiusSpawn,
            Quaternion.identity);

        heroAlleriaController.SetWildCat(wildCatPrefabSpawn);

        wildCatPrefabSpawn.GetComponent<HealthHero>().OnDead += HeroAlleriaSpawnWildCat_OnDead;
        wildCatPrefabSpawn.GetComponent<WildCatInit>().SetHeroOwnerEXPManager(
            transform.GetComponent<HeroEXPManager>());
        isReadySpawn = false;
    }

    public void RespawnWildCat()
    {
        wildCatPrefabSpawn.GetComponent<WildCatInit>().ActiveWildCat(false);
        wildCatPrefabSpawn.GetComponent<HealthHero>().ResetHealth();
        wildCatPrefabSpawn.SetActive(true);
        wildCatPrefabSpawn.transform.position = (Vector2)transform.position + Random.insideUnitCircle * radiusSpawn;
        isReadySpawn = false;
    }

    private void HeroAlleriaSpawnWildCat_OnDead()
    {
        StartCoroutine(CoroutineRespawn());
    }

    private IEnumerator CoroutineRespawn()
    {
        yield return new WaitForSeconds(wildcatRespawnTime);
        isReadySpawn = true;
    }

    public bool GetIsReadySpawn()
    {
        return isReadySpawn;
    }
}
