using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : SingletonMonoBase<JoystickController>, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;

    float joystickRadius = 125f;

    Vector2 _inputVector;
    public Vector2 InputVertor
    {
        get => _inputVector;
        set
        {
            _inputVector = value;
            OnJoystickMoved?.Invoke(_inputVector);
        }
    }

    private RectTransform parentRectTransform;

    public event Action<Vector2> OnJoystickMoved;

    void Start()
    {
        parentRectTransform = joystickBackground.parent as RectTransform;

        joystickBackground.sizeDelta = new Vector2(joystickRadius * 2, joystickRadius * 2);

        joystickBackground.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            joystickBackground.anchoredPosition = localPoint;
        }

        joystickBackground.gameObject.SetActive(true);
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, eventData.position, eventData.pressEventCamera, out localPoint))
        {

            if (localPoint.magnitude > joystickRadius)
            {
                localPoint = localPoint.normalized * joystickRadius;
            }

            joystickHandle.anchoredPosition = localPoint;

            InputVertor = localPoint / joystickRadius;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickBackground.gameObject.SetActive(false);

        joystickHandle.anchoredPosition = Vector2.zero;
        InputVertor = Vector2.zero;
    }
}