using System;

public interface IStat
{
    /// <summary>
    /// 이 엔티티가 피해를 입었을 때 발생하는 이벤트입니다.
    /// </summary>
    event Action<float> OnTakeDamage;

    /// <summary>
    /// 이 엔티티가 사망했을 때 발생하는 이벤트입니다.
    /// </summary>
    event Action OnDied;

    /// <summary>
    /// 현재 사망 상태인지 여부를 반환합니다.
    /// </summary>
    bool IsDead { get; }
}