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
    [SerializeField] private float knockbackDuration = 0.2f; // 넉백 상태 지속 시간

    // --- 상태 객체들을 미리 생성 (GC 최적화) ---
    public ChaseState ChaseState { get; private set; }
    public AttackState AttackState { get; private set; }
    public AutoChaseState AutoChaseState { get; private set; }

    public KnockbackState KnockbackState { get; private set; }
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
    private readonly int hashSpeed = Animator.StringToHash("Speed"); // Speed 파라미터도 캐싱

    private Vector2 _currentVelocity;



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

    private float animationSmoothTime = 0.03f;
    // --- 애니메이션 스무딩을 위한 변수 ---
    private float _animatorSpeed; // 애니메이터에 실제로 전달될, 부드럽게 처리된 속도 값
    private float _speedChangeVelocity; // SmoothDamp 내부 계산에 사용될 참조 변수 (신경 쓸 필요 없음)
    public Vector2 lastKnockbackDirection;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // 모든 상태 객체를 게임 시작 시 한 번만 생성
        ChaseState = new ChaseState(this, FindClosestEnemy);
        AttackState = new AttackState(this); // 더 이상 생성자에 target을 넘기지 않음
        AutoChaseState = new AutoChaseState(this, FindClosestEnemy);
        KnockbackState = new KnockbackState(this, knockbackDuration);

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

    void Update()
    {
        currentState?.Update();
    }

    void FixedUpdate()
    {
        // Update에서 계산된 속도를 Rigidbody에 적용
        rb.linearVelocity = _currentVelocity;

        // 회전 로직 실행
        _iRotate?.ChangeRotation();

        // 경계 제한
        EnforceBounds();
        // 블랜드 트리 로직
        BrendTreatment();
    }

    void BrendTreatment()
    {
        float targetSpeed = _currentVelocity.magnitude;

        // _animatorSpeed 값을 targetSpeed 값으로 animationSmoothTime에 걸쳐 부드럽게 변경
        _animatorSpeed = Mathf.SmoothDamp(_animatorSpeed, targetSpeed, ref _speedChangeVelocity, animationSmoothTime);

        // 최종적으로 부드럽게 처리된 _animatorSpeed 값을 애니메이터에 전달
        animator.SetFloat(hashSpeed, _animatorSpeed);
    }

    private void EnforceBounds()
    {
        if (DungeonBoundary.Instance == null) return;

        Bounds bounds = DungeonBoundary.Instance.Boundary;

        // 현재 위치를 경계의 min, max 값 사이로 제한(Clamp)
        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(transform.position.x, bounds.min.x, bounds.max.x),
            Mathf.Clamp(transform.position.y, bounds.min.y, bounds.max.y)
        );

        // 제한된 위치로 플레이어 위치 설정
        transform.position = clampedPosition;
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
    /// <summary>
    /// 넉백을 위해 _currentVelocity를 직접 설정하는 새로운 메서드
    /// </summary>
    public void ApplyKnockbackVelocity(Vector2 knockbackDirection, float knockbackForce)
    {
        _currentVelocity = knockbackDirection * knockbackForce;
    }

    /// <summary>
    /// 현재 속도를 점진적으로 줄이는 메서드 (넉백 상태에서 사용)
    /// </summary>
    public void DecayVelocity(float friction)
    {
        // Lerp를 사용하여 현재 속도를 0으로 부드럽게 감속시킵니다.
        _currentVelocity = Vector2.Lerp(_currentVelocity, Vector2.zero, friction * Time.deltaTime);
    }
    public void Move()
    {
        _currentVelocity = MoveInput * moveSpeed;
    }

    public void AutoMove()
    {
        if (CurrentTarget == null)
        {
            _currentVelocity = Vector2.zero;
            return;
        }
        Vector2 direction = (CurrentTarget.transform.position - transform.position).normalized;
        _currentVelocity = direction * moveSpeed;
    }

    public void StopMove()
    {
        _currentVelocity = Vector2.zero;
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