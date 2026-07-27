using UnityEngine;
using System;
using System.Collections;

public class HealthHero : Health
{
    public bool isDead = false;
    public event Action OnDead;
    public event Action<int, int> OnHitDamage;
    public int lifeSpan;

    public override void ApplyDamage(int damage)
    {
        base.ApplyDamage(damage);
        OnHitDamage?.Invoke(health, maxHealth);
        if (IsDead())
        {
            OnDead?.Invoke();
        }
    }

    public void StartLife()
    {
        StartCoroutine(CoroutineLife());
    }

    protected virtual IEnumerator CoroutineLife()
    {
        yield return new WaitForSeconds(lifeSpan);
        ApplyDamage(maxHealth);
        if(ReinforceSpawnHero.Instance != null)
        {
            ReinforceSpawnHero.Instance.ReturnToPool(
                transform.GetComponent<BaseUnitStateMachine>().unitData.reinforceType,
                gameObject);
        }
    }
}
