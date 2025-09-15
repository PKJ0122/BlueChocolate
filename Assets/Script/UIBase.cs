using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class UIBase : MonoBehaviour
{
    public int SortingOrder
    {
        get => _canvas.sortingOrder;
        set => _canvas.sortingOrder = value;
    }

    protected Canvas _canvas;

    EventSystem _eventSystem;
    public EventSystem EventSystem => _eventSystem;

    GraphicRaycaster _graphicRaycaster;
    public GraphicRaycaster GraphicRaycaster => _graphicRaycaster;


    protected virtual void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        _eventSystem = EventSystem.current;

        UIManager.Instance.RegisterUI(this);
    }

    public virtual void Show()
    {
        if (_canvas.enabled) return;

        _canvas.enabled = true;
        UIManager.Instance.PushUI(this);
    }

    public virtual void Hide()
    {
        if (!_canvas.enabled) return;

        _canvas.enabled = false;
        UIManager.Instance.PopUI(this);
    }
}
