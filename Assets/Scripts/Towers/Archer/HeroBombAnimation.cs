using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBombAnimation : MonoBehaviour, IHeroAnimation
{
    public int indexHeroIdle;
    public int indexHeroAttack;

    // Triển khai interface (Nhận tham số nhưng không dùng đến)
    public void Idle(Vector2 defaultDir, TowerStateMachine tower)
    {
        SpriteSheetAnimator.Instance.PlayAnimation(
            gameObject,
            tower.GetDataTower().heroBombAnim[indexHeroIdle].nameAnimation,
            tower.GetDataTower().heroBombAnim[indexHeroIdle].startFrame,
            tower.GetDataTower().heroBombAnim[indexHeroIdle].endFrame
        );
    }

    // Triển khai interface (Nhận enemyTarget nhưng không dùng vì trụ bom tự xử lý đạn)
    public void Attack(Transform enemyTarget, TowerStateMachine tower)
    {
        SpriteSheetAnimator.Instance.PlayAnimation(
            gameObject,
            tower.GetDataTower().heroBombAnim[indexHeroAttack].nameAnimation,
            tower.GetDataTower().heroBombAnim[indexHeroAttack].startFrame,
            tower.GetDataTower().heroBombAnim[indexHeroAttack].endFrame,
            frameRate: -1,
            () =>
            {
                Idle(Vector2.zero, tower);
            }
        );
    }
}