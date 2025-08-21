using TMPro;

public class GoldUI : UIBase
{
    TMP_Text _gold;

    protected override void Awake()
    {
        base.Awake();
        _gold = transform.Find("Panel/Image - Gold/Text (TMP) - Gold").GetComponent<TMP_Text>();

        int gold = PlayerData.Instance.Container.Gold;
        Refresh(gold);
        PlayerData.Instance.Container.GoldChanged += Refresh;
    }

    void Refresh(int gold)
    {
        _gold.text = gold.ToString();
    }
}