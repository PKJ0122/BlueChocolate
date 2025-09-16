using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerDataContainer
{
    [SerializeField] int _gold = 500;
    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            OnGoldChanged?.Invoke(_gold);
        }
    }
    public event Action<int> OnGoldChanged;

    [SerializeField] int _expLevel = 1;
    public int ExpLevel
    {
        get => _expLevel;
        set
        {
            _expLevel = value;
            OnExpLevelChanged?.Invoke(_expLevel);
        }
    }
    public event Action<int> OnExpLevelChanged;

    [SerializeField] int _nestLevel = 1;
    public int NestLevel
    {
        get => _nestLevel;
        set
        {
            _nestLevel = value;
            OnNestLevelChanged?.Invoke(_nestLevel);
        }
    }
    public event Action<int> OnNestLevelChanged;

    [SerializeField] bool _auto = false;
    public bool Auto
    {
        get => _auto;
        set
        {
            _auto = value;
            OnAutoChanged?.Invoke(_auto);
        }
    }
    public event Action<bool> OnAutoChanged;

    [SerializeField] float _autoTime;
    public float AutoTime
    {
        get => _autoTime;
        set
        {
            float newValue = (value <= 0) ? 0 : value;
            _autoTime = newValue;
            OnAutoTimeChanged?.Invoke(_autoTime);
        }
    }
    public event Action<float> OnAutoTimeChanged;

    public string GameEndTime = "1999-01-22";
    public bool First = false;
}