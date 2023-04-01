using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4Controller : MonoBehaviour, IClear
{
    [SerializeField] float elapsedTime;

    [SerializeField] Fade _fade;

    [SerializeField] GameObject[] Character;

    [Header("Clear")]
    [SerializeField] GameObject Rock;
    [SerializeField] Transform[] Gate = new Transform[2];
    private Coroutine coroutine;
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

    public void ActiveCharacter(int[] num)
    {
        for(int i = 0; i < num.Length; i++)
        {
            Character[num[i]].SetActive(true);
        }
        for(int i = 0; i < Character.Length; i++)
        {
            if (!Character[i].activeSelf) return;
        }
        GateOpen();
    }

    private void GateOpen()
    {
        StartCoroutine(GateOpenEffect());
    }

    private IEnumerator GateOpenEffect()
    {
        AudioManager.instance.SE(1);
        yield return null;
        Rock.SetActive(false);
        ClearArea.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.SE(2);
        var wait = new WaitForSeconds(0.1f);
        Vector3 pos = new Vector3(0, 0.05f, 0);
        for (int i = 0; i < 50; i++)
        {
            Gate[0].position += pos;
            Gate[1].position -= pos;
            yield return wait;
        }
        yield break;
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
        if (!GameManager.isClearStage[3])
        {
            GameManager.isClearStage[3] = true;
            clearPanel.isFirstClear = true;
        }
    }
}