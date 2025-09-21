using System.Collections.Generic;
using UnityEngine;

public class AttackerManager : SingletonMonoBase<AttackerManager>
{
    [SerializeField] Vector3[] _offsets;
    public Vector3[] Offsets => _offsets;

    Attacker[] _deck;


    public void AttackerBuy()
    {

    }

    public void AttackerSell()
    {

    }
}