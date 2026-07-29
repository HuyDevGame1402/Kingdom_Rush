using UnityEngine;

public class Health : MonoBehaviour
{
    protected int health;
    protected int maxHealth;

    private int healthBuff;

    public void InitHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
    }
    public virtual void ApplyDamage(int damage, Transform attacker)
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

    public void ChangeMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void BuffHealthWithPercentMaxHealth(float percent)
    {
        healthBuff = (int)(percent * maxHealth);
        health += healthBuff;
        if(health > maxHealth) health = maxHealth;
    }
}
