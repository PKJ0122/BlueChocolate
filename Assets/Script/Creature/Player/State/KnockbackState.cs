using UnityEngine;

public class KnockbackState : IState
{
    private readonly PlayerController player;
    private readonly PlayerDamageHandler playerDH;
    private readonly float knockbackDuration;
    private float knockbackTimer;
    private float friction = 5f; // 마찰 계수 (높을수록 빨리 멈춤)

    public KnockbackState(PlayerController player, float duration)
    {
        this.player = player;
        playerDH = player.GetComponent<PlayerDamageHandler>();
        this.knockbackDuration = duration;
    }

    public void Enter()
    {
        // Debug.Log("상태 진입: 넉백");
        knockbackTimer = 0f;

        // PlayerController의 메서드를 호출하여 _currentVelocity에 넉백 속도를 즉시 적용
        float force = playerDH.knockbackForce;
        player.ApplyKnockbackVelocity(player.lastKnockbackDirection, force);
    }

    public void Update()
    {
        // 넉백이 진행되는 동안 속도를 점진적으로 감소시킴 (자연스러운 감속)
        player.DecayVelocity(friction);

        knockbackTimer += Time.deltaTime;
        if (knockbackTimer >= knockbackDuration)
        {
            player.ChangeState(player.Auto ? player.AutoChaseState : player.ChaseState);
        }
    }

    public void Exit()
    {
        // Debug.Log("상태 종료: 넉백");
        playerDH.StartKnockbackCooldown();
        // StopMove()는 이제 필요 없습니다. DecayVelocity가 이미 속도를 거의 0으로 만들었기 때문입니다.
    }
}
