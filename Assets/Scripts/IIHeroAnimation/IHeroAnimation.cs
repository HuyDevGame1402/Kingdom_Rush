using UnityEngine;

public interface IHeroAnimation
{
    void Idle(Vector2 defaultDir, TowerStateMachine tower);
    //void Attack(Transform enemyTarget, TowerStateMachine tower);
    void Attack(Transform enemyTarget, TowerStateMachine tower, System.Action onComplete = null);
}