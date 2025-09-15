using System;
using UnityEngine;

public class AutoChaseState : IState
{
    private readonly PlayerController player;
    private readonly Func<Enemy> findClosestEnemyFunc;

    public AutoChaseState(PlayerController player, Func<Enemy> findClosestEnemyFunc)
    {
        this.player = player;
        this.findClosestEnemyFunc = findClosestEnemyFunc;
    }

    public void Enter()
    {
        Debug.Log("상태 진입: 자동 추격");
        player.CurrentTarget = null;
        player.ChangeIRotate(typeof(AttackMode)); // 자동 추격 시에는 타겟을 바라보는게 자연스러움
    }

    public void Update()
    {
        Enemy target = findClosestEnemyFunc();

        if (target != null)
        {
            player.CurrentTarget = target;
            float distance = Vector3.Distance(player.transform.position, target.transform.position);

            if (distance > player.attackRange)
            {
                player.AutoMove();
            }
            else
            {
                player.ChangeState(player.AttackState);
            }
        }
        else
        {
            // 추격할 대상이 없으면 정지
            player.StopMove();
        }
    }

    public void Exit()
    {
        Debug.Log("상태 종료: 자동 추격");
    }
}