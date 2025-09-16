using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float KONCKBACK_FORCE = 25f;
    const float KNOCKBACK_DURATION = 0.2f;
    const float FRICTION = 5f;
    const float ANIMATION_SMOOTH_TIME = 0.03f;
    const float MOVE_SPEED = 5f;
    const float ATTACK_RANGE = 5f;

    #region 상태 객체
    public ChaseState ChaseState { get; private set; }
    public AttackState AttackState { get; private set; }
    public AutoChaseState AutoChaseState { get; private set; }
    public KnockbackState KnockbackState { get; private set; }

    private IState currentState;
    #endregion

    #region 회전 전략 객체
    private IRotate _iRotate;
    private Dictionary<Type, IRotate> _rotates;
    #endregion

    #region 애니메이터 값 해시 캐싱
    readonly int _hashIsAttack = Animator.StringToHash("IsAttack");
    readonly int _hashIsIdle = Animator.StringToHash("IsIdle");
    readonly int _hashSpeed = Animator.StringToHash("Speed");
    #endregion

    #region 오토 , 오토 변환 이벤트
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
    #endregion

    PlayerStat _playerStat;
    Rigidbody2D _rb;
    Animator _animator;
    EnemySpawner enemySpawner;


    public float AttackDamage => _playerStat.AttackDamage;

    Vector2 _currentVelocity;
    float _animatorSpeed;
    float _speedChangeVelocity;

    public EnemyAI CurrentTarget { get; set; }
    public Vector2 MoveInput { get; private set; }
    public Vector2 LastKnockbackDirection { get; set; }


    void Awake()
    {
        enemySpawner = EnemySpawner.Instance;
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerStat = GetComponent<PlayerStat>();

        InitStateModes();
        InitRotateModes();
    }

    void Start()
    {
        ChangeState(ChaseState);
        ChangeIRotate(typeof(ChaseMode));
    }

    void OnEnable()
    {
        JoystickController.Instance.OnJoystickMoved += SetMoveInput;
    }

    void OnDisable()
    {
        if (!JoystickController.Instance.IsApplicationQuit)
        {
            JoystickController.Instance.OnJoystickMoved -= SetMoveInput;
        }
    }

    void Update()
    {
        currentState?.Update();
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = _currentVelocity;
        _iRotate?.ChangeRotation();
        EnforceBounds();
        BrendTreatment();
    }

    #region 상태,전략,애니메이션 변경 요청 메서드
    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void ChangeIRotate(Type type)
    {
        if (_rotates.ContainsKey(type))
        {
            _iRotate = _rotates[type];
        }
    }

    public void ChangeAnimation(int animationHash)
    {
        _animator.SetTrigger(animationHash);
    }
    public void PlayAttackAnimation() => ChangeAnimation(_hashIsAttack);
    public void PlayIdleAnimation() => ChangeAnimation(_hashIsIdle);
    #endregion

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
        _currentVelocity = Vector2.Lerp(_currentVelocity, Vector2.zero, friction * Time.deltaTime);
    }
    public void Move()
    {
        _currentVelocity = MoveInput * MOVE_SPEED;
    }

    public void AutoMove()
    {
        if (CurrentTarget == null)
        {
            _currentVelocity = Vector2.zero;
            return;
        }
        Vector2 direction = (CurrentTarget.transform.position - transform.position).normalized;
        _currentVelocity = direction * MOVE_SPEED;
    }

    public void StopMove()
    {
        _currentVelocity = Vector2.zero;
    }

    public void PerformAttack()
    {
        if (CurrentTarget == null || !CurrentTarget.gameObject.activeSelf) return;
        CurrentTarget.TakeDamage(AttackDamage);
    }

    #endregion

    #region 타겟 탐색 로직
    public EnemyAI FindClosestEnemy()
    {
        EnemyAI closestEnemy = null;
        float minDistanceSqr = float.MaxValue;

        if (enemySpawner.livingEnemies.Count == 0) return null;

        foreach (EnemyAI enemy in enemySpawner.livingEnemies)
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

    void SetMoveInput(Vector2 v)
    {
        MoveInput = v;
    }

    void BrendTreatment()
    {
        float targetSpeed = _currentVelocity.magnitude;

        _animatorSpeed = Mathf.SmoothDamp(_animatorSpeed, targetSpeed, ref _speedChangeVelocity, ANIMATION_SMOOTH_TIME);

        _animator.SetFloat(_hashSpeed, _animatorSpeed);
    }

    void EnforceBounds()
    {
        if (DungeonBoundary.Instance == null) return;

        Bounds bounds = DungeonBoundary.Instance.Boundary;

        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(transform.position.x, bounds.min.x, bounds.max.x),
            Mathf.Clamp(transform.position.y, bounds.min.y, bounds.max.y)
        );

        transform.position = clampedPosition;
    }

    void InitStateModes()
    {
        ChaseState = new ChaseState(this, FindClosestEnemy, ATTACK_RANGE);
        AttackState = new AttackState(this);
        AutoChaseState = new AutoChaseState(this, FindClosestEnemy, ATTACK_RANGE);
        KnockbackState = new KnockbackState(this, KNOCKBACK_DURATION, KONCKBACK_FORCE, FRICTION);
    }

    void InitRotateModes()
    {
        _rotates = new Dictionary<Type, IRotate>()
        {
            { typeof(AttackMode), new AttackMode(this) },
            { typeof(ChaseMode), new ChaseMode(this) }
        };
    }
}