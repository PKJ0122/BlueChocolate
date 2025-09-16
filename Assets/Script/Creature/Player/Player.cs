using System;
using UnityEngine;

public class Player : SingletonMonoBase<Player>
{
    public PlayerDamageHandler PlayerDamageHandler { get; private set; } 
    public PlayerController PlayerController { get; private set; }



    protected override void Awake()
    {
        base.Awake();
        PlayerDamageHandler = GetComponent<PlayerDamageHandler>();
        PlayerController = GetComponent<PlayerController>();
    }
}
