using UnityEngine;

public class Player : Singleton<Player>
{
    IDamageable _target;


    void Awake()
    {
        _target = Enemy.Instance;
    }

    public void Attack()
    {
        _target?.TakeDamage(PlayerStat.Instance.Attack);
    }

    public void SetDamageableTarget(IDamageable target)
    {
        _target = target;
    }
}
