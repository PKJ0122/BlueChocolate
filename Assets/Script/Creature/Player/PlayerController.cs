using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float KONCKBACK_FORCE = 25f;
    const float KNOCKBACK_DURATION = 0.2f;
    const float FRICTION = 5f;
    const float MOVE_SPEED = 5f;

    #region 상태 객체
    public MoveState MoveState;
    public KnockbackState KnockbackState { get; private set; }

    private IState currentState;
    #endregion

    PlayerStat _playerStat;
    Rigidbody2D _rb;
    EnemySpawner enemySpawner;


    public float AttackDamage => _playerStat.AttackDamage;

    Vector2 _currentVelocity;

    public EnemyAI CurrentTarget { get; set; }
    public Vector2 MoveInput { get; private set; }
    public Vector2 LastKnockbackDirection { get; set; }

    public bool AttackEnd { get; set; }


    void Awake()
    {
        enemySpawner = EnemySpawner.Instance;
        _rb = GetComponent<Rigidbody2D>();
        _playerStat = GetComponent<PlayerStat>();

        InitStateModes();
    }

    void OnEnable()
    {
        ChangeState(MoveState);
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
        EnforceBounds();
        ChangeRotate();
    }

    #region 상태,전략 변경 요청 메서드
    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void ChangeRotate()
    {
        float inputX = MoveInput.x;

        if (inputX > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (inputX < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
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

    public void StopMove()
    {
        _currentVelocity = Vector2.zero;
    }
    #endregion

    void SetMoveInput(Vector2 v)
    {
        MoveInput = v;
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
        MoveState = new MoveState(this);
        KnockbackState = new KnockbackState(this, KNOCKBACK_DURATION, KONCKBACK_FORCE, FRICTION);
    }
}