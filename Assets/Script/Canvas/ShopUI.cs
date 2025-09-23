using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : UIBase
{
    Transform _shopSlotLocation;
    GameObject _shopSlotPrefab;

    public Button _levelUp;
    public Button _reRoll;

    public TextMeshProUGUI _level;
    public TextMeshProUGUI _probability;


    protected override void Awake()
    {
        base.Awake();
        _levelUp.onClick.AddListener(ShopManager.Instance.LevelUp);
        _reRoll.onClick.AddListener(ShopManager.Instance.ReRoll);

        _shopSlotLocation = transform.Find("Panel/Image - Shop");
        _shopSlotPrefab = Resources.Load<GameObject>("Image - ShopSlot");
    }

    private void Start()
    {
        LevelChange(1);
        Init();
    }

    private void OnEnable()
    {
        ShopManager.Instance.OnLevelChanged += LevelChange;
    }

    private void OnDisable()
    {
        if (!ShopManager.IsApplicationQuit)
        {
            ShopManager.Instance.OnLevelChanged -= LevelChange;
        }
    }

    public void CreateShopSlot(int count)
    {
        GameObject gameObject = Instantiate(_shopSlotPrefab, _shopSlotLocation, false);
        //Button button = gameObject.GetComponent<Button>();
        //TMP_Text tMP_Text = gameObject.GetComponent<TMP_Text>();


        //button.onClick.RemoveAllListeners();
        //button.onClick.AddListener(() => ShopManager.Instance.BuySlime(count));

        //void Refresh(string[] slims)
        //{
        //    gameObject.SetActive(true);
        //    tMP_Text.text = slims[count];
        //}

        //ShopManager.Instance.ShopChanged += Refresh;
    }

    void LevelChange(int level)
    {
        _level.SetText("Lv. {0}", level);
        _probability.SetText(ShopManager.Instance.Probability());
    }

    private void Init()
    {
        int slotCount = ShopManager.Instance.CurrentShop.Length;

        for (int i = 0; i < slotCount; i++)
        {
            CreateShopSlot(i);
        }
    }
}
