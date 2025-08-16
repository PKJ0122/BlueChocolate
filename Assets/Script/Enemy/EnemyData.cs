using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData",fileName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    public Sprite[] Sprites;
    public float Hp;
    public int Gold;
}