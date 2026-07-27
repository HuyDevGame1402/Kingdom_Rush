using UnityEngine;

// trigger enemy ở vòng cận chiến
public class TriggerEnemyHero : MonoBehaviour
{
    private const string ENEMYTAG = "EnemyKingdomRush";
    [SerializeField] private BaseUnitStateMachine heroStateMachine;

    //private void Awake()
    //{
    //    if (heroStateMachine == null)
    //    {
    //        heroStateMachine = GetComponent<BaseUnitStateMachine>();
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
    }
}

//private void OnCollisionExit2D(Collision2D collision)
//{
//    if (collision.collider.CompareTag(ENEMYTAG) && heroStateMachine.targetList.Contains(collision.transform))
//    {
//        if (collision.transform == heroStateMachine.currentTarget)
//        {
//            heroStateMachine.ResetTarget();
//        }
//        heroStateMachine.targetList.Remove(collision.transform);
//    }
//    else
//    {
//        Debug.LogError("Lỗi");
//    }
//}
