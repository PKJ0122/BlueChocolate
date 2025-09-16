using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStat : SingletonMonoBase<PlayerStat>
{
    float _baseAttack;
    public float Attack { get; private set; }


    float _maxHealth = 100f;
    public float MaxHealth => _maxHealth;
    public float CurrentHealth { get; private set; }

    public bool IsDead => CurrentHealth <= 0;


    readonly List<StatModifier> _modifiers = new();


    protected override void Awake()
    {
        base.Awake();
        int myWeaponUpgrade = PlayerData.Instance.Container.WeaponUpgrade;
        _baseAttack = WeaponManager.Instance.GetWeaponData(myWeaponUpgrade).Attack;
        CalculateFinalStats();
        CurrentHealth = _maxHealth;
    }

    public void DecreaseHealth(float damage)
    {
        if (IsDead) return; // 이미 죽었다면 무시

        CurrentHealth -= damage;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }

        Debug.Log($"플레이어 체력: {CurrentHealth} / {MaxHealth}");
        // OnHealthChanged?.Invoke(); // HP UI 업데이트가 필요할 때 이벤트를 발생시킴
    }

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