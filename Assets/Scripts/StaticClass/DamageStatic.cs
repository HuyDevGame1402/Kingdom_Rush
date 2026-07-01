using UnityEngine;

public static class DamageStatic
{
    public static int GetDamageBase(int minDamage, int maxDamage)
    {
        return Random.Range(minDamage, maxDamage + 1);
    }
}
