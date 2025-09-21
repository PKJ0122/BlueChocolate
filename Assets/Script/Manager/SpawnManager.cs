// 한 웨이브에 등장할 몬스터의 종류와 등장 확률(가중치)을 정의합니다.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnemyToSpawn
{
    public GameObject enemyPrefab; // 몬스터 프리팹
    [Range(1, 100)]
    public int weight = 1; // 등장 확률 가중치 (높을수록 잘 나옴)
}

// 각 웨이브의 설정을 정의합니다.
[System.Serializable]
public class Wave
{
    public string waveName; // 웨이브 이름 (예: Wave 1, Elite Wave)
    public float duration; // 웨이브 지속 시간 (초)

    [Tooltip("웨이브 시작 시 몬스터 생성 간격")]
    public float startSpawnInterval = 1.0f;
    [Tooltip("웨이브 종료 시 몬스터 생성 간격 (가장 빨라지는 속도)")]
    public float endSpawnInterval = 0.2f;

    public List<EnemyToSpawn> enemies; // 이 웨이브에 등장할 몬스터 리스트
}

public class SpawnManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static SpawnManager Instance;

    [Header("Wave Settings")]
    public Wave[] waves; // 게임의 모든 웨이브 설정

    [Header("Spawn Settings")]
    public Transform playerTransform; // 플레이어의 Transform
    public float spawnRadius = 20f;   // 플레이어로부터 스폰될 거리

    // 내부 변수
    private int currentWaveIndex = -1;
    private float waveTimer;
    private bool isSpawning = false;

    // 현재 씬에 활성화된 몬스터 리스트
    [HideInInspector]
    public List<EnemyAI> activeEnemies = new();

    public PoolObject _p;
    public Button _zz;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ObjectPool.Instance.CreatePool("z", _p, 20);
    }

    void Update()
    {
        // TODO: 여기서 게임 상태를 체크하여 다음 웨이브를 시작할 수 있습니다.
        // 예를 들어, 상점 시스템이 있다면 상점을 닫을 때 StartNextWave()를 호출합니다.
        if (Input.GetKeyDown(KeyCode.N) && !isSpawning) // 테스트용: N키로 다음 웨이브 시작
        {
            StartNextWave();
        }

        // 웨이브 타이머 UI 업데이트 등
        if (isSpawning)
        {
            waveTimer -= Time.deltaTime;
            // TODO: waveTimer 값을 UI에 표시
        }
    }

    public void StartNextWave()
    {
        currentWaveIndex++;
        if (currentWaveIndex < waves.Length)
        {
            StartCoroutine(SpawnWaveCoroutine());
        }
        else
        {
            Debug.Log("모든 웨이브 클리어! 게임 승리!");
            // TODO: 게임 승리 로직
        }
    }

    IEnumerator SpawnWaveCoroutine()
    {
        isSpawning = true;
        Wave currentWave = waves[currentWaveIndex];
        waveTimer = currentWave.duration;
        float elapsedTime = 0f;
        float currentSpawnInterval = 0f;

        Debug.Log(currentWave.waveName + " 시작!");

        // 웨이브 지속 시간 동안 반복
        while (elapsedTime < currentWave.duration)
        {
            elapsedTime += currentSpawnInterval;

            // 웨이브 진행도 (0.0 ~ 1.0)
            float waveProgress = elapsedTime / currentWave.duration;

            // 진행도에 따라 스폰 간격을 동적으로 계산 (점점 짧아짐)
            currentSpawnInterval = Mathf.Lerp(currentWave.startSpawnInterval, currentWave.endSpawnInterval, waveProgress);

            // 몬스터 스폰
            SpawnEnemy(currentWave);

            // 계산된 스폰 간격만큼 대기
            yield return new WaitForSeconds(currentSpawnInterval);

            Debug.Log(elapsedTime);
        }

        isSpawning = false;
        Debug.Log(currentWave.waveName + " 종료!");
        // TODO: 웨이브 종료 후 상점 열기 등 다음 로직 호출
    }

    void SpawnEnemy(Wave currentWave)
    {
        // 1. 스폰할 몬스터를 가중치 기반으로 랜덤 선택
        PoolObject enemyToSpawn = ObjectPool.Instance.Get("z").Get();
        if (enemyToSpawn == null) return;

        // 2. 플레이어 주변의 랜덤한 위치 계산
        Vector2 v = DungeonBoundary.Instance.GetRandomPosition();
        enemyToSpawn.transform.position = v;

        activeEnemies.Add(enemyToSpawn.GetComponent<EnemyAI>());
    }

    GameObject ChooseEnemy(Wave currentWave)
    {
        int totalWeight = 0;
        foreach (var enemy in currentWave.enemies)
        {
            totalWeight += enemy.weight;
        }

        int randomWeight = Random.Range(0, totalWeight);

        foreach (var enemy in currentWave.enemies)
        {
            if (randomWeight < enemy.weight)
            {
                return enemy.enemyPrefab;
            }
            randomWeight -= enemy.weight;
        }
        return null;
    }
}