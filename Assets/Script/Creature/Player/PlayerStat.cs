using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStat : MonoBehaviour, IStat
{
    public event Action<float> OnTakeDamage;
    public event Action OnDied;
    public bool IsDead => CurrentHealth <= 0;

    float _baseAttackDamage;
    public float AttackDamage { get; private set; }


    float _maxHealth = 100f;
    public float MaxHealth => _maxHealth;
    public float CurrentHealth { get; private set; }




    readonly List<StatModifier> _modifiers = new();


    void Awake()
    {
        int myWeaponUpgrade = PlayerData.Instance.Container.WeaponUpgrade;
        _baseAttackDamage = WeaponManager.Instance.GetWeaponData(myWeaponUpgrade).Attack;
        CalculateFinalStats();
        CurrentHealth = _maxHealth;
    }

    public void DecreaseHealth(float damage)
    {
        if (IsDead) return;

        CurrentHealth -= damage;
        if (CurrentHealth < 0) CurrentHealth = 0;

        OnTakeDamage?.Invoke(damage);
        UIManager.Instance.Get<DamageUI>().GetDamageOfPlayer(damage, transform.position);

        if (IsDead)
        {
            OnDied?.Invoke();
            // 몬스터는 보통 자기 자신만 죽으면 되므로 static 이벤트는 필요 없음
        }
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
        AttackDamage = _baseAttackDamage;

        foreach (var mod in _modifiers.Where(m => m.ModType == StatModType.Flat))
        {
            AttackDamage += mod.Value;
        }

        foreach (var mod in _modifiers.Where(m => m.ModType == StatModType.PercentMult))
        {
            AttackDamage *= mod.Value;
        }
    }
}