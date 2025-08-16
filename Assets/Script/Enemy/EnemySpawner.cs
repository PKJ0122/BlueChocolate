public class EnemySpawner : Singleton<EnemySpawner>
{
    Enemy _enemy;




    protected override void Awake()
    {
        base.Awake();

        _enemy = FindAnyObjectByType<Enemy>();
        Player player = FindAnyObjectByType<Player>();
        player.SetTargetDamageable(_enemy);
    }

    public void SetEnemyInfo()
    {

    }
}