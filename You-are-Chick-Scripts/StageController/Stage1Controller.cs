using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Controller : MonoBehaviour, IClear
{
    [SerializeField] float elapsedTime;

    [SerializeField] int inputNum = 0;
    [SerializeField] bool[] isInput = new bool[5];
    [SerializeField] string[] key = new string[5];
    [SerializeField] GameObject[] Key;

    [SerializeField] GameObject Caret;
    private bool playCoroutine = false;
    private bool isCaretActive = true;
    [SerializeField] Fade _fade;

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
        Caret.SetActive(false);
        ClearArea.SetActive(false);
        for (int i = 0; i < isInput.Length; i++)
        {
            isInput[i] = false;
            Key[i].SetActive(false);
        }
        _fade.FadeOut(0.75f);
    }
    
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (inputNum >= isInput.Length)
        {
            if (coroutine == null)
                coroutine = StartCoroutine(GateOpenEffect());
            return;
        }
        if (Input.GetKey(key[inputNum]))
        {
            if (isInput[inputNum]) return;
            isInput[inputNum] = true;
            AudioManager.instance.SE(14);
            Key[inputNum].SetActive(true);
            isCaretActive = false;
            inputNum++;
        }
    }

    public void ClickText()
    {
        if (!playCoroutine)
            StartCoroutine(CaretEffect());
        isCaretActive = true;
    }

    private IEnumerator CaretEffect()
    {
        playCoroutine = true;
        var wait = new WaitForSeconds(0.75f);
        while (isCaretActive)
        {
            Caret.gameObject.SetActive(true);
            yield return wait;
            Caret.gameObject.SetActive(false);
            yield return wait;
        }
        playCoroutine = false;
        yield break;
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
        for(int i = 0; i < 50; i++)
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
        if (!GameManager.isClearStage[0])
        {
            GameManager.isClearStage[0] = true;
            clearPanel.isFirstClear = true;
        }
    }
}
