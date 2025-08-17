using System.Collections;
using UnityEngine;

public class PotionUI : UIBase
{
    const float TICK_TIME = 1f;
    const float BUFF_AMOUNT = 2f;
    const StatModType STATMODTYPE = StatModType.PercentMult;

    PlayerStat _stat;
    DataContainer _container;


    protected override void Awake()
    {
        base.Awake();
        _stat = FindAnyObjectByType<PlayerStat>();
        _container = PlayerData.Instance.Container;

        StartCoroutine(C_Buff());
    }

    readonly WaitForSecondsRealtime _delay = new(TICK_TIME);

    IEnumerator C_Buff()
    {
        if (_container.AdsBuffTime > 0)
        {
            StatModifier buff = new(BUFF_AMOUNT, STATMODTYPE);
            _stat.AddModifier(buff);

            while (_container.AdsBuffTime > 0)
            {
                yield return _delay;
                _container.AdsBuffTime -= TICK_TIME;
            }

            _stat.RemoveModifier(buff);
        }
    }
}