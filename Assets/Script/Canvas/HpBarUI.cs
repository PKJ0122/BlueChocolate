using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : UIBase
{
    Slider _slider;

    Transform _target;
    RectTransform _sliderRect;


    protected override void Awake()
    {
        base.Awake();
        _slider = transform.Find("Panel/Slider - HpBar").GetComponent<Slider>();
        _target = Enemy.Instance.transform;
        _sliderRect = _slider.GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 targetPos = _target.position - new Vector3(0, -1.25f, 0);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPos);

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            screenPos,
            _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out localPos);

        _sliderRect.localPosition = localPos;
    }
}
