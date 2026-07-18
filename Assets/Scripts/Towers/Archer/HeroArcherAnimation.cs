using UnityEngine;

public class HeroArcherAnimation : MonoBehaviour, IHeroAnimation
{
    [SerializeField] private Vector2 dir = new Vector2(1, 0);
    [SerializeField] private TowerStateMachine _controller;
    [SerializeField] private GameObject attackSpawn;
    [SerializeField] private Transform firePoint;

    [SerializeField] private ISoundHero soundHero;

    private bool isAttacking;
    private VerticalAnimation currentVertical;

    private void Awake()
    {
        soundHero = GetComponent<ISoundHero>();
    }

    public void Idle(Vector2 dir, TowerStateMachine tower)
    {
        DirectionUtility.GetDirection(
            dir,
            out bool faceLeft,
            out VerticalAnimation vertical
        );

        currentVertical = vertical;
        isAttacking = false;

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

    public void Attack(Transform enemyTarget, TowerStateMachine tower, System.Action onComplete = null)
    {
        dir = (enemyTarget.position - transform.position).normalized;
        DirectionUtility.GetDirection(
            dir,
            out bool faceLeft,
            out VerticalAnimation vertical
        );

        currentVertical = vertical;
        isAttacking = true;

        UpdateFacing(faceLeft);

        Debug.Log($"[LOG TẤN CÔNG] Bắt đầu gọi hàm Attack. Hướng bắn: {vertical}. Mục tiêu: {(enemyTarget != null ? enemyTarget.name : "NULL")}");

        switch (vertical)
        {
            case VerticalAnimation.Down:
                PlayAttackDown(enemyTarget, tower, onComplete);
                break;

            case VerticalAnimation.Up:
                PlayAttackUp(enemyTarget, tower, onComplete);
                break;
        }

        // Sound
        soundHero.PlaySoundHeroAttack();
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

    private void PlayAttackDown(Transform enemyTarget, TowerStateMachine tower, System.Action onComplete)
    {
        int startFrame = tower.GetDataTower().frameHeroStartAttackDown;
        int endFrame = tower.GetDataTower().frameHeroEndAttackDown;

        Debug.Log($"[LOG DOWN] Cấu hình Anim Down: Chạy từ {startFrame} đến {endFrame}. Sinh đạn ở CUỐI hoạt ảnh.");

        
        SpriteSheetAnimator.Instance.PlayAnimation(
            gameObject,
            tower.GetDataTower().animationHero,
            startFrame,
            endFrame,
            -1,
            null,
            0.05f,
            () =>
            {
                Debug.Log($"[LOG EVENT] Hoạt ảnh kết thúc! Gọi SpawnAttack ngay lập tức.");
                if (enemyTarget != null) SpawnAttack(enemyTarget, tower);

                isAttacking = false;

                Debug.Log("[LOG CALLBACK] Quay về Idle và báo Hồi chiêu.");
                Idle(dir, tower);
                onComplete?.Invoke();
            }
        );
    }

    private void PlayAttackUp(Transform enemyTarget, TowerStateMachine tower, System.Action onComplete)
    {
        int startFrame = tower.GetDataTower().frameHeroStartAttackUp;
        int endFrame = tower.GetDataTower().frameHeroEndAttackUp;

        Debug.Log($"[LOG UP] Cấu hình Anim Up: Chạy từ {startFrame} đến {endFrame}. Sinh đạn ở CUỐI hoạt ảnh.");

        SpriteSheetAnimator.Instance.PlayAnimation(
            gameObject,
            tower.GetDataTower().animationHero,
            startFrame,
            endFrame,
            -1,
            null,
            0.05f,
            () =>
            {
                // 🌟 SINH ĐẠN NGAY TẠI ĐÂY - Nơi chắc chắn code sẽ chạy tới
                Debug.Log($"[LOG EVENT] Hoạt ảnh kết thúc! Gọi SpawnAttack ngay lập tức.");
                if (enemyTarget != null) SpawnAttack(enemyTarget, tower);

                Debug.Log("[LOG CALLBACK] Quay về Idle và báo Hồi chiêu.");
                Idle(dir, tower);
                onComplete?.Invoke();
            }
        );
    }

    public void UpdateFacing(bool faceLeft)
    {
        Vector3 scale = transform.localScale;
        float absX = Mathf.Abs(scale.x);
        scale.x = faceLeft ? -absX : absX;
        transform.localScale = scale;
    }

    private void SpawnAttack(Transform enemyTarget, TowerStateMachine tower)
    {
        if (attackSpawn == null)
        {
            Debug.LogError("[LOG LỖI NẶNG] Biến 'attackSpawn' (Prefab đạn) đang bị trống (None) trong Inspector của HeroArcher! Hãy kéo thả viên đạn vào.");
            return;
        }
        if (firePoint == null)
        {
            Debug.LogError("[LOG LỖI NẶNG] Biến 'firePoint' (Vị trí bắn) đang bị trống (None) trong Inspector của HeroArcher! Hãy kéo một Transform vào.");
            return;
        }

        Debug.Log($"[LOG SPAWN] Đang khởi tạo viên đạn từ vị trí: {firePoint.position}");
        GameObject projectileGO = Instantiate(attackSpawn, firePoint.position, Quaternion.identity);

        BaseProjectile projectileScript = projectileGO.GetComponent<BaseProjectile>();
        if (projectileScript != null)
        {
            // 3. Dùng kĩ thuật ép kiểu để phân biệt cách truyền tham số một cách tự động
            if (projectileScript is ArrowKingdomRush arrow)
            {
                // Nếu linh hồn của nó là Mũi tên -> Bắn kiểu Parabol có ArcHeight
                arrow.LaunchWithArc(enemyTarget, tower.GetDataTower().attackSpeed, tower.GetDataTower().arrowArcHeight
                    , DamageStatic.GetDamageBase(tower.GetDataTower().minDamage, tower.GetDataTower().maxDamage));
            }
            else
            {
                // Nếu là Đạn pháp sư (MageBolt) hoặc các loại đạn thẳng sau này -> Chỉ cần truyền tốc độ thẳng
                projectileScript.Launch(enemyTarget, tower.GetDataTower().attackSpeed,
                    DamageStatic.GetDamageBase(tower.GetDataTower().minDamage, tower.GetDataTower().maxDamage));
            }
        }
    }
    public void ReloadAnimation()
    {
        int currentFrame =
            SpriteSheetAnimator.Instance.GetCurrentFrameNumber(gameObject);

        // Nếu đang Idle
        if (!isAttacking)
        {
            switch (currentVertical)
            {
                case VerticalAnimation.Down:
                    {
                        SpriteSheetAnimator.Instance.PlayAnimationContinue(
                            target: gameObject,
                            animPrefix: _controller.GetDataTower().animationHero,
                            startFrame: _controller.GetDataTower().frameHeroStartIdleDown,
                            endFrame: _controller.GetDataTower().frameHeroEndIdleDown,
                            startFromCurrentFrame: currentFrame,
                            frameRate: -1f);

                        break;
                    }

                case VerticalAnimation.Up:
                    {
                        SpriteSheetAnimator.Instance.PlayAnimationContinue(
                            target: gameObject,
                            animPrefix: _controller.GetDataTower().animationHero,
                            startFrame: _controller.GetDataTower().frameHeroStartIdleUp,
                            endFrame: _controller.GetDataTower().frameHeroEndIdleUp,
                            startFromCurrentFrame: currentFrame,
                            frameRate: -1f);

                        break;
                    }
            }

            return;
        }

        // Nếu đang Attack
        switch (currentVertical)
        {
            case VerticalAnimation.Down:
                {
                    SpriteSheetAnimator.Instance.PlayAnimationContinue(
                        target: gameObject,
                        animPrefix: _controller.GetDataTower().animationHero,
                        startFrame: _controller.GetDataTower().frameHeroStartAttackDown,
                        endFrame: _controller.GetDataTower().frameHeroEndAttackDown,
                        startFromCurrentFrame: currentFrame,
                        frameRate: 0.05f,
                        onComplete: () =>
                        {
                            isAttacking = false;
                            Idle(dir, _controller);
                        });

                    break;
                }

            case VerticalAnimation.Up:
                {
                    SpriteSheetAnimator.Instance.PlayAnimationContinue(
                        target: gameObject,
                        animPrefix: _controller.GetDataTower().animationHero,
                        startFrame: _controller.GetDataTower().frameHeroStartAttackUp,
                        endFrame: _controller.GetDataTower().frameHeroEndAttackUp,
                        startFromCurrentFrame: currentFrame,
                        frameRate: 0.05f,
                        onComplete: () =>
                        {
                            isAttacking = false;
                            Idle(dir, _controller);
                        });

                    break;
                }
        }
    }
}