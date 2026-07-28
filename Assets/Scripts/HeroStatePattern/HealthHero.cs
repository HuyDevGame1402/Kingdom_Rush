using UnityEngine;
using System;
using System.Collections;

public class HealthHero : Health
{
    public bool isDead = false;
    public event Action OnDead;
    public event Action<int, int, Transform> OnHitDamage;
    public event Action<int, Transform> OnHitDamageSheldEvent;
    public int lifeSpan;

    public override void ApplyDamage(int damage, Transform attacker)
    {
        base.ApplyDamage(damage, attacker);
        OnHitDamage?.Invoke(health, maxHealth, attacker);
        OnHitDamageSheldEvent?.Invoke(damage, attacker);
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
        ApplyDamage(maxHealth, transform);
        if(ReinforceSpawnHero.Instance != null)
        {
            ReinforceSpawnHero.Instance.ReturnToPool(
                transform.GetComponent<BaseUnitStateMachine>().unitData.reinforceType,
                gameObject);
        }
    }
}
