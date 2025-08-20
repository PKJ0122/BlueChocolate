using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UIBase
{
    protected override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = 60;
    }
}
