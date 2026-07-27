using UnityEngine;

public class Health : MonoBehaviour
{
    protected int health;
    protected int maxHealth;

    public void InitHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
    }
    public virtual void ApplyDamage(int damage)
    {
        health -= damage;
    }
    public bool IsDead()
    {
        return health <= 0;
    }
    public void ResetHealth()
    {
        health = maxHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
