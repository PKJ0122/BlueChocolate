using UnityEngine;
public class DungeonBoundary : SingletonMonoBase<DungeonBoundary>
{
    SpriteRenderer _dungeonBackground;

    public Bounds Boundary { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _dungeonBackground = GetComponent<SpriteRenderer>();
        Boundary = _dungeonBackground.bounds;

        Application.targetFrameRate = 60;
    }

    /// <summary>
    /// 경계 내의 랜덤한 위치를 반환합니다. (몬스터 스폰 시 사용)
    /// </summary>
    /// <returns>경계 내의 랜덤한 Vector2 위치</returns>
    public Vector2 GetRandomPosition()
    {
        float randomX = Random.Range(Boundary.min.x, Boundary.max.x);
        float randomY = Random.Range(Boundary.min.y, Boundary.max.y);
        return new Vector2(randomX, randomY);
    }

    // Scene 뷰에서 경계를 시각적으로 확인하기 위한 Gizmos (매우 유용합니다)
    private void OnDrawGizmos()
    {
        if (Boundary.size != Vector3.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Boundary.center, Boundary.size);
        }
    }
}