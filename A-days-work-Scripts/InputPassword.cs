using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InputPassword : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private OpenBox _box;

    private int digitNum = 3;
    private int[] pass = new int[4];

    private AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _box = GetComponentInParent<OpenBox>();
        _audio = GetComponent<AudioSource>();
        for (int i = 0; i < 4; i++)
        {
            pass[i] = 0;
        }
    }

    public void SendChoice(int num)
    {
        if (digitNum < 0)
            return;

        _audio.PlayOneShot(_audio.clip);

        pass[digitNum] = num;

        int inputNum = pass[3] * 1000 + pass[2] * 100 + pass[1] * 10 + pass[0];
        _text.text = inputNum.ToString("0000");

        if (digitNum == 0)
        {
            _box.JudgePassword(inputNum);

            digitNum = 3;
        }
        else
        {
            digitNum--;
        }
    }

    public void ResetPass()
    {
        for (int i = 0; i < 4; i++)
        {
            pass[i] = 0;
        }
        _text.text = "0000";
    }
}