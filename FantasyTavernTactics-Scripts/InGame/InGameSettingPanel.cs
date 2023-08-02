using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class InGameSettingPanel : MonoBehaviour
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

    private OVRScreenFade _fade;

    private GameInfomation _gameInfomation;

    void Start()
    {
        _gameInfomation = GetComponent<GameInfomation>();
        for (int i = 0; i < changeBGMButtons.Length; i++)
        {
            if (i != _gameInfomation.isGameBGM)
                changeBGMButtons[i].sprite = buttonFlame[0];
            else
                changeBGMButtons[i].sprite = buttonFlame[1];
        }

        _fade = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<OVRScreenFade>();
        SettingButton.SetActive(false);
        SettingPanel.SetActive(false);
    }

    //歯車ボタンの表示
    public void ChangeActiveButton()
    {
        SettingButton.SetActive(true);
    }

    //設定パネルの表示、非表示
    public void ChangeActivePanel()
    {
        AudioManager.Instance.SE(0);
        if (SettingPanel.activeSelf)
            SettingPanel.SetActive(false);
        else
            SettingPanel.SetActive(true);
        _BGMVol = PlayerPrefs.GetFloat("BGMVolume", 0);
        BGMSlider.value = _BGMVol;
        _SEVol = PlayerPrefs.GetFloat("SEVolume", 0);
        SESlider.value = _SEVol;
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
        _gameInfomation.isGameBGM = num;
        PlayerPrefs.SetInt("isGameBGM", num);
        AudioManager.Instance.SetBGM(7);
        for (int i = 0; i < changeBGMButtons.Length; i++)
        {
            if (i != num)
                changeBGMButtons[i].sprite = buttonFlame[0];
            else
                changeBGMButtons[i].sprite = buttonFlame[1];
        }
    }

    //タイトルへ戻る
    public void RetrunToTitle()
    {
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        AudioManager.Instance.SE(0);
        AudioManager.Instance.StopBGM(1);
        _fade.FadeOut();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("SampleScene");
    }

    private void OnEnable()
    {
        _BGMVol = PlayerPrefs.GetFloat("BGMVolume", 0);
        BGMSlider.value = _BGMVol;
        _SEVol = PlayerPrefs.GetFloat("SEVolume", 0);
        SESlider.value = _SEVol;
    }

    private void OnDisable()
    {
        isFirstTime = true;
    }
}
