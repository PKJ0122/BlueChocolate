using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : SingletonMonoBase<EnemySpawner>
{
    public GameObject enemyPrefab;
    public List<EnemyAI> livingEnemies = new();

    public event Action<EnemyAI> OnEnemySpawn;


    //void Start()
    //{
    //    // 테스트를 위해 5마리 소환
    //    for (int i = 0; i < 10; i++)
    //    {
    //        Vector3 spawnPos = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
    //        GameObject enemyObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    //        enemyObj.name = $"Enemy_{i + 1}";
    //        EnemyAI enemy = enemyObj.GetComponent<EnemyAI>();

    //        enemy.Setup(this); // 스포너 정보 넘겨주기
    //        livingEnemies.Add(enemy);
    //        OnEnemySpawn?.Invoke(enemy);
    //    }
    //}

    // 적이 죽었을 때 호출될 메서드
    public void OnEnemyKilled(EnemyAI enemy)
    {
        if (livingEnemies.Contains(enemy))
        {
            livingEnemies.Remove(enemy);
        }
    }
}