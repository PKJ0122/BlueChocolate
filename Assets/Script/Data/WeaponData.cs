using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObject/Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    public int Upgrade;
    public float Damage;
    public int EnhanceCost;
    public float EnhancementProbability;
}