using TMPro;
using UnityEngine.UI;

public class ShopUI : UIBase
{
    public Button _levelUp;
    public Button _reRoll;

    public TextMeshProUGUI _level;
    public TextMeshProUGUI _probability;


    protected override void Awake()
    {
        base.Awake();
        _levelUp.onClick.AddListener(ShopManager.Instance.LevelUp);
        _reRoll.onClick.AddListener(ShopManager.Instance.ReRoll);

        ShopManager.Instance.OnLevelChanged += LevelChange;
    }

    private void Start()
    {
        LevelChange(1);
    }

    private void OnDisable()
    {
        if (!ShopManager.IsApplicationQuit)
        {
            ShopManager.Instance.OnLevelChanged -= LevelChange;
        }
    }

    void LevelChange(int level)
    {
        _level.SetText("Lv. {0}", level);
        _probability.SetText(ShopManager.Instance.Probability());
    }
}
