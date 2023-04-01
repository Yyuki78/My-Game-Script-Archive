using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage9Controller : MonoBehaviour, IClear
{
    [SerializeField] float elapsedTime;

    [SerializeField] int spaceNum = 0;
    [SerializeField] PopupText[] spaceEffect = new PopupText[5];
    private bool canSpaceInput = true;

    [SerializeField] int inputNum = 0;
    [SerializeField] bool[] isInput = new bool[12];
    [SerializeField] string[] key = new string[12];
    [SerializeField] GameObject[] Key;
    [SerializeField] PopupText Miss;

    [SerializeField] Transform Caret;
    private bool playCoroutine = false;
    private bool isCaretActive = true;
    [SerializeField] Transform TitleText;
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
        Miss.gameObject.SetActive(false);
        Caret.gameObject.SetActive(false);
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

        //Cを指定位置以外で押すとバツが出る
        if (Input.GetKeyDown("c") && spaceNum != 3 && canSpaceInput)
        {
            AudioManager.instance.SE(3);
            Miss.gameObject.SetActive(true);
            Miss.popUpText();
            Miss.transform.localPosition = new Vector3(-250 + spaceNum * 40, 0, 0);
        }

        //Spaceは何回でも押せる
        if (Input.GetKeyDown("space") && canSpaceInput && spaceNum < 5)
        {
            TitleText.localPosition += new Vector3(40, 0, 0);
            Caret.localPosition += new Vector3(40, 0, 0);
            spaceEffect[spaceNum].popUpText();
            spaceNum++;
        }

        //BackSpaceがDeleteで空白は消せる
        if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
        {
            if (!canSpaceInput) return;
            if (spaceNum <= 0) return;
            TitleText.localPosition -= new Vector3(40, 0, 0);
            Caret.localPosition -= new Vector3(40, 0, 0);
            spaceNum--;
        }

        if (Input.GetKeyDown(key[inputNum]) && spaceNum == 3)
        {
            if (isInput[inputNum]) return;
            isInput[inputNum] = true;
            AudioManager.instance.SE(14);
            canSpaceInput = false;
            isCaretActive = false;
            Key[inputNum].SetActive(true);
            inputNum++;
        }
    }

    public void ClickText()
    {
        if(!playCoroutine)
            StartCoroutine(CaretEffect());
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

    public void GateOpen()
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
        if (!GameManager.isClearStage[8])
        {
            GameManager.isClearStage[8] = true;
            clearPanel.isFirstClear = true;
        }
    }
}