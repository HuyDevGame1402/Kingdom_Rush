using UnityEngine;
using System.Collections.Generic;
using System;

public enum ReinforceType
{
    ReinforceA,
    ReinforceB,
    ReinforceC,
    HeroBarrack,
}

[CreateAssetMenu(menuName = "ReinforceSO")]
public class ReinforceSO : ScriptableObject
{
    public List<ReinforceHeros> reinforceHeros;
}

[Serializable]
public class ReinforceHeros
{
    public int level;
    public List<PoolConfig> heros;
}