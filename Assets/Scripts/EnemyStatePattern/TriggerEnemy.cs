using UnityEngine;

public class TriggerEnemy : MonoBehaviour
{
    private const string SOLIDERTAG = "Soldier";
    [SerializeField] private EnemyController enemyController;

    private void Awake()
    {
        if (enemyController == null)
        {
            enemyController = GetComponentInParent<EnemyController>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(SOLIDERTAG) && enemyController.target == null)
        {
            if(collision.transform.TryGetComponent(out BaseUnitStateMachine heroStateMachine))
            {
                if (heroStateMachine.CheckAttackerCount() && enemyController.target == null)
                {
                    enemyController.target = collision.transform;
                    heroStateMachine.attackerCount += 1;
                }
            }
            else
            {
                Debug.LogWarning("Không tìm thấy BaseUnitStateMachine trên đối tượng: " + collision.transform.name);
            }
            if(enemyController.targetList.Contains(collision.transform) == false)
            {
                enemyController.targetList.Add(collision.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if(collision.transform == enemyController.target)
        //{
        //    enemyController.ResetTarget();
        //}
        //else
        //{
        //    if (collision.CompareTag(SOLIDERTAG))
        //    {
        //        if (enemyController.targetList.Contains(collision.transform) == true)
        //        {
        //            enemyController.targetList.Remove(collision.transform);
        //        }
        //    }
        //}
        if (collision.CompareTag(SOLIDERTAG))
        {
            if (enemyController.targetList.Contains(collision.transform) == true)
            {
                enemyController.targetList.Remove(collision.transform);
            }
        }
    }
}
