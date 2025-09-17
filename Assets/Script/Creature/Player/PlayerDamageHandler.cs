using DG.Tweening;
using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour
{
    const float KNOCKBACK_FORCE = 25;
    const float KNOCKBACK_INVULNERABILITY_DURATION = 1.5f;
    int _hitEffectHash = Shader.PropertyToID("_FlashAmount");
    const float HIT_EFFECT_DURATION = 0.3f;


    Rigidbody2D _rb;
    PlayerStat _playerStat;
    PlayerController _playerController;

    Material _material;

    Vector2 _lastKnockbackDirection;
    float _knockbackCooldownEndTime;

    public bool IsKnockbackImmune => Time.time < _knockbackCooldownEndTime;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerStat = GetComponent<PlayerStat>();
        _playerController = GetComponent<PlayerController>();
        _material = GetComponent<SpriteRenderer>().material;
    }

    public void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        if (_playerStat.IsDead) return;

        if (!IsKnockbackImmune)
        {
            _playerStat.DecreaseHealth(damage);
            _playerController.LastKnockbackDirection = knockbackDirection;
            _playerController.ChangeState(_playerController.KnockbackState);
            PlayHitEffect();
        }
    }

    public void PlayHitEffect()
    {
        _material.DOKill();
        _material.DOFloat(1f, _hitEffectHash, HIT_EFFECT_DURATION / 2).SetLoops(2, LoopType.Yoyo);
    }
    public void StartKnockbackCooldown()
    {
        _knockbackCooldownEndTime = Time.time + KNOCKBACK_INVULNERABILITY_DURATION;
    }

    public void ApplyKnockback()
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(_lastKnockbackDirection * KNOCKBACK_FORCE, ForceMode2D.Impulse);
    }
}
