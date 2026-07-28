using UnityEngine;

public class TriggerEnemy : MonoBehaviour
{
    private const string SOLIDERTAG = "Soldier";
    [SerializeField] private EnemyController enemyController;
    private CircleCollider2D bigTrigger;
    private void Awake()
    {
        if (enemyController == null)
        {
            enemyController = GetComponentInParent<EnemyController>();
        }
        bigTrigger = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!bigTrigger.IsTouching(collision))
            return;
        if (collision.CompareTag(SOLIDERTAG))
        {
            if(collision.transform.parent.TryGetComponent(out BaseUnitStateMachine heroStateMachine))
            {
                if (heroStateMachine.CheckAttackerCount() && enemyController.target == null)
                {
                    enemyController.target = collision.transform.parent;
                    heroStateMachine.attackerCount += 1;
                }
            }
            else
            {
                Debug.LogWarning("Không tìm thấy BaseUnitStateMachine trên đối tượng: " + collision.transform.parent.name);
            }
            if(enemyController.targetList.Contains(collision.transform.parent) == false)
            {
                enemyController.targetList.Add(collision.transform.parent);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(SOLIDERTAG) && enemyController.targetList.Contains(collision.transform.parent))
        {
            if (collision.transform.parent == enemyController.target)
            {
                enemyController.ResetTarget();
            }
            enemyController.targetList.Remove(collision.transform.parent);
        }
    }
}
