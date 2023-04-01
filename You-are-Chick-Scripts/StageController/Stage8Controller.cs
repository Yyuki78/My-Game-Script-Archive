using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage8Controller : MonoBehaviour, IClear
{
    [SerializeField] float elapsedTime;

    [SerializeField] Fade _fade;

    [Header("Clear")]
    [SerializeField] GameObject Rock;
    [SerializeField] GameObject ClearArea;

    [SerializeField] ClearPanel clearPanel;
    [SerializeField] GameObject ClearParticle;
    [SerializeField] HintSystem _hint;
    [SerializeField] HiyokoInfomation _info;
    [SerializeField] GameObject SettingPanel;

    void Start()
    {
        Application.targetFrameRate = 60;
        ClearArea.SetActive(false);
        _fade.FadeOut(0.75f);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public void GateOpen()
    {
        AudioManager.instance.SE(1);
        Rock.SetActive(false);
        ClearArea.SetActive(true);
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
        if (!GameManager.isClearStage[7])
        {
            GameManager.isClearStage[7] = true;
            clearPanel.isFirstClear = true;
        }
    }
}
