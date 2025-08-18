using System;
using UnityEngine;

public class DataContainer
{
    [SerializeField] private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            NameChanged?.Invoke(_name);
        }
    }
    public event Action<string> NameChanged;

    [SerializeField] private int _weaponUpgrade;
    public int WeaponUpgrade
    {
        get => _weaponUpgrade;
        set
        {
            _weaponUpgrade = value;
            WeaponUpgradeChanged?.Invoke(_weaponUpgrade);
        }
    }
    public event Action<int> WeaponUpgradeChanged;

    [SerializeField] private int _gold;
    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            GoldChanged?.Invoke(_gold);
        }
    }
    public event Action<int> GoldChanged;

    [SerializeField] private int _selectSlimeNum;
    public int SelectSlimeNum
    {
        get => _selectSlimeNum;
        set
        {
            _selectSlimeNum = value;
            SelectSlimeNumChanged?.Invoke(_selectSlimeNum);
        }
    }
    public event Action<int> SelectSlimeNumChanged;

    [SerializeField] private float _adsBuffTime;
    public float AdsBuffTime
    {
        get => _adsBuffTime;
        set
        {
            _adsBuffTime = value < 0 ? 0 : value;
            AdsBuffTimeChanged?.Invoke(_adsBuffTime);
        }
    }
    public event Action<float> AdsBuffTimeChanged;
}
