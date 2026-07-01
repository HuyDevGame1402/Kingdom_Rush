using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttackTowerKingdomRush : MonoBehaviour
{
    [SerializeField] private TowerStateMachine towerStateMachine;
    public List<Transform> targets = new List<Transform>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyKingdomRush"))
        {
            if(towerStateMachine.CurrentTarget == null) 
                towerStateMachine.SetTargetEnemy(collision.transform);
            if(targets.Contains(collision.transform) == false) 
                targets.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyKingdomRush"))
        {
            if (targets.Contains(collision.transform) == true)
                targets.Remove(collision.transform);
            if (towerStateMachine.CurrentTarget != null && towerStateMachine.CurrentTarget == collision.transform)
            {
                if(targets.Count > 0)
                {
                    towerStateMachine.SetTargetEnemy(targets[0]);
                }
                else
                {
                    towerStateMachine.SetTargetEnemy(null);
                }
            }    
        }
    }
}
