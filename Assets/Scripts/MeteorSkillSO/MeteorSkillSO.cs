using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MeteorSkillData", menuName = "Game/Skills/Meteor Skill")]
public class MeteorSkillSO : ScriptableObject
{
    public MeteorLevelData[] levels;

    public MeteorLevelData GetLevelData(int level)
    {
        level = Mathf.Clamp(level, 0, levels.Length - 1);
        return levels[level];
    }
}

[Serializable]
public class MeteorLevelData
{
    [Header("Meteor")]
    [Min(1)]
    public int numberOfMeteors;

    public Vector2 meteorDamage;

    [Min(0)]
    public float cooldown;

    [Header("Scorched Earth")]
    public bool createScorchedEarth;

    [Min(0)]
    public float scorchedEarthDuration;

    public Vector2 scorchedEarthDamage;
}