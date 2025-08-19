using System.Collections;
using UnityEngine;

public class BuffManager : Singleton<BuffManager>
{
    const float TICK_TIME = 1f;

    DataContainer _container;
    PlayerStat _stat;


    protected override void Awake()
    {
        base.Awake();
        _container = PlayerData.Instance.Container;
        _stat = PlayerStat.Instance;
    }

    readonly WaitForSecondsRealtime _delay = new(TICK_TIME);

    public IEnumerator C_Buff(float value, StatModType modType)
    {
        StatModifier buff = new(value, modType);
        _stat.AddModifier(buff);

        while (_container.AdsBuffTime > 0)
        {
            yield return _delay;
            _container.AdsBuffTime -= TICK_TIME;
        }

        _stat.RemoveModifier(buff);
    }
}
