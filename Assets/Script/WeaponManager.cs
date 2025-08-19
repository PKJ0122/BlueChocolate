using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    #region WeaponData Dictionary
    private Dictionary<int, WeaponData> _weaponData;
    public Dictionary<int, WeaponData> WeaponData
    {
        get
        {
            if (_weaponData == null)
            {
                _weaponData = new Dictionary<int, WeaponData>();
                WeaponDatas weaponDatas = Resources.Load<WeaponDatas>("WeaponDatas");
                foreach (WeaponData item in weaponDatas.weaponDatas)
                {
                    if (!_weaponData.TryAdd(item.Upgrade, item))
                    {
                        Debug.LogWarning($"WeaponDatas Level : {item.Upgrade} Warning");
                    }
                }
            }

            return _weaponData;
        }
    }

    public WeaponData GetWeaponData(int upgrade) => WeaponData[upgrade];
    #endregion

    protected override void Awake()
    {
        base.Awake();
    }
}
