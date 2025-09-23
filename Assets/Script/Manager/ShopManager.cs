using System;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopManager : SingletonMonoBase<ShopManager>
{
    ReRollData _rollData;
    CostDatas _costDatas;

    int _level = 1;
    public int Level
    {
        get => _level;
        set
        {
            _level = value;
            _currentProbabilityData = _rollData.ReRollProbabilitieDics[value].reRollProbabilities;
            OnLevelChanged?.Invoke(value);
        }
    }
    public Action<int> OnLevelChanged;

    int _exp;
    public int Exp
    {
        get => _exp;
        set
        {
            _exp = value;
            OnExpChanged?.Invoke(value);
        }
    }
    public Action<int> OnExpChanged;

    ReRollProbability[] _currentProbabilityData;
    public ReRollProbability[] CurrentProbabilityData => _currentProbabilityData;

    readonly string[] _currentShop = new string[6];
    public string[] CurrentShop => _currentShop;

    public Action<string[]> ShopChanged;

    protected override void Awake()
    {
        base.Awake();
        _rollData = Resources.Load<ReRollData>("ReRollData");
        _costDatas = Resources.Load<CostDatas>("CostDatas");
        _currentProbabilityData = _rollData.ReRollProbabilitieDics[Level].reRollProbabilities;
    }

    public string Probability()
    {
        Array.Sort(_currentProbabilityData);

        StringBuilder builder = new();

        for (int i = 0; i < _currentProbabilityData.Length; i++)
        {
            ReRollProbability reRollProbability = _currentProbabilityData[i];
            Cost cost = reRollProbability.Cost;

            CostData costData = _costDatas.CostDataDic[(int)cost];
            Color color = costData.Color;

            string hexColor = ColorUtility.ToHtmlStringRGBA(color);
            builder.Append($"<color=#{hexColor}> ■ {reRollProbability.Probability:F0}%");
        }

        return builder.ToString();
    }

    public void ReRoll()
    {
        Array.Clear(_currentShop, 0, _currentShop.Length);

        for (int i = 0; i < _currentShop.Length; i++)
        {
            int dumy = Random.Range(1, 101);
            int zz = 0;
            int zzz = 0;

            for (int j = 0; j < _currentProbabilityData.Length; j++)
            {
                int probability = _currentProbabilityData[j].Probability;
                zzz += probability;

                if (zz < dumy && dumy <= zzz)
                {
                    _currentShop[i] = _currentProbabilityData[j].Cost.ToString();
                    break;
                }

                zz = zzz;
            }
        }

        string zzzz = string.Empty;

        for (int i = 0; i < _currentShop.Length; i++)
        {
            zzzz += _currentShop[i];
            zzzz += " / ";
        }

        Debug.Log(zzzz);
    }

    public void LevelUp()
    {
        Level++;
    }

    public void BuySlime(int count)
    {
        if (_currentShop[count].Equals(string.Empty)) return;

        // 슬라임 구매처리
    }
}
