using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3Controller : MonoBehaviour, IClear
{
    [SerializeField] float elapsedTime;

    [SerializeField] Fade _fade;

    [Header("Clear")]
    [SerializeField] ClearPanel clearPanel;
    [SerializeField] GameObject ClearParticle;
    [SerializeField] HintSystem _hint;
    [SerializeField] HiyokoInfomation _info;
    [SerializeField] GameObject SettingPanel;

    void Start()
    {
        Application.targetFrameRate = 60;
        _fade.FadeOut(0.75f);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public void Clear()
    {
        clearPanel.gameObject.SetActive(true);
        clearPanel.clearTime = elapsedTime;
        ClearParticle.SetActive(true);
        _hint.stopHint();
        _info.SetState(HiyokoInfomation.State.Finish);
        SettingPanel.SetActive(false);
        AudioManager.instance.BGM(3);
        if (!GameManager.isClearStage[2])
        {
            GameManager.isClearStage[2] = true;
            clearPanel.isFirstClear = true;
        }
    }
}
