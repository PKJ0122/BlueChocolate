using System;
using UnityEngine;

public class ChaseState : IState
{
    private readonly PlayerController player;
    private readonly Func<Enemy> findClosestEnemyFunc;

    public ChaseState(PlayerController player, Func<Enemy> findClosestEnemyFunc)
    {
        this.player = player;
        this.findClosestEnemyFunc = findClosestEnemyFunc;
    }

    public void Enter()
    {
        Debug.Log("상태 진입: 추격");
        player.CurrentTarget = null; // 새로운 타겟을 찾기 위해 초기화
    }

    public void Update()
    {
        Enemy target = findClosestEnemyFunc();

        if (target != null)
        {
            player.CurrentTarget = target;
            float distance = Vector3.Distance(player.gameObject.transform.position, target.transform.position);

            if (distance <= player.attackRange)
            {
                player.ChangeState(new AttackState(player, target));
            }
        }

        if (player.Auto)
        {
            player.ChangeState(new AutoChaseState(player, player.FindClosestEnemy));
            return;
        }

        player.Move();
    }

    public void Exit()
    {
        Debug.Log("상태 종료: 추격");
    }
}