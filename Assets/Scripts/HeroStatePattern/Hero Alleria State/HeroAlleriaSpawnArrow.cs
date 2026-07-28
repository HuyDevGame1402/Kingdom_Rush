using UnityEngine;

public class HeroAlleriaSpawnArrow : MonoBehaviour, IHasSpawnBullet
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private float shootSpeed;
    [SerializeField] private float height = 2f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private HeroDataInGame heroDataInGame;

    public void SpawnBullet(Transform enemyTarget, int finnalDamage)
    {
        GameObject projectileGO = Instantiate(arrow, firePoint.position, Quaternion.identity);

        BaseProjectile projectileScript = projectileGO.GetComponent<BaseProjectile>();
        if (projectileScript != null)
        {
            if (projectileScript is ArrowKingdomRush arrow)
            {
                arrow.LaunchWithArc(enemyTarget, shootSpeed, height
                        , finnalDamage);
            }
        }
    }
}
