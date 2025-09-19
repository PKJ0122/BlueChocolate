using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(MonsterStat))]
public class EnemyAI : MonoBehaviour
{
    const float HIT_EFFECT_DURATION = 0.3f;

    private SpawnManager spawner; // 자신을 생성한 스포너

    private Transform playerTransform;
    private Rigidbody2D rb;

    private float _moveSpeed;


    public MonsterStat MonsterStat { get; private set; }


    Material _material;
    int _hitEffectHash = Shader.PropertyToID("_FlashAmount");


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        MonsterStat = GetComponent<MonsterStat>();
        float baseSpeed = MonsterStat.MoveSpeed;
        _moveSpeed = Random.Range(baseSpeed * 0.5f, baseSpeed * 2.3f);
        _material = GetComponent<SpriteRenderer>().material;
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

    private void OnEnable()
    {
        MonsterStat.OnDied += Die;
        MonsterStat.OnTakeDamage += PlayHitEffect;
    }
    private void OnDisable()
    {
        MonsterStat.OnDied -= Die;
        MonsterStat.OnTakeDamage -= PlayHitEffect;
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;

        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // 1. 공격 범위 안에 있는 경우
        if (distanceToPlayer <= MonsterStat.AttackRange)
        {
            Attack();
        }
        // 2. 공격 범위 밖에 있는 경우
        else
        {
            // 플레이어를 향하는 방향 계산
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            // 계산된 방향으로 속도 설정
            rb.linearVelocity = direction * _moveSpeed;
        }

        ChangeIRotate();
    }

    public void Setup(SpawnManager spawner)
    {
        this.spawner = spawner;
    }

    public void TakeDamage(float damage)
    {
        MonsterStat.TakeDamage(damage);
    }

    public void PlayHitEffect(float damage)
    {
        _material.DOKill();
        _material.DOFloat(1f, _hitEffectHash, HIT_EFFECT_DURATION / 2).SetLoops(2, LoopType.Yoyo);
    }

    void Attack()
    {
        PlayerDamageHandler damageHandler = playerTransform.GetComponent<PlayerDamageHandler>();
        if (damageHandler != null)
        {
            Vector2 knockbackDirection = (playerTransform.position - transform.position).normalized;
            // TakeDamage 메서드를 호출하는 것은 동일
            damageHandler.TakeDamage(MonsterStat.AttackDamage, knockbackDirection);
        }
    }

    void ChangeIRotate()
    {
        float playerX = playerTransform.transform.position.x;
        float myX = transform.position.x;

        if (playerX > myX)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (playerX < myX)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    void Die()
    {
        _material.DOKill();
        gameObject.GetComponent<PoolObject>().Release();
        SpawnManager.Instance.activeEnemies.Remove(this);
    }
}
