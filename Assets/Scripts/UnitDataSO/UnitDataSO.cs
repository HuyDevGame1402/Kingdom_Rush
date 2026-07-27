using UnityEngine;

// Định nghĩa loại đơn vị để dễ phân loại xử lý logic trong Code nếu cần
public enum UnitType
{
    RegularSoldier, // Lính Barracks bình thường
    Hero,           // Tướng (Có cấp độ, kỹ năng riêng)
    Elemental,      // Đơn vị triệu hồi đặc biệt (như Người đá)
    Enemy           // Quân địch / Quái vật
}

[CreateAssetMenu(fileName = "NewUnitData", menuName = "KingdomRush/Unit Data")]
public class UnitDataSO : ScriptableObject
{
    [Header("Identity")]
    public string unitName;
    public ReinforceType reinforceType;
    public UnitType unitType;
    [TextArea(2, 5)] public string description;

    [Header("Base Stats")]
    public float maxHealth = 50f;
    public float moveSpeed = 2f;
    [Tooltip("Giáp vật lý (tính theo % giảm sát thương hoặc trừ trực tiếp tùy logic game của bạn)")]
    public float armor = 0f;
    [Tooltip("Kháng phép (%)")]
    public float magicResistance = 0f;

    [Header("Combat Stats")]
    public float minDamage;
    public float maxDamage;
    public float attackRange = 0.5f;
    public float attackCooldown = 1.5f;
    public float attackCooldownAdd;

    [Header("Animation Settings")]
    public UnitAnimationConfig animations;

    public Vector3 localScaleLeft;
    public Vector3 localScaleRight;

    public float heroScale;
    public float timeToResurrect;

    public int maxAttacker = 1;

    public int bounty;
    public int livesTaken;

    public bool isHasLevelHero;
    public bool isLongRangeAttack;

    public Sprite characterGUI;

    // --- TIỆN ÍCH LẤY SÁT THƯƠNG NGẪU NHIÊN ---
    public float GetRandomDamage()
    {
        return Random.Range(minDamage, maxDamage);
    }
}