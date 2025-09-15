using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    public EnemySpawner enemySpawner; // 인스펙터에서 할당

    [Header("Stats")]
    public float moveSpeed = 5f;
    public float attackRange = 1f;
    public float attackDamage = 25f;
    public float attackCooldown = 1f;
    private Vector2 moveInput;

    bool _auto;
    public bool Auto
    {
        get => _auto;
        set => _auto = value;
    }

    Rigidbody2D rb;
    Animator animator;

    // 상태 클래스들이 플레이어의 현재 타겟에 접근할 수 있도록 프로퍼티로 제공
    public Enemy CurrentTarget { get; set; }

    private IState currentState;
    private IRotate _iRotate;
    Dictionary<Type, IRotate> rotates;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (enemySpawner == null)
        {
            Debug.LogError("EnemySpawner가 할당되지 않았습니다!");
            return;
        }
        // 시작 상태를 추격 상태로 설정
        ChangeState(new ChaseState(this, FindClosestEnemy));

        JoystickController.Instance.OnJoystickMoved += v =>
        {
            moveInput = v;
        };
    }

    void Init()
    {
        rotates = new Dictionary<Type, IRotate>()
        {
            { typeof(AttackMode),new AttackMode(this) },
            { typeof(ChaseMode),new ChaseMode(this)}
        };
    }

    void FixedUpdate()
    {
        // 현재 상태의 Update 로직을 매 프레임 실행
        currentState?.Update();
        _iRotate?.ChangeRotation();
    }

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void ChangeIRotate(Type type)
    {
        _iRotate = rotates[type];
    }

    public void ChangeAnimation(string name)
    {
        int animaHash = Animator.StringToHash(name);
        animator.SetTrigger(animaHash);
    }

    #region 행동 메서드 (상태 클래스에서 호출)

    public void AutoMove()
    {
        if (CurrentTarget == null) return;

        Vector3 direction = (CurrentTarget.transform.position - transform.position).normalized;
        transform.position += moveSpeed * Time.deltaTime * direction;
    }

    public void Move()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void PerformAttack()
    {
        if (CurrentTarget == null || CurrentTarget.IsDead) return;

        CurrentTarget.TakeDamage(attackDamage);
    }

    #endregion

    #region 타겟 탐색 로직 (Func로 전달될 메서드)

    // 이 메서드를 Func<Enemy> 델리게이트로 ChaseState에 전달
    public Enemy FindClosestEnemy()
    {
        Enemy closestEnemy = null;
        float minDistance = float.MaxValue;

        if (enemySpawner.livingEnemies.Count == 0)
        {
            return null;
        }

        foreach (Enemy enemy in enemySpawner.livingEnemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }

    #endregion
}