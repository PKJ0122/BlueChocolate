using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public List<Enemy> livingEnemies = new List<Enemy>();

    void Start()
    {
        // 테스트를 위해 5마리 소환
        for (int i = 0; i < 1; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
            GameObject enemyObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemyObj.name = $"Enemy_{i + 1}";
            Enemy enemy = enemyObj.GetComponent<Enemy>();

            enemy.Setup(this); // 스포너 정보 넘겨주기
            livingEnemies.Add(enemy);
        }
    }

    // 적이 죽었을 때 호출될 메서드
    public void OnEnemyKilled(Enemy enemy)
    {
        if (livingEnemies.Contains(enemy))
        {
            livingEnemies.Remove(enemy);
        }
    }
}