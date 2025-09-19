using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    bool at;

    public float speed = 20f;
    public float lifetime = 3f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        at = false;
    }

    // 다른 트리거 콜라이더와 충돌했을 때
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyAI enemyAI) && !at)
        {
            at = true;
            enemyAI.TakeDamage(10);
            Deactivate();
        }
    }

    public void Set(Vector3 direction)
    {
        // 발사 방향으로 속도 설정
        rb.linearVelocity = direction * speed;
        // lifetime 이후에 자동으로 풀에 반환되도록 예약
        Invoke(nameof(Deactivate), lifetime);
    }

    // 오브젝트를 파괴하는 대신 비활성화하는 함수
    void Deactivate()
    {
        // 예약된 Invoke가 있다면 취소 (중복 실행 방지)
        CancelInvoke();
        // Rigidbody의 속도를 0으로 만들어 관성이 남지 않게 함
        rb.linearVelocity = Vector3.zero;

        gameObject.SetActive(false);
    }
}
