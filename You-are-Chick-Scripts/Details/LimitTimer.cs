using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LimitTimer : MonoBehaviour
{
    private float elapsedTime;
    private Stage10Controller _controller;
    private TextMeshProUGUI _text;

    void Start()
    {
        _controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Stage10Controller>();
        _text = GetComponent<TextMeshProUGUI>();
    }
    
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (30f - elapsedTime <= 0)
        {
            _text.text = "0.00";
            _controller.GameOver();
        }
        else
        {
            _text.text = (30f - elapsedTime).ToString("f2");
        }
    }
}
