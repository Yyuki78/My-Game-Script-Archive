using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleRanking : MonoBehaviour
{
    string[] ranking = { "ランキング1位", "ランキング2位", "ランキング3位", "ランキング4位", "ランキング5位" };
    float[] rankingValue = new float[5];
    [SerializeField] TextMeshProUGUI[] rankingText = new TextMeshProUGUI[2];
    
    void Start()
    {
        for (int i = 0; i < ranking.Length; i++)
            rankingValue[i] = PlayerPrefs.GetFloat(ranking[i]);
        for (int i = 0; i < ranking.Length; i++)
            rankingText[i].text = rankingValue[i].ToString("f2") + "km";
    }
}
