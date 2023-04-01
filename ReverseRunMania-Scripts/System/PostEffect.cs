using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PostEffect : MonoBehaviour
{
    [SerializeField] PostProcessVolume postProcessVolume;
    private ChromaticAberration chromatic;
    private Vignette vignette;
    private DepthOfField depth;

    [SerializeField] Image _fade;

    private Coroutine coroutine;
    private Coroutine fadeCoroutine;

    private GameObject Player;
    private float MaxPos;

    void Start()
    {
        chromatic = postProcessVolume.profile.GetSetting<ChromaticAberration>();
        vignette = postProcessVolume.profile.GetSetting<Vignette>();
        depth = postProcessVolume.profile.GetSetting<DepthOfField>();
        Player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        if (MaxPos > Player.transform.localPosition.x) return;
        MaxPos = Player.transform.localPosition.x;
    }

    public void DrunkEffect()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine("drunkEffect");
    }

    private IEnumerator drunkEffect()
    {
        AudioManager.instance.drunkBGM(0);
        chromatic.intensity.value = 1f;
        vignette.intensity.value = 0.4f;
        depth.focusDistance.value = 1f;
        depth.aperture.value = 10f;

        yield return new WaitForSeconds(2f);
        var wait = new WaitForSeconds(0.1f);
        for(int i = 0; i < 30; i++)
        {
            chromatic.intensity.value -= 1f / 30f;
            depth.aperture.value += 0.5f;
            yield return wait;
        }

        AudioManager.instance.drunkBGM(1);
        chromatic.intensity.value = 0f;
        vignette.intensity.value = 0f;
        depth.focusDistance.value = 10f;

        yield break;
    }

    private IEnumerator FadeOut()
    {
        _fade.color = new Color(0, 0, 0, 0);
        _fade.gameObject.SetActive(true);
        vignette.intensity.value = 0f;
        yield return null;

        Color col = new Color(0, 0, 0, 1 / 90f);
        for (int i = 0; i < 90; i++)
        {
            _fade.color += col;
            vignette.intensity.value += 0.005f;
            yield return null;
        }
        vignette.intensity.value = 0f;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().SetCurrentState(GameManager.GameState.Result);
        Time.timeScale = 1f;
        Player.SetActive(false);
        yield break;
    }

    private IEnumerator FadeIn()
    {
        _fade.color = new Color(0, 0, 0, 1);
        _fade.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        
        Color col = new Color(0, 0, 0, 1 / 30f);
        for (int i = 0; i < 30; i++)
        {
            _fade.color = _fade.color - col;
            yield return null;
            yield return null;
        }
        _fade.gameObject.SetActive(false);
        yield break;
    }

    private IEnumerator FadeOutToIn()
    {
        _fade.color = new Color(0, 0, 0, 0);
        _fade.gameObject.SetActive(true);
        vignette.intensity.value = 0f;
        yield return new WaitForSeconds(0.1f);
        
        Color col = new Color(0, 0, 0, 1 / 60f);
        for (int i = 0; i < 60; i++)
        {
            _fade.color += col;
            vignette.intensity.value += 0.01f;
            yield return null;
        }

        _fade.color = new Color(0, 0, 0, 1);
        vignette.intensity.value = 10f;
        
        yield return new WaitForSeconds(0.25f);
        vignette.intensity.value = 0;

        Player.transform.position = new Vector3(MaxPos, 0.03f, 6.85f);
        Player.transform.rotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));

        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < 60; i++)
        {
            _fade.color -= col;
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
                if (fadeCoroutine == null)
                    fadeCoroutine = StartCoroutine(FadeOut());
                break;
            case 1:
                if (fadeCoroutine == null)
                    fadeCoroutine = StartCoroutine(FadeIn());
                break;
            case 2:
                if (fadeCoroutine == null)
                    fadeCoroutine = StartCoroutine("FadeOutToIn");
                break;
            default:
                Debug.Log("Fadeの数字が違います");
                break;
        }
    }
}
