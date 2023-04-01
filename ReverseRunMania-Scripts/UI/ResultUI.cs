using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    string[] ranking = { "ランキング1位", "ランキング2位", "ランキング3位", "ランキング4位", "ランキング5位" };
    float[] rankingValue = new float[5];
    [SerializeField] TextMeshProUGUI[] rankingText = new TextMeshProUGUI[2];

    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _myRankText;
    private int rankNum = 0;
    [SerializeField] GameObject achieveText;

    [SerializeField] AchieveNotification _achieve;
    [SerializeField] MileageText _mileage;

    void Start()
    {
        AudioManager.instance.StopAllSounds();
        AudioManager.instance.BGM(2);
        _scoreText.text = _mileage.mileage.ToString("f2") + "km";
        GetRanking();
        SetRanking(_mileage.mileage);
        if (rankNum != 0)
            _myRankText.text = "順位:" + rankNum + "位";
        else
            _myRankText.text = "順位:圏外";

        if (_achieve.isAchieve)
            achieveText.SetActive(true);

        for (int i = 0; i < ranking.Length; i++)
            rankingText[i].text = rankingValue[i].ToString("f2") + "km";
    }

    private void GetRanking()
    {
        for (int i = 0; i < ranking.Length; i++)
        {
            rankingValue[i] = PlayerPrefs.GetFloat(ranking[i]);
        }
    }

    private void SetRanking(float _value)
    {
        for (int i = 0; i < ranking.Length; i++)
        {
            if (_value > rankingValue[i])
            {
                rankNum = i + 1;
                break;
            }
        }
        for (int i = 0; i < ranking.Length; i++)
        {
            if (_value > rankingValue[i])
            {
                var change = rankingValue[i];
                rankingValue[i] = _value;
                _value = change;
            }
        }
        
        for (int i = 0; i < ranking.Length; i++)
        {
            PlayerPrefs.SetFloat(ranking[i], rankingValue[i]);
        }
    }
}
