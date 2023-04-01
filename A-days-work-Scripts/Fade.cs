using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;

public class Fade : MonoBehaviour
{
    public int mode = 0;

    [SerializeField] Image _fade;

    [SerializeField] private Volume volume = null;
    private Vignette _vignette;

    //ゲーム開始用
    [SerializeField] GameObject Player;
    [SerializeField] GameObject TitleStage;
    [SerializeField] TaskManager _manager;
    [SerializeField] GameObject TaskClearText;

    private void Awake()
    {
        if (volume.profile.TryGet(out Vignette vignette))
        {
            _vignette = vignette;
        }
        _fade.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        _fade.color = new Color(0, 0, 0, 0);
        _fade.gameObject.SetActive(true);
        _vignette.intensity.value = 0f;
        yield return new WaitForSeconds(0.1f);

        var wait = new WaitForSeconds(0.01f);
        Color col= new Color(0, 0, 0, 1 / 51f);
        for (int i = 0; i < 51; i++)
        {
            _fade.color += col;
            _vignette.intensity.value += 0.01f;
            yield return wait;
        }
        _vignette.intensity.value = 0f;

        SceneManager.LoadScene("SampleGameScene");
        yield break;
    }

    private IEnumerator FadeIn()
    {
        _fade.color = new Color(0, 0, 0, 1);
        _fade.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        
        Color col = new Color(0, 0, 0, 1 / 120f);
        for (int i = 0; i < 120; i++)
        {
            _fade.color = _fade.color - col;
            yield return null;
        }
        _fade.gameObject.SetActive(false);
        yield break;
    }


    public void changeScene(int type)
    {
        switch (type)
        {
            case 0:
                StartCoroutine(FadeOut());
                break;
            case 1:
                StartCoroutine(FadeIn());
                break;
            case 2:
                StartCoroutine(FadeOutToIn());
                break;
            case 3:
                StartCoroutine(SimpleFadeOutToIn());
                break;
            default:
                Debug.Log("Fadeの数字が違います");
                break;
        }
    }

    private IEnumerator FadeOutToIn()
    {
        _fade.color = new Color(0, 0, 0, 0);
        _fade.gameObject.SetActive(true);
        _vignette.intensity.value = 0f;
        yield return new WaitForSeconds(0.1f);

        var wait = new WaitForSeconds(0.01f);
        Color col = new Color(0, 0, 0, 1 / 51f);
        for (int i = 0; i < 51; i++)
        {
            _fade.color += col;
            _vignette.intensity.value += 0.01f;
            yield return wait;
        }

        _fade.color = new Color(0, 0, 0, 1);
        _vignette.intensity.value = 10f;

        TaskClearText.SetActive(true);
        _manager.SetTask();
        yield return new WaitForSeconds(0.25f);
        _vignette.intensity.value = 0;

        Player.transform.localPosition = new Vector3(0, 0, 7.6f);
        TitleStage.SetActive(false);
        TaskClearText.SetActive(false);
        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < 51; i++)
        {
            _fade.color -= col;
            yield return wait;
        }
        _fade.gameObject.SetActive(false);
        yield break;
    }

    private IEnumerator SimpleFadeOutToIn()
    {
        _fade.color = new Color(0, 0, 0, 0);
        _fade.gameObject.SetActive(true);
        _vignette.intensity.value = 0f;
        yield return new WaitForSeconds(0.1f);

        var wait = new WaitForSeconds(0.01f);
        Color col = new Color(0, 0, 0, 1 / 51f);
        for (int i = 0; i < 51; i++)
        {
            _fade.color += col;
            _vignette.intensity.value += 0.01f;
            yield return wait;
        }

        _fade.color = new Color(0, 0, 0, 1);
        _vignette.intensity.value = 10f;

        yield return new WaitForSeconds(0.25f);
        _vignette.intensity.value = 0;
        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < 51; i++)
        {
            _fade.color -= col;
            yield return wait;
        }
        _fade.gameObject.SetActive(false);
        yield break;
    }
}
