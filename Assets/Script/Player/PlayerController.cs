using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    public EnemySpawner enemySpawner;

    [Header("Stats")]
    public float moveSpeed = 5f;
    public float attackRange = 1f;
    public float attackDamage = 25f;
    public float attackCooldown = 1f;

    // --- 상태 객체들을 미리 생성 (GC 최적화) ---
    public ChaseState ChaseState { get; private set; }
    public AttackState AttackState { get; private set; }
    public AutoChaseState AutoChaseState { get; private set; }
    // ------------------------------------------

    private IState currentState;
    public Enemy CurrentTarget { get; set; }

    // --- 회전 상태 객체 ---
    private IRotate _iRotate;
    private Dictionary<Type, IRotate> rotates;

    // --- 컴포넌트 및 입력 변수 ---
    private Rigidbody2D rb;
    private Animator animator;
    public Vector2 MoveInput { get; private set; } // 다른 클래스에서 읽을 수 있도록 public get으로 변경

    // --- 애니메이터 해시 캐싱 (최적화) ---
    private readonly int hashIsAttack = Animator.StringToHash("IsAttack");
    private readonly int hashIsIdle = Animator.StringToHash("IsIdle");

    private bool _auto;
    public bool Auto
    {
        get => _auto;
        set
        {
            if (_auto == value) return; // 같은 값이면 무시
            _auto = value;

            // Auto 상태가 변경될 때 직접 상태 전환을 트리거
            if (_auto)
            {
                // 현재 수동 추격 중이었다면 자동으로 전환
                if (currentState == ChaseState)
                {
                    ChangeState(AutoChaseState);
                }
            }
            else
            {
                // 현재 자동 추격 중이었다면 수동으로 전환
                if (currentState == AutoChaseState)
                {
                    ChangeState(ChaseState);
                }
            }
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // 모든 상태 객체를 게임 시작 시 한 번만 생성
        ChaseState = new ChaseState(this, FindClosestEnemy);
        AttackState = new AttackState(this); // 더 이상 생성자에 target을 넘기지 않음
        AutoChaseState = new AutoChaseState(this, FindClosestEnemy);

        // 회전 모드 초기화
        InitRotateModes();
    }

    void Start()
    {
        if (enemySpawner == null)
        {
            Debug.LogError("EnemySpawner가 할당되지 않았습니다!");
            return;
        }

        // 조이스틱 이벤트 구독
        JoystickController.Instance.OnJoystickMoved += v => MoveInput = v;

        // 시작 상태 및 회전 모드 설정
        ChangeState(ChaseState);
        ChangeIRotate(typeof(ChaseMode));
    }

    void FixedUpdate()
    {
        // 상태와 회전 로직은 물리 업데이트에서 처리
        currentState?.Update();
        _iRotate?.ChangeRotation();
    }

    // 상태 변경 메서드 (이제 new 키워드를 사용하지 않음)
    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void ChangeIRotate(Type type)
    {
        if (rotates.ContainsKey(type))
        {
            _iRotate = rotates[type];
        }
    }

    // 미리 캐싱된 해시를 사용하여 애니메이션 변경 (최적화)
    public void ChangeAnimation(int animationHash)
    {
        animator.SetTrigger(animationHash);
    }
    public void PlayAttackAnimation() => ChangeAnimation(hashIsAttack);
    public void PlayIdleAnimation() => ChangeAnimation(hashIsIdle);


    #region 행동 메서드

    public void Move()
    {
        rb.linearVelocity = MoveInput * moveSpeed;
    }

    public void AutoMove()
    {
        if (CurrentTarget == null) return;
        Vector2 direction = (CurrentTarget.transform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    public void StopMove()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void PerformAttack()
    {
        if (CurrentTarget == null || CurrentTarget.IsDead) return;
        CurrentTarget.TakeDamage(attackDamage);
    }

    #endregion

    #region 타겟 탐색 로직
    public Enemy FindClosestEnemy()
    {
        Enemy closestEnemy = null;
        float minDistanceSqr = float.MaxValue; // 제곱 거리로 비교 (Vector3.Distance보다 빠름)

        if (enemySpawner.livingEnemies.Count == 0) return null;

        foreach (Enemy enemy in enemySpawner.livingEnemies)
        {
            float distanceSqr = (enemy.transform.position - transform.position).sqrMagnitude;
            if (distanceSqr < minDistanceSqr)
            {
                minDistanceSqr = distanceSqr;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
    #endregion

    private void InitRotateModes()
    {
        rotates = new Dictionary<Type, IRotate>()
        {
            { typeof(AttackMode), new AttackMode(this) },
            { typeof(ChaseMode), new ChaseMode(this) }
        };
    }
}