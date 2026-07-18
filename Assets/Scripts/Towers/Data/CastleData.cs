using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(
    fileName = "CastleData",
    menuName = "TD/Castle Data"
)]

public class CastleData : ScriptableObject
{
    [Header("Info")]
    public string castleName;

    [TextArea]
    public string description;

    [Header("Stats")]
    public int maxHealth;

    public int startGold;

    [Header("Visual")]
    public Sprite icon;

    public GameObject visualPrefab;

    [Header("Upgrade")]
    public int upgradeCost;

    public CastleData nextLevel;

    [Header("Damage")]
    public int minDamage;
    public int maxDamage;
    public float attackSpeed;
    public float attackRate;
    public float arrowArcHeight = 1.5f;

    [Header("Position Hero")]
    public List<Vector3> heroPositionList; 

    [Header("Animation Value")]
    public string animationTower;
    public int frameTowerStartIdle;
    public int frameTowerEndIdle;
    public int frameTowerStartAttack;
    public int frameTowerEndAttack;

    public string animationHero;
    public int frameHeroStartIdleUp;
    public int frameHeroEndIdleUp;
    public int frameHeroStartIdleDown;
    public int frameHeroEndIdleDown;
    public int frameHeroStartAttackUp;
    public int frameHeroEndAttackUp;
    public int frameHeroStartAttackDown;
    public int frameHeroEndAttackDown;

    public UnitDataSO heroDataSO;

    // Bomb Tower Hero
    public List<AnimationFrameRangeUpdate> heroBombAnim;
    public Vector3 positionSmoke;
    public AnimationFrameRangeUpdate smokeAnim;
}