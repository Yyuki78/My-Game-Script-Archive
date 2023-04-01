using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultTime : MonoBehaviour
{
    private int highScore;
    public Text resultTime;
    public Text bestTime;
    [SerializeField] GameObject gameClear;//ゲームクリア画面
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }
        else
        {
            highScore = 999;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameClear.activeSelf == true)
        {
            int result = Mathf.FloorToInt(Timer.time);
            resultTime.text = "クリアタイム:" + result;
            bestTime.text = "ベストタイム:" + highScore;

            if (highScore > result)
            {
                PlayerPrefs.SetInt("HighScore", result);
            }
        }
        //Debug.Log(PlayerPrefs.GetInt("HighScore"));
    }
}
