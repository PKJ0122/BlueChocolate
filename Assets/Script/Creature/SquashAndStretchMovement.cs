using DG.Tweening;
using UnityEngine;

public class SquashAndStretchMovement : MonoBehaviour
{
    const float X_OFFSET = 1.2f;
    const float Y_OFFSET = 0.8f;

    readonly float _animationDuration = 0.5f;

    Vector3 _baseScale;
    float _squashScaleX;
    float _squashScaleY;

    Sequence _squashSequence;

    private void Awake()
    {
        _baseScale = transform.localScale;
        _squashScaleX = _baseScale.x * X_OFFSET;
        _squashScaleY = _baseScale.y * Y_OFFSET;

        CreateSquashAnimation();
    }

    void OnEnable()
    {
        _squashSequence.Restart();
    }

    void OnDisable()
    {
        _squashSequence?.Kill();
        transform.localScale = _baseScale;
    }

    void CreateSquashAnimation()
    {
        _squashSequence = DOTween.Sequence();

        _squashSequence
            .Append(transform.DOScale(new Vector3(_squashScaleX, _squashScaleY, _baseScale.z), _animationDuration / 2))
            .Append(transform.DOScale(_baseScale, _animationDuration / 2))
            .SetLoops(-1, LoopType.Restart)
            .SetAutoKill(false);
    }
}
