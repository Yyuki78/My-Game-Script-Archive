using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    [SerializeField] Fade _fade;
    [SerializeField] GameObject DictionaryPanel;
    [SerializeField] GameObject OptionPanel;
    [SerializeField] GameObject SettingPanel;
    [SerializeField] string nextScene;

    private void Start()
    {
        AudioManager.instance.Init();
    }

    public void OnClickButton(int num)
    {
        if (num != 10)
            AudioManager.instance.SE(13);
        switch (num)
        {
            case 0:
                DictionaryPanel.SetActive(true);
                break;
            case 1:
                _fade.FadeIn(0.75f, loadThis);
                break;
            case 2:
                _fade.FadeIn(0.75f, loadNext);
                break;
            case 3:
                _fade.FadeIn(0.75f, loadHome);
                break;
            case 4:
                OptionPanel.SetActive(true);
                break;
            case 5:
                PlayerPrefs.DeleteAll();
                _fade.FadeIn(1.0f, loadHome);
                break;
            case 6:
                Application.Quit();
                break;
            case 7:
                SettingPanel.SetActive(true);
                break;
            case 8:
                SettingPanel.SetActive(false);
                break;
            case 9:
                _fade.FadeIn(0.75f, loadTitle);
                break;
            case 10:
                AudioManager.instance.guitarSE();
                break;
            default:
                break;
        }
        Time.timeScale = 1f;
    }

    public void setNextScene(string sceneName)
    {
        nextScene = sceneName;
    }

    private void loadNext()
    {
        AudioManager.instance.StopAllSounds();
        SceneManager.LoadScene(nextScene);
    }

    private void loadThis()
    {
        AudioManager.instance.StopAllSounds();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void loadHome()
    {
        AudioManager.instance.StopAllSounds();
        SceneManager.LoadScene("Home");
    }

    private void loadTitle()
    {
        AudioManager.instance.StopAllSounds();
        SceneManager.LoadScene("Title");
    }
}