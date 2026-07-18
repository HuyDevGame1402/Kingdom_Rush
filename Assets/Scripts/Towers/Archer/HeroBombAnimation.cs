//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class HeroBombAnimation : MonoBehaviour, IHeroAnimation
//{
//    public int indexHeroIdle;
//    public int indexHeroAttack;

//    // Triển khai interface (Nhận tham số nhưng không dùng đến)
//    public void Idle(Vector2 defaultDir, TowerStateMachine tower)
//    {
//        SpriteSheetAnimator.Instance.PlayAnimation(
//            gameObject,
//            tower.GetDataTower().heroBombAnim[indexHeroIdle].nameAnimation,
//            tower.GetDataTower().heroBombAnim[indexHeroIdle].startFrame,
//            tower.GetDataTower().heroBombAnim[indexHeroIdle].endFrame
//        );
//    }

//    // Triển khai interface (Nhận enemyTarget nhưng không dùng vì trụ bom tự xử lý đạn)
//    public void Attack(Transform enemyTarget, TowerStateMachine tower)
//    {
//        SpriteSheetAnimator.Instance.PlayAnimation(
//            gameObject,
//            tower.GetDataTower().heroBombAnim[indexHeroAttack].nameAnimation,
//            tower.GetDataTower().heroBombAnim[indexHeroAttack].startFrame,
//            tower.GetDataTower().heroBombAnim[indexHeroAttack].endFrame,
//            frameRate: -1,
//            () =>
//            {
//                Idle(Vector2.zero, tower);
//            }
//        );
//    }
//}

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

    // ✅ Đã sửa: Thêm tham số System.Action onComplete = null để khớp với Interface IHeroAnimation
    public void Attack(Transform enemyTarget, TowerStateMachine tower, System.Action onComplete = null)
    {
        SpriteSheetAnimator.Instance.PlayAnimation(
            gameObject,
            tower.GetDataTower().heroBombAnim[indexHeroAttack].nameAnimation,
            tower.GetDataTower().heroBombAnim[indexHeroAttack].startFrame,
            tower.GetDataTower().heroBombAnim[indexHeroAttack].endFrame,
            frameRate: -1,
            () =>
            {
                // Khi hoạt ảnh ném bom kết thúc hoàn toàn:
                Idle(Vector2.zero, tower);

                // 🌟 QUAN TRỌNG: Báo cho State Machine biết là đã diễn xong hoạt ảnh để chuyển sang Cooldown!
                onComplete?.Invoke();
            }
        );
    }
    public void ReloadAnimation()
    {

    }
}