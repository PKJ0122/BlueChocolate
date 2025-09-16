using System;
using UnityEngine;

public class ChaseState : IState
{
    private readonly PlayerController player;
    private readonly Func<EnemyAI> findClosestEnemyFunc;
    private float _attackRange;

    public ChaseState(PlayerController player, Func<EnemyAI> findClosestEnemyFunc, float attackRange)
    {
        this.player = player;
        this.findClosestEnemyFunc = findClosestEnemyFunc;
        this._attackRange = attackRange;
    }

    public void Enter()
    {
        Debug.Log("상태 진입: 수동 추격");
        player.CurrentTarget = null;
        player.ChangeIRotate(typeof(ChaseMode));
    }

    public void Update()
    {
        EnemyAI target = findClosestEnemyFunc();

        if (target != null)
        {
            player.CurrentTarget = target;
            float distance = Vector3.Distance(player.transform.position, target.transform.position);

            // 사거리 내에 들어왔다면 공격 상태로 전환
            if (distance <= _attackRange)
            {
                // new AttackState(...) 대신 PlayerController가 가진 인스턴스를 사용
                player.ChangeState(player.AttackState);
                return;
            }
        }

        // 수동 이동은 항상 처리
        player.Move();
    }

    public void Exit()
    {
        Debug.Log("상태 종료: 수동 추격");
    }
}