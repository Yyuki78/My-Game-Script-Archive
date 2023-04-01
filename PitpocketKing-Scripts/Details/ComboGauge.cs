using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboGauge : MonoBehaviour
{
    public int comboNum = 0;
    
    private bool isMinus = false;

    private Image _image;
    private float fillAmount;

    [SerializeField] GameObject comboNowText;

    //数値が変わる時に徐々に変わるため用
    public Action OnComplete = null;//終わった時のコールバック

    private float speed;
    private float targetNumber;
    private Coroutine playCoroutine = null;

    private AudioManager _audio;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        fillAmount = 0;
        comboNowText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isEnd) return;
        _image.fillAmount = fillAmount;
        if (fillAmount >= 1)
        {
            _audio.StopSE9();
            isMinus = true;
        }
        if (isMinus)
        {
            fillAmount -= 0.0015f;
            _audio.ChangeBGMpitch(1.1f);
        }
        if (fillAmount <= 0)
        {
            isMinus = false;
            fillAmount = 0;
            comboNum = 0;
            _audio.ChangeBGMpitch(1.0f);
        }

        if (comboNum > 1)
            comboNowText.SetActive(true);
        else
            comboNowText.SetActive(false);
    }

    public void ComboDecide()
    {
        _audio.SE9();

        isMinus = false;
        comboNum++;

        targetNumber = 1;
        speed = ((targetNumber - fillAmount) / 1f);

        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
        }
        playCoroutine = StartCoroutine("slideTo");
    }

    private IEnumerator slideTo()
    {
        while (true)
        {
            var delta = speed * Time.deltaTime;
            var next_number = fillAmount + delta;
            fillAmount = next_number;

            if (UnityEngine.Mathf.Sign(speed) * (targetNumber - fillAmount) <= 0.0f)
            {
                break;
            }
            yield return null;
        }
        playCoroutine = null;
        fillAmount = targetNumber;
        if (OnComplete != null)
        {
            OnComplete();
            OnComplete = null;
        }
    }

    public void Reset()
    {
        isMinus = false;
        fillAmount = 0;
        comboNum = 0;
        comboNowText.SetActive(false);
        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
        }
    }
}
