using System;
using UnityEngine;

public class ChaseState : IState
{
    private readonly Attacker _attacker;
    private readonly Func<EnemyAI> _findClosestEnemyFunc;
    private float _attackRange;

    public ChaseState(Attacker attacker, Func<EnemyAI> findClosestEnemyFunc, float attackRange)
    {
        _attacker = attacker;
        _findClosestEnemyFunc = findClosestEnemyFunc;
        _attackRange = attackRange;
    }

    public void Enter()
    {
        _attacker.CurrentTarget = null;
    }

    public void Update()
    {
        EnemyAI target = _findClosestEnemyFunc();

        if (target != null)
        {
            _attacker.CurrentTarget = target;
            float distance = Vector3.Distance(_attacker.transform.position, target.transform.position);

            if (distance <= _attackRange)
            {
                _attacker.ChangeState(_attacker.AttackState);
                return;
            }
        }
    }

    public void Exit()
    {
    }
}