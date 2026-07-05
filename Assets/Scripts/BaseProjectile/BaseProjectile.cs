using UnityEngine;


public abstract class BaseProjectile : MonoBehaviour
{
    // ─── Trạng thái bay ────────────────────────────────────────────────────────
    protected Transform targetEnemy;
    protected Vector3 startPosition;
    protected float speed;
    public bool isFlying = false;
    public int damage;
    public bool continueIfTargetDies = true;

    public TextSO textSO;

    public virtual void Launch(Transform enemy, float projectileSpeed,
        int damage = 1)
    {
        if (enemy == null)
        {
            gameObject.SetActive(false);
            return;
        }

        targetEnemy = enemy;
        startPosition = transform.position;
        speed = projectileSpeed;
        isFlying = true;
        gameObject.SetActive(true);
        this.damage = damage;
        OnLaunched();
    }

    protected virtual void OnLaunched() { }

    protected abstract void MoveLogic();

    protected virtual void OnTargetLost()
    {
        isFlying = false;
        gameObject.SetActive(false);
    }

    protected virtual void OnHitTarget()
    {
        isFlying = false;
        Debug.Log($"<color=yellow>[Projectile]</color> Trúng: {targetEnemy?.name}");
        // TODO: gọi DealDamage(), SpawnHitFX(), v.v.
        gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (!isFlying) return;

        if(continueIfTargetDies == false)
        {
            if (targetEnemy == null || !targetEnemy.gameObject.activeInHierarchy)
            {
                OnTargetLost();
                return;
            }
        }

        MoveLogic();
    }
}