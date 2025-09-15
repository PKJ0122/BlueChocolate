using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBase<UIManager>
{
    /// <summary>
    /// 모든 UI가 등록되어있는 Dictionary
    /// </summary>
    Dictionary<Type, UIBase> _uis = new Dictionary<Type, UIBase>();

    /// <summary>
    /// 현재 활성화중인 UI
    /// </summary>
    Stack<UIBase> _ui = new Stack<UIBase>();
    public Stack<UIBase> Ui => _ui;

    public event Action<bool> OnUIChange;

    
    /// <summary>
    /// UIManager에 UI를 등록하는 함수
    /// </summary>
    public void RegisterUI(UIBase ui)
    {
        if (!_uis.TryAdd(ui.GetType(), ui))
        {
            Debug.LogWarning("이미 등록된 UI입니다.");
        }
    }

    /// <summary>
    /// UIManager에 등록되어있는 딕셔너리에 Type를 키로 UI객체를 반환해주는 함수
    /// </summary>
    public T Get<T>()
        where T : UIBase
    {
        return (T)_uis[typeof(T)];
    }

    /// <summary>
    /// UIManager에 등록되어있는 활성화 Stack에 등록하는 함수
    /// </summary>
    public void PushUI(UIBase ui)
    {
        _ui.Push(ui);
        ui.SortingOrder = _ui.Count;
        OnUIChange?.Invoke(_ui.Count == 0);
    }

    /// <summary>
    /// UIManager에 등록되어있는 활성화 Stack에서 제거해주는 함수
    /// </summary>
    public void PopUI(UIBase ui)
    {
        if (_ui.Peek() != ui)
        {
            Debug.LogWarning("해당 UI가 최상단이 아닙니다.");
            return;
        }
        _ui.Pop();
        OnUIChange?.Invoke(_ui.Count == 0);
    }

    public bool UIPeekCheck(UIBase ui)
    {
        if (_ui.Count == 0)
        {
            return false;
        }

        return _ui.Peek() == ui;
    }
}
