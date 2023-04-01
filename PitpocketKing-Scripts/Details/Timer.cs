using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private GameObject GameManagerObj;
    private GameManager _manager;

    private TextMeshProUGUI _text;

    private int currentHours;
    private float currentMinutes;

    // Start is called before the first frame update
    void Start()
    {
        GameManagerObj = GameObject.FindGameObjectWithTag("GameManager");
        _manager = GameManagerObj.GetComponent<GameManager>();
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        currentMinutes = _manager.elapsedTime * 4;
        currentHours = 12 + (int)currentMinutes / 60;
        _text.text = currentHours + ":" + (currentMinutes % 60).ToString("00");
    }
}
