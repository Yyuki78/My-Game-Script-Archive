using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MileageText : MonoBehaviour
{
    private GameObject Player;
    private TextMeshProUGUI _text;
    private float MaxPos;
    public float mileage { get { return MaxPos / 200f; } }

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (MaxPos > Player.transform.localPosition.x || GameManager.isGameOver) return;
        MaxPos = Player.transform.localPosition.x;
        _text.text = (MaxPos / 200f).ToString("f2") + "km";
    }
}
