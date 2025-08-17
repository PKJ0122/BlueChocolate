using System;
using UnityEngine;

public class DataContainer
{
    [SerializeField] private float _adsBuffTime;
    public float AdsBuffTime
    {
        get => _adsBuffTime;
        set
        {
            _adsBuffTime = value < 0 ? 0 : value;
            OnAdsBuffTimeChange?.Invoke(_adsBuffTime);
        }
    }
    public event Action<float> OnAdsBuffTimeChange;
}
