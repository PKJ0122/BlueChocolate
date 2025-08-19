using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpUI : UIBase
{
    TMP_Text _detail;


    protected override void Awake()
    {
        base.Awake();
        _detail = transform.Find("Panel/Image/Text (TMP) - Detail").GetComponent<TMP_Text>();
    }

    public void Show(string detail)
    {
        base.Show();
        _detail.text = detail;
    }
}
