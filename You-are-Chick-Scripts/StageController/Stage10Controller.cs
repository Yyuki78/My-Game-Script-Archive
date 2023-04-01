using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage10Controller : MonoBehaviour
{
    [SerializeField] float elapsedTime;

    [SerializeField] int SwipeNum = 0;
    private int Maxnum = 62;

    [SerializeField] Fade _fade;
    [SerializeField] GameObject GameOverPanel;
    private bool once = true;

    [Header("Clear")]
    [SerializeField] ClearPanel clearPanel;
    [SerializeField] GameObject ClearParticle;
    [SerializeField] HintSystem _hint;
    [SerializeField] HiyokoInfomation _info;

    void Start()
    {
        Application.targetFrameRate = 60;
        _fade.FadeOut(0.75f);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 60f)
            Maxnum = 63;
    }

    public void SwipeObj()
    {
        SwipeNum++;
        if (SwipeNum >= Maxnum)
        {
            Clear();
        }
    }

    public void Clear()
    {
        clearPanel.gameObject.SetActive(true);
        clearPanel.clearTime = elapsedTime;
        ClearParticle.SetActive(true);
        _hint.stopHint();
        AudioManager.instance.BGM(3);
        if (!GameManager.isClearStage[9])
        {
            GameManager.isClearStage[9] = true;
            clearPanel.isFirstClear = true;
        }
    }

    public void GameOver()
    {
        if (!once) return;
        once = false;
        GameOverPanel.SetActive(true);
        AudioManager.instance.BGM(4);
    }
}
