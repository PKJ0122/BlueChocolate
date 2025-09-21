using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReRollData", menuName = "ScriptableObject/ReRollData")]
public class ReRollData : ScriptableObject
{
    public ReRollProbabilitys[] ReRollProbabilities;

    Dictionary<int, ReRollProbabilitys> _reRollProbabilitieDics;
    public Dictionary<int, ReRollProbabilitys> ReRollProbabilitieDics
    {
        get
        {
            if (_reRollProbabilitieDics == null)
            {
                _reRollProbabilitieDics = new();

                foreach (var item in ReRollProbabilities)
                {
                    _reRollProbabilitieDics.Add(item.Level, item);
                }
            }

            return _reRollProbabilitieDics;
        }
    }
}