using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    private EnemySpawner spawner; // 자신을 생성한 스포너

    public bool IsDead => health <= 0;

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