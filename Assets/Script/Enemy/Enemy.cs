using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

public class Enemy : Singleton<Enemy>, IDamageable, ISetEnemy
{
    private readonly int _spawn = Animator.StringToHash("IsSpawn");
    private readonly int _reSpawn = Animator.StringToHash("IsReSpawn");
    private readonly int _die = Animator.StringToHash("IsDie");

    float _maxHp;
    float _hp;
    public float Hp
    {
        get => _hp;
        set
        {
            _hp = value;

            if (_maxHp != 0)
            {
                HealthChanged?.Invoke(_hp / _maxHp);
            }
            if (_hp <= 0)
            {
                Die();
            }
        }
    }

    SpriteRenderer _spriteRenderer;
    Animator _animator;
    EnemyData _enemyData;

    readonly WaitForSecondsRealtime _delay = new WaitForSecondsRealtime(0.2f);

    public event Action<float> TakeDamaged;
    public event Action<float> HealthChanged;


    protected override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    IEnumerator Start()
    {
        int count = 0;

        while (true)
        {
            yield return _delay;

            if (_enemyData != null)
            {
                count = count + 1 >= _enemyData.Sprites.Length ? 0 : ++count;
                _spriteRenderer.sprite = _enemyData.Sprites[count];
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (_maxHp == 0 || _hp <= 0) return;

        Hp -= damage;
        TakeDamaged?.Invoke(damage);
    }

    public void SetEnemy(EnemyData enemyData)
    {
        if (enemyData == null) return;

        _enemyData = enemyData;
        _maxHp = enemyData.Hp;
        Hp = enemyData.Hp;
        Spawn();
    }

    public void Spawn()
    {
        _animator.SetTrigger(_spawn);
    }


    public void ReSpawn()
    {
        _animator.SetTrigger(_reSpawn);
    }

    [Preserve]
    void CompleteSpawn()
    {
        EnemySpawner.Instance.EnemySpawn();
    }

    [Preserve]
    void CompleteDeSpawn()
    {
        EnemySpawner.Instance.EnemyDeSpawn();
        Hp = _enemyData.Hp;
        ReSpawn();
    }

    void Die()
    {
        EnemySpawner.Instance.EnemyDie();
        _animator.SetTrigger(_die);
        PlayerData.Instance.Container.Gold += _enemyData.Gold;
    }
}
