using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ChangeLanguageButton : MonoBehaviour
{
    [SerializeField] GameObject CheckMarkImage;
    [SerializeField] TaskTextManager _manager;
    [SerializeField] ChangeTextLanguage _text;

    [SerializeField] Fade _fade;
    [SerializeField] AudioSource _audio;

    void Start()
    {
        StartCoroutine(wait());
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.05f);

        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            CheckMarkImage.transform.localPosition = new Vector3(-181.1f, 58f, 0);
        }
        else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            CheckMarkImage.transform.localPosition = new Vector3(-181.1f, 87f, 0);
        }
        _manager.set();
        //_text.ChangeText();

        yield return new WaitForSeconds(0.05f);
        if (PlayerPrefs.HasKey("Language"))
        {
            if (PlayerPrefs.GetInt("Language") == 1)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                CheckMarkImage.transform.localPosition = new Vector3(-181.1f, 58f, 0);
            }
            else
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
                CheckMarkImage.transform.localPosition = new Vector3(-181.1f, 87f, 0);
            }
            _manager.set();
        }
        yield return null;
        yield break;
    }

    public void OnClickChangeLanguage(int num)
    {
        _audio.PlayOneShot(_audio.clip);
        _fade.changeScene(1);
        switch (num)
        {
            case 0:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
                CheckMarkImage.transform.localPosition = new Vector3(-181.1f, 87f, 0);
                _manager.set();
                PlayerPrefs.SetInt("Language", 0);
                break;
            case 1:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                CheckMarkImage.transform.localPosition = new Vector3(-181.1f, 58f, 0);
                _manager.set();
                PlayerPrefs.SetInt("Language", 1);
                break;
        }
        //_text.ChangeText();
    }
}
