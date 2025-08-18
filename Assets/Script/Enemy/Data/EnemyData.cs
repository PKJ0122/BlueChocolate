using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Enemy/EnemyData", fileName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    public int Level;
    public Sprite[] Sprites;
    public float Hp;
    public int Gold;
}