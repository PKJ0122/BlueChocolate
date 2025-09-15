using UnityEngine;

public class AttackState : IState
{
    private readonly PlayerController player;
    private readonly Enemy targetEnemy;

    public AttackState(PlayerController player, Enemy target)
    {
        this.player = player;
        this.targetEnemy = target;
    }

    public void Enter()
    {
        Debug.Log($"상태 진입: {targetEnemy.name} 공격 시작");
        player.CurrentTarget = targetEnemy;
        player.ChangeAnimation("IsAttack");
    }

    public void Update()
    {
        // 타겟이 죽었는지(혹은 파괴되었는지) 먼저 확인
        if (targetEnemy == null || !targetEnemy.gameObject.activeSelf)
        {
            if (player.Auto)
            {
                // 타겟이 죽었다면 다시 추격 상태로 전환
                player.ChangeState(new AutoChaseState(player, player.FindClosestEnemy));
                return;
            }
            else
            {
                player.ChangeState(new ChaseState(player, player.FindClosestEnemy));
                return;
            }
        }

        if (player.Auto)
        {
            return;
        }


        player.Move();
    }

    public void Exit()
    {
        Debug.Log($"상태 종료: {targetEnemy.name} 공격 종료");
        player.ChangeAnimation("IsIdle");
        player.CurrentTarget = null;
    }
}