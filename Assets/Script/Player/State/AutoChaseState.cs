using System;
using UnityEngine;

public class AutoChaseState : IState
{
    private readonly PlayerController player;
    // 가장 가까운 적을 찾는 '방법(Func)'을 외부(PlayerController)에서 주입받음
    private readonly Func<Enemy> findClosestEnemyFunc;

    public AutoChaseState(PlayerController player, Func<Enemy> findClosestEnemyFunc)
    {
        this.player = player;
        this.findClosestEnemyFunc = findClosestEnemyFunc;
    }

    public void Enter()
    {
        Debug.Log("상태 진입: 오토추격");
        player.CurrentTarget = null; // 새로운 타겟을 찾기 위해 초기화
    }

    public void Update()
    {
        // 주입받은 Func를 실행하여 가장 가까운 적을 찾음
        Enemy target = findClosestEnemyFunc();

        if (target != null)
        {
            player.CurrentTarget = target;
            float distance = Vector3.Distance(player.transform.position, target.transform.position);

            // 사거리 밖에 있다면, 타겟을 향해 이동
            if (distance > player.attackRange)
            {
                player.AutoMove();
            }
            // 사거리 내에 들어왔다면, 공격 상태로 전환
            else
            {
                player.ChangeState(new AttackState(player, target));
            }
        }

        if (!player.Auto)
        {
            player.ChangeState(new ChaseState(player, player.FindClosestEnemy));
        }
    }

    public void Exit()
    {
        Debug.Log("상태 종료: 오토추격");
    }
}
