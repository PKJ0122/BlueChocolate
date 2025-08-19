using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObject/Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    public int Upgrade;
    public float Attack;
    public int EnhanceCost;
    public float EnhancementProbability;
}