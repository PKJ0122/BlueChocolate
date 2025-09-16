using UnityEngine;

public class KnockbackState : IState
{
    readonly PlayerController _player;
    readonly PlayerDamageHandler _playerDH;
    readonly float _knockbackDuration;
    readonly float _knockbackForce;
    readonly float _friction;

    float _knockbackTimer;

    public KnockbackState(PlayerController player, float duration, float knockbackForce, float friction)
    {
        _player = player;
        _playerDH = player.GetComponent<PlayerDamageHandler>();
        _knockbackDuration = duration;
        _knockbackForce = knockbackForce;
        _friction = friction;
    }

    public void Enter()
    {
        _knockbackTimer = 0f;

        float force = _knockbackForce;
        _player.ApplyKnockbackVelocity(_player.LastKnockbackDirection, force);
        _playerDH.StartKnockbackCooldown();
    }

    public void Update()
    {
        _player.DecayVelocity(_friction);

        _knockbackTimer += Time.deltaTime;
        if (_knockbackTimer >= _knockbackDuration)
        {
            _player.ChangeState(_player.Auto ? _player.AutoChaseState : _player.ChaseState);
        }
    }

    public void Exit()
    {
    }
}
