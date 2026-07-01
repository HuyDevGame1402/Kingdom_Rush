using UnityEngine;

public class Health : MonoBehaviour
{
    private int health;
    private int maxHealth;

    public void InitHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
    }
    public void ApplyDamage(int damage)
    {
        health -= damage;
    }
    public bool IsDead()
    {
        return health <= 0;
    }
}
