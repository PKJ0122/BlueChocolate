using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    private EnemySpawner spawner; // 자신을 생성한 스포너

    public bool IsDead => health <= 0;

    [Header("이동 및 공격 설정")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 1.5f; // 이 거리 안으로 들어오면 공격
    [SerializeField] private float attackDamage = 10f;

    private Transform playerTransform;
    private Rigidbody2D rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // "Player" 태그를 가진 오브젝트를 찾아 타겟으로 설정
        GameObject playerObj = Player.Instance.gameObject;
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("씬에 'Player' 태그를 가진 오브젝트가 없습니다!");
            // 플레이어가 없으면 스크립트를 비활성화하여 오류 방지
            this.enabled = false;
        }
    }

    // 물리 기반 로직은 FixedUpdate에서 처리하는 것이 안정적입니다.
    void FixedUpdate()
    {
        if (playerTransform == null) return;

        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // 1. 공격 범위 안에 있는 경우
        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        // 2. 공격 범위 밖에 있는 경우
        else
        {
            // 플레이어를 향하는 방향 계산
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            // 계산된 방향으로 속도 설정
            rb.linearVelocity = direction * moveSpeed;
        }
    }

    private void Attack()
    {
        PlayerDamageHandler damageHandler = playerTransform.GetComponent<PlayerDamageHandler>();
        if (damageHandler != null)
        {
            Vector2 knockbackDirection = (playerTransform.position - transform.position).normalized;
            // TakeDamage 메서드를 호출하는 것은 동일
            damageHandler.TakeDamage(attackDamage, knockbackDirection);
        }
    }

    public void Setup(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        health -= amount;
        Debug.Log($"{gameObject.name}이(가) 피해를 입었습니다. 남은 체력: {health}");

        if (IsDead)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name}이(가) 죽었습니다.");
        // 죽었을 때 스포너에게 알림
        spawner.OnEnemyKilled(this);
        // 오브젝트 파괴
        Destroy(gameObject, 0.1f);
    }

    private void OnDisable()
    {
        spawner.OnEnemyKilled(this);
    }


}