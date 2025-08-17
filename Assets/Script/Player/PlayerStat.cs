using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStat : Singleton<PlayerStat>
{
    public float Attack { get; private set; }
    float _baseAttack;

    readonly List<StatModifier> _modifiers = new();


    public void AddModifier(StatModifier mod)
    {
        _modifiers.Add(mod);
        CalculateFinalStats();
    }

    public void RemoveModifier(StatModifier mod)
    {
        _modifiers.Remove(mod);
        CalculateFinalStats();
    }

    private void CalculateFinalStats()
    {
        Attack = _baseAttack;

        foreach (var mod in _modifiers.Where(m => m.ModType == StatModType.Flat))
        {
            Attack += mod.Value;
        }

        foreach (var mod in _modifiers.Where(m => m.ModType == StatModType.PercentMult))
        {
            Attack *= mod.Value;
        }
    }
}