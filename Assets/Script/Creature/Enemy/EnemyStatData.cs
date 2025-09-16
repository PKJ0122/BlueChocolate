using UnityEngine;


[CreateAssetMenu(fileName = "EnemyStatData",menuName = "ScriptableObject/Enemy/EnemyStatData")]
public class EnemyStatData : ScriptableObject
{
    [Header("±‚∫ª Ω∫≈»")]
    public float maxHealth = 50f;
    public float attackDamage = 10f;

    [Header("AI «‡µø Ω∫≈»")]
    public float moveSpeed = 2f;
    public float attackRange = 0.3f;
}
