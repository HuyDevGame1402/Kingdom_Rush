using UnityEngine;

// trigger enemy ở vòng đánh xa
public class TriggerEnemyHeroRange : MonoBehaviour
{
    private const string ENEMYTAG = "EnemyKingdomRush";
    [SerializeField] private BaseUnitStateMachine heroStateMachine;

    private void Awake()
    {
        if (heroStateMachine == null)
        {
            heroStateMachine = transform.parent.GetComponent<BaseUnitStateMachine>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(heroStateMachine == null)
        {
            Debug.LogWarning("Biến đang null");
        }
        if (collision.CompareTag(ENEMYTAG) && heroStateMachine.healthHero.IsDead() == false)
        {
            if (collision.transform.TryGetComponent(out EnemyController enemyCtr)
                && heroStateMachine.IsTargetEnemy() == false)
            {
                heroStateMachine.currentTarget = collision.gameObject.transform;
            }

            if (heroStateMachine.targetLongRangeList.Contains(collision.transform) == false)
            {
                heroStateMachine.targetLongRangeList.Add(collision.transform);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag(ENEMYTAG) && heroStateMachine.targetLongRangeList.Contains(collision.transform))
        {
            heroStateMachine.targetLongRangeList.Remove(collision.transform);
            if (collision.transform == heroStateMachine.currentTarget)
            {
                heroStateMachine.ResetTarget();
            }
        }
    }
}