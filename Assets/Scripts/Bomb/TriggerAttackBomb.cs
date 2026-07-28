using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TriggerAttackBomb : MonoBehaviour
{
    private const string ENEMYTAG = "EnemyKingdomRush";

    [SerializeField] private List<Transform> enemyList = new List<Transform>();

    private float timeDes = 0.25f;
    private float timeDisGroundExpands = 3f;
    [SerializeField] private Collider2D collider2D;
    [SerializeField] private Transform groundExpands;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        collider2D.enabled = false;
        if(transform.parent.GetComponent<BombProjectile>() != null)
        {
            transform.parent.GetComponent<BombProjectile>().OnHitEvent += EnableCollider;
            //Debug.LogWarning("Trigger Attack Đã đăng ký sự kiện");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ENEMYTAG) && !enemyList.Contains(collision.transform))
        {
            Debug.LogWarning("Attack Enemy");
            if (collision.transform.TryGetComponent(out EnemyController enemyCtrl))
            {
                Debug.LogWarning("Take Damage Enemy");
                enemyCtrl.TakeDamage(transform.parent.GetComponent<BaseProjectile>()
                    .damage, transform.parent.GetComponent<BaseProjectile>().textSO, DamageType.Physical);
                enemyList.Add(collision.transform);
            }
        }
    }

    private IEnumerator CoroutineDisableCollider()
    {
        yield return new WaitForSeconds(timeDes);
        DisableCollider();
    }

    private IEnumerator CoroutineDisableGroundExpands()
    {
        yield return new WaitForSeconds(timeDisGroundExpands);
        groundExpands.gameObject.SetActive(false);
        transform.parent.GetComponent<BombProjectile>().HideGameObject();
    }

    public void EnableCollider()
    {
        collider2D.enabled = true;
        groundExpands.gameObject.SetActive(true);
        //Debug.LogWarning("Enable Collider 2D Bomb Attack");
        StartCoroutine(CoroutineDisableCollider());   
    }
    private void DisableCollider()
    {
        enemyList.Clear();
        collider2D.enabled = false;
        StartCoroutine(CoroutineDisableGroundExpands());
    }
}
