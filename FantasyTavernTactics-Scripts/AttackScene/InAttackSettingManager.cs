using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class InAttackSettingManager : MonoBehaviour
{
    [SerializeField] GameObject SettingButton;
    [SerializeField] GameObject SettingPanel;

    [Header("ChangeBGM")]
    [SerializeField] Image[] changeBGMButtons = new Image[6];

    [SerializeField] Sprite[] buttonFlame = new Sprite[2];

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

    private GameInfomation _gameInfomation;

    void Start()
    {
        _gameInfomation = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>();
        for (int i = 0; i < changeBGMButtons.Length; i++)
        {
            if (i != _gameInfomation.isBattleBGM)
                changeBGMButtons[i].sprite = buttonFlame[0];
            else
                changeBGMButtons[i].sprite = buttonFlame[1];
        }

        _BGMVol = PlayerPrefs.GetFloat("BGMVolume", 0);
        BGMSlider.value = _BGMVol;
        _SEVol = PlayerPrefs.GetFloat("SEVolume", 0);
        SESlider.value = _SEVol;

        SettingPanel.SetActive(false);
    }

    //歯車ボタンの非表示
    public void InActiveButton()
    {
        SettingButton.SetActive(false);
        SettingPanel.SetActive(false);
    }

    //設定パネルの表示、非表示
    public void ChangeActivePanel()
    {
        AudioManager.Instance.SE(0);
        if (SettingPanel.activeSelf)
            SettingPanel.SetActive(false);
        else
            SettingPanel.SetActive(true);
    }

    //音量変更
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

    //BGM切り替え
    public void ChangeBGM(int num)
    {
        AudioManager.Instance.SE(0);
        _gameInfomation.isBattleBGM = num;
        PlayerPrefs.SetInt("isBattleBGM", num);
        for (int i = 0; i < changeBGMButtons.Length; i++)
        {
            if (i != num)
                changeBGMButtons[i].sprite = buttonFlame[0];
            else
                changeBGMButtons[i].sprite = buttonFlame[1];
        }
    }
}
