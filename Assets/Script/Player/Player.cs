using UnityEngine;

public class Player : Singleton<Player>
{
    private readonly int _attack = Animator.StringToHash("IsAttack");
    private readonly int _idle = Animator.StringToHash("IsIdle");

    IDamageable _target;

    Animator _animator;


    protected override void Awake()
    {
        base.Awake();
        _target = Enemy.Instance;

        _animator = GetComponent<Animator>();

        EnemySpawner.Instance.OnSpawn += () =>
        {
            _animator.SetTrigger(_attack);
        };
        EnemySpawner.Instance.OnDie += () =>
        {
            _animator.SetTrigger(_idle);
        };
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
