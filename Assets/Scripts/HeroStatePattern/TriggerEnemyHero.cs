using UnityEngine;

public class TriggerEnemyHero : MonoBehaviour
{
    private const string ENEMYTAG = "EnemyKingdomRush";
    [SerializeField] private BaseUnitStateMachine heroStateMachine;

    private void Awake()
    {
        if (heroStateMachine == null)
        {
            heroStateMachine = GetComponent<BaseUnitStateMachine>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ENEMYTAG)
            && heroStateMachine.healthHero.IsDead()==false)
        {
            if(collision.transform.TryGetComponent(out EnemyController enemyCtr)
                /*&& enemyCtr.CheckAttackerCount()*/ && heroStateMachine.IsTargetEnemy() == false)
            {
                //enemyCtr.attackerCount += 1;
                heroStateMachine.currentTarget = collision.gameObject.transform;
            }

            if(heroStateMachine.targetList.Contains(collision.transform) == false)
            {
                heroStateMachine.targetList.Add(collision.transform);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(ENEMYTAG) && heroStateMachine.targetList.Contains(collision.transform))
        {
            if (collision.transform == heroStateMachine.currentTarget)
            {
                heroStateMachine.ResetTarget();
            }
            heroStateMachine.targetList.Remove(collision.transform);
        }
        else
        {
            Debug.LogError("Lỗi");
        }
    }
}
