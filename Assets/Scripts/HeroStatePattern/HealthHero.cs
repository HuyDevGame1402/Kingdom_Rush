using UnityEngine;
using System;

public class HealthHero : Health
{
    public bool isDead = false;
    public event Action OnDead;
    public override void ApplyDamage(int damage)
    {
        base.ApplyDamage(damage);
        if (IsDead())
        {
            OnDead?.Invoke();
        }
    }
}
