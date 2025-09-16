using UnityEngine;

public class AttackState : IState
{
    private readonly PlayerController player;
    private EnemyAI currentAttackTarget;

    // 생성자에서 더 이상 특정 target을 받지 않음
    public AttackState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        // 상태 진입 시 PlayerController의 현재 타겟을 나의 공격 타겟으로 설정
        currentAttackTarget = player.CurrentTarget;

        if (currentAttackTarget == null)
        {
            // 공격할 타겟이 없으면 즉시 현재 모드에 맞는 추격 상태로 복귀
            player.ChangeState(player.Auto ? player.AutoChaseState : player.ChaseState);
            return;
        }

        Debug.Log($"상태 진입: {currentAttackTarget.name} 공격 시작");
        player.ChangeIRotate(typeof(AttackMode));
        player.PlayAttackAnimation();
    }

    public void Update()
    {
        if (currentAttackTarget == null || !currentAttackTarget.gameObject.activeSelf)
        {
            // 타겟이 죽었다면 현재 모드에 맞는 추격 상태로 전환
            player.ChangeState(player.Auto ? player.AutoChaseState : player.ChaseState);
            return;
        }

        // Auto 모드가 아닐 경우, 공격 중에도 이동(무빙샷) 가능
        if (!player.Auto)
        {
            player.Move();
        }
    }

    public void Exit()
    {
        Debug.Log("상태 종료: 공격 종료");
        player.PlayIdleAnimation();
    }
}