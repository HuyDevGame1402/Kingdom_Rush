using UnityEngine;

public class HeroArcherAnimation : MonoBehaviour, IHeroAnimation
{
    [SerializeField] private Vector2 dir = new Vector2(1, 0);
    [SerializeField] private TowerStateMachine _controller;
    [SerializeField] private GameObject attackSpawn;
    [SerializeField] private Transform firePoint;

    public void Idle(Vector2 dir, TowerStateMachine tower)
    {
        DirectionUtility.GetDirection(
            dir,
            out bool faceLeft,
            out VerticalAnimation vertical
        );

        UpdateFacing(faceLeft);

        switch (vertical)
        {
            case VerticalAnimation.Down:
                PlayIdleDown(tower);
                break;

            case VerticalAnimation.Up:
                PlayIdleUp(tower);
                break;
        }
    }
    public void Attack(Transform enemyTarget, TowerStateMachine tower)
    {
        dir = (enemyTarget.position - transform.position).normalized;
        DirectionUtility.GetDirection(
            dir,
            out bool faceLeft,
            out VerticalAnimation vertical
        );

        UpdateFacing(faceLeft);

        switch (vertical)
        {
            case VerticalAnimation.Down:
                PlayAttackDown(enemyTarget, tower);
                break;

            case VerticalAnimation.Up:
                PlayAttackUp(enemyTarget, tower);
                break;
        }
    }
    private void PlayIdleDown(TowerStateMachine tower)
    {
        SpriteSheetAnimator.Instance.PlayAnimation(
            gameObject,
            tower.GetDataTower().animationHero,
            tower.GetDataTower().frameHeroStartIdleDown,
            tower.GetDataTower().frameHeroEndIdleDown
        );
    }

    private void PlayIdleUp(TowerStateMachine tower)
    {
        SpriteSheetAnimator.Instance.PlayAnimation(
            gameObject,
            tower.GetDataTower().animationHero,
            tower.GetDataTower().frameHeroStartIdleUp,
            tower.GetDataTower().frameHeroEndIdleUp
        );
    }

    private void PlayAttackDown(Transform enemyTarget, TowerStateMachine tower)
    {
        SpriteSheetAnimator.Instance.PlayAnimation(
            gameObject,
            tower.GetDataTower().animationHero,
            tower.GetDataTower().frameHeroStartAttackDown,
            tower.GetDataTower().frameHeroEndAttackDown,
            tower.GetDataTower().frameHeroEndAttackDown - 5,
            () => {
                if (enemyTarget != null) SpawnAttack(enemyTarget, tower);
            },
            - 1, () =>
            {
                Idle(dir, tower);
            }
        );
    }

    private void PlayAttackUp(Transform enemyTarget, TowerStateMachine tower)
    {
        SpriteSheetAnimator.Instance.PlayAnimation(
            gameObject,
            tower.GetDataTower().animationHero,
            tower.GetDataTower().frameHeroStartAttackUp,
            tower.GetDataTower().frameHeroEndAttackUp,
            tower.GetDataTower().frameHeroEndAttackDown - 5,
            () => {
                if (enemyTarget != null) SpawnAttack(enemyTarget, tower);
            },
            -1, () =>
            {
                SpawnAttack(enemyTarget, tower);
                Idle(dir, tower);
            }
        );
    }

    public void UpdateFacing(bool faceLeft)
    {
        Vector3 scale = transform.localScale;
        float absX = Mathf.Abs(scale.x);
        scale.x = faceLeft ? -absX : absX; // ✅ Luôn dựa trên giá trị tuyệt đối
        transform.localScale = scale;
    }
    private void SpawnAttack(Transform enemyTarget, TowerStateMachine tower)
    {
        // 1. Sinh ra Projectile bất kỳ (Arrow hoặc MageBolt đều được)
        GameObject projectileGO = Object.Instantiate(attackSpawn, firePoint.position, Quaternion.identity);

        // 2. Tìm lớp cha chung
        BaseProjectile projectileScript = projectileGO.GetComponent<BaseProjectile>();

        if (projectileScript != null)
        {
            // 3. Dùng kĩ thuật ép kiểu để phân biệt cách truyền tham số một cách tự động
            if (projectileScript is ArrowKingdomRush arrow)
            {
                // Nếu linh hồn của nó là Mũi tên -> Bắn kiểu Parabol có ArcHeight
                arrow.LaunchWithArc(enemyTarget, tower.GetDataTower().attackSpeed, tower.GetDataTower().arrowArcHeight);
            }
            else
            {
                // Nếu là Đạn pháp sư (MageBolt) hoặc các loại đạn thẳng sau này -> Chỉ cần truyền tốc độ thẳng
                projectileScript.Launch(enemyTarget, tower.GetDataTower().attackSpeed);
            }
        }
    }
}
