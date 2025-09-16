using System;
using UnityEngine;

public class MonsterStat : MonoBehaviour, IStat
{
    public event Action<float> OnTakeDamage;
    public event Action OnDied;
    public bool IsDead => CurrentHealth <= 0;

    public float CurrentHealth { get; private set; }

    [SerializeField] private EnemyStatData statData;
    public float MaxHealth => statData.maxHealth;
    public float AttackDamage => statData.attackDamage;
    public float MoveSpeed => statData.moveSpeed;
    public float AttackRange => statData.attackRange;


    void OnEnable()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;
        CurrentHealth -= damage;
        if (CurrentHealth < 0) CurrentHealth = 0;

        OnTakeDamage?.Invoke(damage);
        UIManager.Instance.Get<DamageUI>().GetDamageOfEnemy(damage, transform.position);

        if (IsDead)
        {
            OnDied?.Invoke();
        }
    }
}
