using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUI : UIBase
{
    TMP_Text _title;
    Button _gameStart;
    Button _rule;
    Button _setting;


    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += (s, l) =>
        {
            Show();
        };
    }
}