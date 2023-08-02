using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;

public class SettingManager : MonoBehaviour
{
    //言語設定、音量設定、課金及びそれらのパネルの表示を司る
    [SerializeField] GameObject SettingPanel;

    [Header("Language")]
    [SerializeField] Image JapaneseButton;
    [SerializeField] Image EnglishButton;
    [SerializeField] Sprite[] buttonFlame = new Sprite[2];

    private BattlePracticeManager _battlePracticeManager;
    private StageSelectManager _stageSelectManager;
    private OptionManager _optionManager;

    [Header("Volume")]
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Slider BGMSlider;
    [SerializeField]
    private Slider SESlider;

    private float _BGMVol;
    private float _SEVol;

    private bool isFirstTime = true;

    [Header("Purchase")]
    [SerializeField] GameObject ExtraOptionPanel;
    [SerializeField] Image[] _checkImage2 = new Image[13];

    private GameInfomation _gameInfomation;

    private void Awake()
    {
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            JapaneseButton.sprite = buttonFlame[0];
            EnglishButton.sprite = buttonFlame[1];
            if (PlayerPrefs.GetInt("Language", 1) == 0)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
                JapaneseButton.sprite = buttonFlame[1];
                EnglishButton.sprite = buttonFlame[0];
            }
        }
        else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            JapaneseButton.sprite = buttonFlame[1];
            EnglishButton.sprite = buttonFlame[0];
            if (PlayerPrefs.GetInt("Language", 0) == 1)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                JapaneseButton.sprite = buttonFlame[0];
                EnglishButton.sprite = buttonFlame[1];
            }
        }
    }

    void Start()
    {
        _battlePracticeManager = GetComponentInParent<BattlePracticeManager>();
        _stageSelectManager = GetComponent<StageSelectManager>();
        _optionManager = GetComponent<OptionManager>();

        _BGMVol = PlayerPrefs.GetFloat("BGMVolume", 0);
        BGMSlider.value = _BGMVol;

        _SEVol = PlayerPrefs.GetFloat("SEVolume", 0);
        SESlider.value = _SEVol;
        audioMixer.SetFloat("BGMVol", _BGMVol);
        audioMixer.SetFloat("SEVol", _SEVol);

        SettingPanel.SetActive(false);
    }

    public void ChangeActivePanel()
    {
        AudioManager.Instance.SE(0);
        if (SettingPanel.activeSelf)
            SettingPanel.SetActive(false);
        else
            SettingPanel.SetActive(true);
    }

    public void ChangeLanguage(int num)
    {
        if (PlayerPrefs.GetInt("Language", 2) == num)
            return;
        AudioManager.Instance.SE(0);
        switch (num)
        {
            case 0:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
                JapaneseButton.sprite = buttonFlame[1];
                EnglishButton.sprite = buttonFlame[0];
                PlayerPrefs.SetInt("Language", 0);
                break;
            case 1:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                JapaneseButton.sprite = buttonFlame[0];
                EnglishButton.sprite = buttonFlame[1];
                PlayerPrefs.SetInt("Language", 1);
                break;
        }
        //ボードサイズのテキストとAILevelと戦闘練習場の役職名を切り替える
        _battlePracticeManager.SetText();
        _stageSelectManager.SetText();
        _optionManager.SetText();
    }

    public void SetBGM(float volume)
    {
        if (volume == -40f)
            volume = -80f;
        audioMixer.SetFloat("BGMVol", volume);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSE(float volume)
    {
        if (volume == -40f)
            volume = -80f;
        audioMixer.SetFloat("SEVol", volume);
        PlayerPrefs.SetFloat("SEVolume", volume);
        if (isFirstTime)
        {
            isFirstTime = false;
            return;
        }
        AudioManager.Instance.SE(0);
    }
}
