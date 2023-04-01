using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    [SerializeField] Fade _fade;
    [SerializeField] GameObject DictionaryPanel;
    [SerializeField] GameObject OptionPanel;
    [SerializeField] GameObject ConfirmPanel;

    public void OnClickButton(int num)
    {
        switch (num)
        {
            case 0:
                DictionaryPanel.SetActive(true);
                break;
            case 1:
                DictionaryPanel.SetActive(false);
                OptionPanel.SetActive(false);
                break;
            case 2:
                _fade.FadeIn(1.0f, loadMain);
                break;
            case 3:
                _fade.FadeIn(1.0f, loadTitle);
                break;
            case 4:
                OptionPanel.SetActive(true);
                break;
            case 5:
                PlayerPrefs.DeleteAll();
                _fade.FadeIn(1.0f, loadTitle);
                break;
            case 6:
                Application.Quit();
                break;
            case 7:
                ConfirmPanel.SetActive(true);
                break;
            case 8:
                ConfirmPanel.SetActive(false);
                break;
            default:
                break;
        }
        Time.timeScale = 1f;
    }

    private void loadMain()
    {
        AudioManager.instance.StopAllSounds();
        SceneManager.LoadScene("Main");
    }

    private void loadTitle()
    {
        AudioManager.instance.StopAllSounds();
        SceneManager.LoadScene("Title");
    }
}
