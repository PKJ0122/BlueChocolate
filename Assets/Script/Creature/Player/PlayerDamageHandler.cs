using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour
{
    [Header("넉백 설정")]
    [SerializeField] public float knockbackForce = 7f;
    [Tooltip("넉백 이후, 다음 넉백이 가능해질 때까지의 면역 시간입니다.")]
    [SerializeField] private float knockbackInvulnerabilityDuration = 2f;

    private Rigidbody2D rb;
    private PlayerStat playerStat;
    private PlayerController playerController;

    private Vector2 _lastKnockbackDirection;
    private float _knockbackCooldownEndTime; // 넉백 면역이 끝나는 시간을 기록할 변수

    /// <summary>
    /// 현재 넉백 면역 상태인지 여부를 반환합니다.
    /// </summary>
    public bool IsKnockbackImmune => Time.time < _knockbackCooldownEndTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStat = GetComponent<PlayerStat>();
        playerController = GetComponent<PlayerController>();
    }

    public void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        if (playerStat.IsDead) return;

        if (!IsKnockbackImmune)
        {
            playerStat.DecreaseHealth(damage);
            // 넉백 방향만 저장하고 상태 변경을 요청
            playerController.lastKnockbackDirection = knockbackDirection; // PlayerController에 방향 저장
            playerController.ChangeState(playerController.KnockbackState);
        }

        if (playerStat.IsDead)
        {
            Die();
        }
    }

    public void StartKnockbackCooldown()
    {
        _knockbackCooldownEndTime = Time.time + knockbackInvulnerabilityDuration;
        // Debug.Log($"넉백 면역 시작! {knockbackInvulnerabilityDuration}초 후에 풀립니다.");
    }

    public void ApplyKnockback()
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(_lastKnockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    private void Die()
    {
        Debug.Log("플레이어가 사망했습니다.");
        //this.enabled = false;
    }
}
