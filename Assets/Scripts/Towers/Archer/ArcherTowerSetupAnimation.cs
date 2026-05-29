using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTowerSetupAnimation : MonoBehaviour
{
    // Thay vì kéo trực tiếp Script Archer, ta kéo các GameObject chứa script Hero vào đây
    [SerializeField] private List<GameObject> heroGameObjects = new List<GameObject>();
    [SerializeField] private float _timeInitTower = 1.5f;
    [SerializeField] private TowerStateMachine tower;
    [SerializeField] private GameObject spawnAttack;
    [SerializeField] private bool isSpawnAttack;
    [SerializeField] private BasePlatfromAnimation basePlatfromAnimation;
    [SerializeField] private GameObject smokeOb;
    [SerializeField] private GameObject firePoint;

    private List<IHeroAnimation> heroList = new List<IHeroAnimation>();
    public int indexHeroAttack = 0;

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
        if (basePlatfromAnimation != null) basePlatfromAnimation.PlatfromAnimation();
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

    public void Attack(Transform enemyTarget, TowerStateMachine tower)
    {
        TowerAttackAnimation(
            tower.GetDataTower().animationTower,
            tower.GetDataTower().frameTowerStartAttack,
            tower.GetDataTower().frameTowerEndAttack,
            tower.GetDataTower().frameTowerStartIdle,
            tower.GetDataTower().frameTowerEndIdle
        );
        if(smokeOb != null)
        {
            smokeOb.SetActive(true);
            SpriteSheetAnimator.Instance.PlayAnimation(smokeOb,tower.GetDataTower().smokeAnim.nameAnimation,
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
            SpawnAttack(enemyTarget);
        }
        if (heroList.Count > 0)
        {
            if(indexHeroAttack == -1)
            {
                for(int i = 0; i < heroList.Count; i++)
                {
                    heroList[i].Attack(enemyTarget, tower);
                }
                return;
            }
            // Gọi Attack qua Interface, hệ thống tự biết Archer hay Bomb để chạy code tương ứng
            heroList[indexHeroAttack].Attack(enemyTarget, tower);
            indexHeroAttack++;
            if (indexHeroAttack >= heroList.Count)
            {
                indexHeroAttack = 0;
            }
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
    private void SpawnAttack(Transform enemyTarget)
    {
        GameObject projectileGO = Object.Instantiate(spawnAttack, firePoint.transform.position, Quaternion.identity);

        // 2. Tìm lớp cha chung
        BaseProjectile projectileScript = projectileGO.GetComponent<BaseProjectile>();

        if (projectileScript != null)
        {
            // 3. Dùng kĩ thuật ép kiểu để phân biệt cách truyền tham số một cách tự động
            if (projectileScript is BombProjectile bomb)
            {
                // Nếu linh hồn của nó là Mũi tên -> Bắn kiểu Parabol có ArcHeight
                bomb.LaunchWithArc(enemyTarget, tower.GetDataTower().attackSpeed, tower.GetDataTower().arrowArcHeight);
            }
        }
    }
}
