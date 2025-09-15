using UnityEngine;
using UnityEngine.UI;

public class AutoUI : UIBase
{
    [SerializeField] Button _zz;

    [SerializeField] PlayerController _playerController;


    protected override void Awake()
    {
        base.Awake();
        _zz.onClick.AddListener(() =>
        {
            _playerController.Auto = !_playerController.Auto;
            Debug.Log("zz");
        });
    }
}
