using UnityEngine;

public class AttackState : IState
{
    private readonly Attacker _attacker;
    private EnemyAI currentAttackTarget;

    float _delay = 0.75f;
    float zz = 0f;


    public AttackState(Attacker attacker)
    {
        _attacker = attacker;
    }

    public void Enter()
    {
        currentAttackTarget = _attacker.CurrentTarget;

        if (currentAttackTarget == null)
        {
            _attacker.ChangeState(_attacker.ChaseState);
            return;
        }

        zz = 0;
        _attacker.Attack();
    }

    public void Update()
    {
        zz += Time.deltaTime;

        if (zz >= _delay)
        {
            _attacker.ChangeState(_attacker.ChaseState);
        }
    }

    public void Exit()
    {

    }
}