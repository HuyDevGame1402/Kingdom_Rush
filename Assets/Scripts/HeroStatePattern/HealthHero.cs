using UnityEngine;
using System;
using System.Collections;

public class HealthHero : Health
{
    public bool isDead = false;
    public event Action OnDead;

    public int lifeSpan;

    public override void ApplyDamage(int damage)
    {
        base.ApplyDamage(damage);
        if (IsDead())
        {
            OnDead?.Invoke();
        }
    }

    public void StartLife()
    {
        StartCoroutine(CoroutineLife());
    }

    private IEnumerator CoroutineLife()
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
