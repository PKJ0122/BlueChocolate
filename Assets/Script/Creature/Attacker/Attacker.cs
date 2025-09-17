using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Attacker : MonoBehaviour
{
    [SerializeField]public BaseAttack _p;

    public ChaseState ChaseState { get; private set; }
    public AttackState AttackState { get; private set; }

    public EnemyAI CurrentTarget { get; set; }

    private IState currentState;


    void Awake()
    {
        CreateIState();
    }

    void Start()
    {
        ChangeState(ChaseState);
    }

    private void FixedUpdate()
    {
        currentState?.Update();
    }

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Attack()
    {
        BaseAttack attack = Instantiate(_p);
        Vector3 direction = (CurrentTarget.transform.position - transform.position).normalized;
        attack.transform.position = transform.position;
        attack.Set(direction);
    }

    void CreateIState()
    {
        ChaseState = new ChaseState(this, FindClosestEnemy, 5f);
        AttackState = new AttackState(this);
    }
    #region Å¸°Ù Å½»ö ·ÎÁ÷
    public EnemyAI FindClosestEnemy()
    {
        EnemySpawner enemySpawner = EnemySpawner.Instance;
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
}
