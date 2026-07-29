using UnityEngine;

public class HeroAlleriaSpawnArrow : MonoBehaviour, IHasSpawnBullet
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private float shootSpeed;
    [SerializeField] private float height = 2f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private HeroDataInGame heroDataInGame;
    GameObject projectileGO;
    BaseProjectile projectileScript;

    public void SpawnBullet(Transform enemyTarget, int finnalDamage, bool isSkill)
    {
        projectileGO = Instantiate(arrow, firePoint.position, Quaternion.identity);

        projectileScript = projectileGO.GetComponent<BaseProjectile>();
        if (projectileScript != null)
        {
            if (projectileScript is ArrowKingdomRush arrow)
            {
                arrow.LaunchWithArc(enemyTarget, shootSpeed, height
                        , finnalDamage, transform);
            }

            if (isSkill)
            {
                projectileGO.GetComponent<ArrowColorEffect>().SetBlue();
            }
        }
    }
}
