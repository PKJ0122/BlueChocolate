using UnityEngine.UI;

public class AdsBuffUI : UIBase
{
    const float BUFF_TIME = 86400f;
    const float BUFF_VALUE = 2f;
    const StatModType STATMODTYPE = StatModType.PercentMult;

    Button _close;
    Button _ads;


    protected override void Awake()
    {
        base.Awake();
        PlayerData.Instance.Container.AdsBuffTimeChanged += Refresh;

        _close = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _ads = transform.Find("Panel/Image/Button - Ads").GetComponent<Button>();

        _close.onClick.AddListener(Hide);
        _ads.onClick.AddListener(RequestAds);

        if (PlayerData.Instance.Container.AdsBuffTime > 0) BuffEnable();
    }

    void Refresh(float value)
    {
        _ads.interactable = value > 0;
    }

    async void RequestAds()
    {
        UIBase lodingUI = UIManager.Instance.Get<LodingUI>();
        PopUpUI popUpUI = UIManager.Instance.Get<PopUpUI>();

        lodingUI.Show();
        AdShowResult result = await AdManager.Instance.LoadAndShowAd();
        lodingUI.Hide();

        switch (result)
        {
            case AdShowResult.Success:
                PlayerData.Instance.Container.AdsBuffTime += BUFF_TIME;
                BuffEnable();
                popUpUI.Show("보상 지급 완료 !\n광고를 시청해주셔서 감사합니다 !");
                break;
            case AdShowResult.Failed:
                popUpUI.Show("광고로드에 실패하였습니다.\n잠시후 이용해주세요.");
                break;
            case AdShowResult.Canceled:
                popUpUI.Show("중간에 광고를 종료하시면\n보상이 지급 되지 않습니다 ㅠ");
                break;
        }
    }

    void BuffEnable()
    {
        StartCoroutine(BuffManager.Instance.C_Buff(BUFF_VALUE, STATMODTYPE));
    }
}
