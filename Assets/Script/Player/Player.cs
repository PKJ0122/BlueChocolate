using UnityEngine;

public class Player : MonoBehaviour
{
    IDamageable _target;

    float _attackpower;


    public void Attack()
    {
        _target?.TakeDamage(_attackpower);
    }

    public void SetTargetDamageable(IDamageable target)
    {
        _target = target;
    }
}
