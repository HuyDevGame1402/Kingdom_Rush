using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTowerSetupAnimation : MonoBehaviour
{
    // Thay vì kéo trực tiếp Script Archer, ta kéo các GameObject chứa script Hero vào đây
    [SerializeField] private List<GameObject> heroGameObjects = new List<GameObject>();
    private float _timeInitTower = 0.5f;
    [SerializeField] private TowerStateMachine tower;
    [SerializeField] private GameObject spawnAttack;
    [SerializeField] private bool isSpawnAttack;
    [SerializeField] private List<BasePlatfromAnimation> basePlatfromAnimations = new List<BasePlatfromAnimation>();
    [SerializeField] private BasePlatfromAnimation basePlatfromAnimation;
    [SerializeField] private GameObject smokeOb;
    [SerializeField] private GameObject firePoint;

    private List<IHeroAnimation> heroList = new List<IHeroAnimation>();
    public int indexHeroAttack = 0;
    public Transform enemyTest;

    private void Awake()
    {
        // Tự động lấy Interface từ các GameObject đã kéo vào Inspector
        foreach (var heroGO in heroGameObjects)
        {
            var heroAnim = heroGO.GetComponent<IHeroAnimation>();
            if (heroAnim != null)
            {
                heroList.Add(heroAnim);
            }
            else
            {
                Debug.LogError($"GameObject {heroGO.name} không có script kế thừa IHeroAnimation!");
            }
        }
    }

    public void InitTower(string name, int frameStart, int frameEnd)
    {
        if (basePlatfromAnimation != null)
        {
            for(int i = 0; i < basePlatfromAnimations.Count; i++)
            {
                basePlatfromAnimations[i].PlatfromAnimation();
            }
            //basePlatfromAnimation.PlatfromAnimation();
        }
        SpriteSheetAnimator.Instance.PlayAnimation(gameObject, name, frameStart, frameEnd);
    }

    public void CreateTower(CastleData _dataTower)
    {
        StartCoroutine(CoroutineCreateTower(_dataTower));
    }

    public void IdleHeros()
    {
        for (int i = 0; i < heroList.Count; i++)
        {
            // Truyền hướng mặc định (1, 0) - Cung thủ sẽ dùng, Trụ bom sẽ phớt lờ
            heroList[i].Idle(new Vector2(1, 0), tower);
        }
    }

    private void SetupHeroPosition(CastleData _dataTower)
    {
        for (int i = 0; i < heroGameObjects.Count; i++)
        {
            heroGameObjects[i].transform.localPosition = _dataTower.heroPositionList[i];
        }
    }
    public void Attack(Transform enemyTarget, TowerStateMachine tower, 
        System.Action onComplete = null, int damage = 1)
    {
        TowerAttackAnimation(
            tower.GetDataTower().animationTower,
            tower.GetDataTower().frameTowerStartAttack,
            tower.GetDataTower().frameTowerEndAttack,
            tower.GetDataTower().frameTowerStartIdle,
            tower.GetDataTower().frameTowerEndIdle
        );

        if (smokeOb != null)
        {
            smokeOb.SetActive(true);
            SpriteSheetAnimator.Instance.PlayAnimation(smokeOb, tower.GetDataTower().smokeAnim.nameAnimation,
                tower.GetDataTower().smokeAnim.startFrame,
                tower.GetDataTower().smokeAnim.endFrame,
                frameRate: -1,
                () =>
                {
                    SpriteSheetAnimator.Instance.PlayAnimation(smokeOb, tower.GetDataTower().smokeAnim.nameAnimation,
                    1,
                    1);
                    smokeOb.SetActive(false);
                }
            );
        }

        if (isSpawnAttack)
        {
            SpawnAttack(enemyTarget, damage);
            // Nếu đây là trụ bắn thẳng (như trụ bom tự spawn đạn không qua hero), 
            // thì gọi onComplete luôn sau khi spawn đạn
            onComplete?.Invoke();
        }

        if (heroList.Count > 0)
        {
            if (indexHeroAttack == -1)
            {
                for (int i = 0; i < heroList.Count; i++)
                {
                    // Truyền onComplete vào từng hero
                    heroList[i].Attack(enemyTarget, tower, onComplete);
                }
                return;
            }

            // Truyền onComplete vào hero được chỉ định tấn công lượt này
            heroList[indexHeroAttack].Attack(enemyTarget, tower, onComplete);

            indexHeroAttack++;
            if (indexHeroAttack >= heroList.Count)
            {
                indexHeroAttack = 0;
            }
        }
        else if (!isSpawnAttack)
        {
            // Phòng hờ nếu trụ không có hero cũng không tự spawn đạn để tránh treo State
            onComplete?.Invoke();
        }
    }

    private void TowerAttackAnimation(string nameAnim, int frameStart, int frameEnd, int frameStartIdle, int frameEndIdle)
    {
        SpriteSheetAnimator.Instance.PlayAnimation(gameObject, nameAnim, frameStart, frameEnd, 0.1f, () =>
        {
            SpriteSheetAnimator.Instance.PlayAnimation(gameObject, nameAnim, frameStartIdle, frameEndIdle);
        });
    }

    private IEnumerator CoroutineCreateTower(CastleData _dataTower)
    {
        yield return new WaitForSeconds(_timeInitTower);
        InitTower(_dataTower.animationTower, _dataTower.frameTowerStartIdle, _dataTower.frameTowerEndIdle);
        SetupHeroPosition(_dataTower);
        IdleHeros();
    }
    private void SpawnAttack(Transform enemyTarget, int damage = 1)
    {
        GameObject projectileGO = Instantiate(spawnAttack, firePoint.transform.position, Quaternion.identity);

        // 2. Tìm lớp cha chung
        BaseProjectile projectileScript = projectileGO.GetComponent<BaseProjectile>();

        if (projectileScript != null)
        {
            // 3. Dùng kĩ thuật ép kiểu để phân biệt cách truyền tham số một cách tự động
            if (projectileScript is BombProjectile bomb)
            {
                // Nếu linh hồn của nó là Mũi tên -> Bắn kiểu Parabol có ArcHeight
                bomb.LaunchWithArc(enemyTarget, tower.GetDataTower().attackSpeed, 
                    tower.GetDataTower().arrowArcHeight, damage);
            }
        }
    }

    public void ReloadAnimationHeroList()
    {
        for(int i = 0; i < heroList.Count; i++)
        {
            heroList[i].ReloadAnimation();
        }
    }

    public void ReloadAnimationTower()
    {
        int currentFrame =
            SpriteSheetAnimator.Instance.GetCurrentFrameNumber(gameObject);

        CastleData data = tower.GetDataTower();

        // Nếu đang Attack thì chạy tiếp phần Attack còn dang dở
        if (tower.GetCurrentState() == tower.AttackState)
        {
            SpriteSheetAnimator.Instance.PlayAnimationContinue(
                gameObject,
                data.animationTower,
                data.frameTowerStartAttack,
                data.frameTowerEndAttack,
                currentFrame,
                0.1f,
                () =>
                {
                    // Attack xong chuyển về Idle
                    SpriteSheetAnimator.Instance.PlayAnimation(
                        gameObject,
                        data.animationTower,
                        data.frameTowerStartIdle,
                        data.frameTowerEndIdle);
                });
        }
        else
        {
            // Idle thì chạy loop tiếp từ frame hiện tại
            SpriteSheetAnimator.Instance.PlayAnimation(
                gameObject,
                data.animationTower,
                data.frameTowerStartIdle,
                data.frameTowerEndIdle,
                currentFrame);
        }
    }
}
