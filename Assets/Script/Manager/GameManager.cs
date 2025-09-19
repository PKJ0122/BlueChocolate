using DG.Tweening;

public class GameManager : SingletonMonoBase<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        DOTween.SetTweensCapacity(500, 200);
    }
}