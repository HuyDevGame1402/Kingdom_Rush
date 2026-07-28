using UnityEngine;

// trigger enemy ở vòng cận chiến
public class TriggerEnemyHero : MonoBehaviour
{
    private const string ENEMYTAG = "EnemyKingdomRush";
    [SerializeField] private BaseUnitStateMachine heroStateMachine;

    [SerializeField] private TriggerHeroInSide triggerHeroInSide;

    private CircleCollider2D bigTrigger;

    private void Awake()
    {
        bigTrigger = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!bigTrigger.IsTouching(collision))
            return;
        if (heroStateMachine == null)
        {
            Debug.LogWarning("Biến đang null");
        }
        if (collision.CompareTag(ENEMYTAG) && heroStateMachine.healthHero.IsDead()==false)
        {
            if(collision.transform.TryGetComponent(out EnemyController enemyCtr))
            {
                Debug.LogWarning(
                    $"MELEE ENTER | Script={gameObject.name} | Collider={GetComponent<Collider2D>()?.name} | Enemy={collision.name}");

                if (heroStateMachine.unitData.isLongRangeAttack == false
                    && heroStateMachine.IsTargetEnemy() == false)
                {
                    heroStateMachine.currentTarget = collision.gameObject.transform;
                }

                if(heroStateMachine.unitData.isLongRangeAttack &&
                    heroStateMachine.CheckEnemyInCloseCombat(collision.gameObject.transform))
                {
                    heroStateMachine.currentTarget = collision.gameObject.transform;
                }
            }

            if(heroStateMachine.targetList.Contains(collision.transform) == false)
            {
                heroStateMachine.targetList.Add(collision.transform);
            }
        }

        if(triggerHeroInSide != null && collision.CompareTag("Soldier"))
        {
            Debug.LogWarning($"ENTER : {collision.name} Tag={collision.tag}");
            triggerHeroInSide.AddSolider(collision.transform.parent.GetComponent<BaseUnitStateMachine>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag(ENEMYTAG) && heroStateMachine.targetList.Contains(collision.transform))
        {
            if (collision.transform == heroStateMachine.currentTarget)
            {
                heroStateMachine.ResetTarget();
            }
            heroStateMachine.targetList.Remove(collision.transform);
        }

        if (triggerHeroInSide != null && collision.CompareTag("Soldier"))
        {
            Debug.LogWarning($"EXIT : {collision.transform.parent.name} Tag={collision.tag}");
            triggerHeroInSide.RemoveSolider(collision.transform.parent.GetComponent<BaseUnitStateMachine>());
        }
    }
}
