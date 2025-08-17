using System.Collections;
using UnityEngine;

public class Enemy : Singleton<Enemy>, IDamageable, ISetEnemy
{
    SpriteRenderer _spriteRenderer;
    EnemyData _enemyData;

    readonly WaitForSecondsRealtime _delay = new WaitForSecondsRealtime(0.2f);


    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    IEnumerator Start()
    {
        int count = 0;

        while (true)
        {
            yield return _delay;

            if (_enemyData != null)
            {
                count = count + 1 >= _enemyData.Sprites.Length ? 0 : ++count;
                _spriteRenderer.sprite = _enemyData.Sprites[count];
            }
        }
    }

    public void TakeDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void SetEnemy(EnemyData enemyData)
    {
        _enemyData = enemyData;
    }
}
