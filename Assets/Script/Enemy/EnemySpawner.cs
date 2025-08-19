using System;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    EnemyDatas enemyDatas;

    public event Action OnSpawn;
    public event Action OnDie;
    public event Action OnDeSpawn;


    protected override void Awake()
    {
        base.Awake();
        enemyDatas = Resources.Load<EnemyDatas>("EnemyDatas");
    }

    void Start()
    {
        int selectSlimeNum = PlayerData.Instance.Container.SelectSlimeNum;
        EnemyData enemyData = enemyDatas.enemyDatas[selectSlimeNum];
        SetEnemyInfo(enemyData);
    }

    public void SetEnemyInfo(EnemyData enemyData)
    {
        Enemy.Instance.SetEnemy(enemyData);
    }

    public void EnemySpawn()
    {
        OnSpawn?.Invoke();
    }

    public void EnemyDie()
    {
        OnDie?.Invoke();
    }

    public void EnemyDeSpawn()
    {
        OnDeSpawn?.Invoke();
    }
}