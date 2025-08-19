using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : UIBase
{
    Button _buffAds;
    TMP_Text _remainingTime;
    CanvasGroup _buffImage;


    protected override void Awake()
    {
        base.Awake();
        PlayerData.Instance.Container.AdsBuffTimeChanged += Refuresh;
        _buffAds = transform.Find("Panel/Button - Potion").GetComponent<Button>();
        _remainingTime = transform.Find("Panel/Button - Potion/Text (TMP) - Time").GetComponent<TMP_Text>();
        _buffImage = transform.Find("Panel/Button - Potion/Image - Buff").GetComponent<CanvasGroup>();

        _buffAds.onClick.AddListener(() => UIManager.Instance.Get<AdsBuffUI>().Show());
    }

    void Refuresh(float value)
    {
        if (value <= 0)
        {
            _remainingTime.text = "";
            return;
        }

        int h = (int)(value / 3600);
        int m = (int)((value % 3600) / 60);

        _buffImage.alpha = value >= 0 ? 1 : 0.3f;
        _remainingTime.text = $"{h:00}h {m:00}m";
    }
}