using UnityEngine;
using UnityEngine.Audio;

public class SettingManager : SingletonMonoBase<SettingManager>
{
    const string SETTING_KEY = "Setting";
    public const string BGM = "BGM";
    public const string SFX = "SFX";
    const float MIN_SOUND = -20f;
    const float MAX_SOUND = 0f;
    const float MUTE_SOUND = -80f;
    const float DARK_MODE_COLOR = 0.2f;
    const int FPS_60 = 60;
    const int FPS_45 = 45;
    const string E_MAIL = "qlrrudwns@gmail.com";

    #region Setting
    Setting _setting;
    public Setting Setting
    {
        get
        {
            if (_setting == null)
            {
                if (PlayerPrefs.HasKey(SETTING_KEY))
                {
                    string setting = PlayerPrefs.GetString(SETTING_KEY);
                    _setting = JsonUtility.FromJson<Setting>(setting);
                }
                else
                {
                    _setting = new Setting();
                }
            }

            return _setting;
        }
    }
    #endregion

    AudioMixerGroup _mixer;


    protected override void Awake()
    {
        base.Awake();
        _mixer = Resources.Load<AudioMixerGroup>("AudioMixer");
        Application.runInBackground = true;
    }

    public void SoundValueChange(string mixerName, float value)
    {
        float Volume = value == 0 ? MUTE_SOUND : Mathf.Lerp(MIN_SOUND, MAX_SOUND, value);
        _mixer.audioMixer.SetFloat(mixerName, Volume);

        if (mixerName.Equals(BGM))
        {
            Setting.BGM_Value = value;
        }
        else if (mixerName.Equals(SFX))
        {
            Setting.SFX_Value = value;
        }

        SaveSetting();
    }

    public void DarkModeChange(bool isOn)
    {
        Camera camera = Camera.main;
        camera.backgroundColor = isOn ? new Color(DARK_MODE_COLOR, DARK_MODE_COLOR, DARK_MODE_COLOR) : Color.white;
        Setting.Dark_Mode = isOn;
        SaveSetting();
    }

    public void FPSChange(bool isOn)
    {
        Application.targetFrameRate = isOn ? FPS_60 : FPS_45;
        Setting.FPS60 = isOn;
        SaveSetting();
    }

    public void EmailCopy()
    {
        GUIUtility.systemCopyBuffer = E_MAIL;
    }

    void SaveSetting()
    {
        string data = JsonUtility.ToJson(Setting);
        PlayerPrefs.SetString(SETTING_KEY, data);
    }
}
