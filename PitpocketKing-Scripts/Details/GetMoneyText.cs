using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetMoneyText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    //数値が変わる時に徐々に変わるため用
    public Action OnComplete = null;//終わった時のコールバック

    private float speed;
    private float number;
    private float targetNumber;

    private Coroutine playCoroutine = null;

    private AudioManager _audio;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.text = "獲得金額:0円";

        _audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // 今の値から to_number に徐々に移行
    public void SlideToNumber(int to_number, float duration)
    {
        targetNumber = to_number;
        speed = ((targetNumber - number) / duration);

        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
        }
        playCoroutine = StartCoroutine("slideTo");
    }

    private IEnumerator slideTo()
    {
        yield return new WaitForSeconds(0.25f);
        if (number < targetNumber)
            _audio.SE17();
        else
            _audio.SE20();
        while (true)
        {
            var delta = speed * Time.deltaTime;
            var next_number = number + delta;
            _text.text = "獲得金額:" + ((int)next_number).ToString() + "円";

            number = next_number;

            if (UnityEngine.Mathf.Sign(speed) * (targetNumber - number) <= 0.0f)
            {
                break;
            }
            yield return null;
        }
        playCoroutine = null; ;
        number = targetNumber;
        _text.text = "獲得金額:" + ((int)number).ToString() + "円";
        if (OnComplete != null)
        {
            OnComplete();
            OnComplete = null;
        }
    }
}
