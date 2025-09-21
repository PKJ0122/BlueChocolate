using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CostDatas" , menuName = "ScriptableObject/CostDatas")]
public class CostDatas : ScriptableObject
{
    public CostData[] costDatas;

    Dictionary<int, CostData> _costDataDic;
    public Dictionary<int, CostData> CostDataDic
    {
        get
        {
            if (_costDataDic == null)
            {
                _costDataDic = new();

                foreach (var data in costDatas)
                {
                    _costDataDic.Add(data.Cost, data);
                }
            }

            return _costDataDic;
        }
    }
}


[Serializable]
public class CostData
{
    public int Cost;
    public Sprite Back;
    public Color Color;
}