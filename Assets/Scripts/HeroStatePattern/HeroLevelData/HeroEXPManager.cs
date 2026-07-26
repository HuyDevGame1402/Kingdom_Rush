using UnityEngine;

public class HeroEXPManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HeroDataInGame heroData;

    [Header("EXP Gain Formula Configs")]
    [Tooltip("Số Damage gây ra để nhận được 1 EXP (Ví dụ: 10 Damage = 1 EXP)")]
    [SerializeField] private int damagePerExpRate = 10;

    [Tooltip("Lượng EXP tối thiểu nhận được cho mỗi lần chém trúng địch")]
    [SerializeField] private int minExpPerDealDamage = 1;

    [Space(5)]
    [Tooltip("Số Damage nhận vào để nhận được 1 EXP (Ví dụ: 15 Damage = 1 EXP)")]
    [SerializeField] private int takenDamagePerExpRate = 15;

    [Tooltip("Lượng EXP tối thiểu nhận được cho mỗi lần bị địch đánh")]
    [SerializeField] private int minExpPerTakeDamage = 1;

    [Header("Enemy Kill / Assist Configs")]
    [Tooltip("Tỷ lệ % Max HP của quái chuyển thành EXP khi quái chết (0.1 = 10% Max HP)")]
    [Range(0.01f, 0.5f)]
    [SerializeField] private float enemyMaxHpToExpRate = 0.1f;

    [Tooltip("Lượng EXP tối thiểu nhận được khi hạ quái nhỏ")]
    [SerializeField] private int minKillExp = 5;

    [Tooltip("Lượng EXP tối đa nhận được khi hạ Boss/Mini-boss")]
    [SerializeField] private int maxKillExp = 200;

    [Tooltip("Bán kính xung quanh Hero để nhận EXP khi quái chết (Assist)")]
    [SerializeField] private float assistRadius = 6f;

    /// <summary>
    /// Gọi khi Hero gây sát thương cho quái
    /// </summary>
    public void OnDealDamage(int damageDealt)
    {
        int expGained = damageDealt / damagePerExpRate;
        expGained = Mathf.Max(minExpPerDealDamage, expGained);

        heroData.AddEXP(expGained);
    }

    /// <summary>
    /// Gọi khi Hero bị quái đánh (Sát thương gốc trước khi giảm trừ Armor)
    /// </summary>
    public void OnTakeDamage(int rawDamageTaken)
    {
        int expGained = rawDamageTaken / takenDamagePerExpRate;
        expGained = Mathf.Max(minExpPerTakeDamage, expGained);

        heroData.AddEXP(expGained);
    }

    /// <summary>
    /// Gọi khi một Enemy trong Map bị tiêu diệt
    /// </summary>
    public void OnEnemyKilled(int enemyMaxHP, Vector3 enemyPosition)
    {
        float distance = Vector3.Distance(transform.position, enemyPosition);

        if (distance <= assistRadius)
        {
            // Tính EXP = % Max HP của quái và kẹp trong khoảng [minKillExp, maxKillExp]
            int rawKillExp = Mathf.RoundToInt(enemyMaxHP * enemyMaxHpToExpRate);
            int killEXP = Mathf.Clamp(rawKillExp, minKillExp, maxKillExp);

            heroData.AddEXP(killEXP);
        }
    }
}