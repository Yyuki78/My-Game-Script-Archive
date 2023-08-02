using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    //このゲーム開始時の演出とステージ選択後のゲーム開始時の設定を行う
    [SerializeField] TextMeshProUGUI _titleText;
    private string[] titleArray;
    private string titleWords;
    private StringBuilder sb = new StringBuilder();

    [SerializeField] GameObject[] InActiveObject;
    [SerializeField] GameObject[] Panels;
    [SerializeField] GameObject TitleTexts;

    private bool once = true;
    [SerializeField] OVRScreenFade _fade;

    [SerializeField] GameObject[] StagePrefab;
    private StageSelectManager _stageSelectManager;
    private OptionManager _optionManager;
    private BattlePracticeManager _practice;

    private int stageNum;

    [SerializeField] GameObject Tutorial;

    void Awake()
    {
        _stageSelectManager = GetComponent<StageSelectManager>();
        _optionManager = GetComponent<OptionManager>();
        _practice = GetComponentInParent<BattlePracticeManager>();

        titleWords = "F,i,e,l,d, ,o,f,\n,R,e,a,l,m, ,T,a,c,t,i,c,s";
        titleWords = "F,a,n,t,a,s,y,\n,T,a,v,e,r,n, ,T,a,c,t,i,c,s";
        titleArray = titleWords.Split(',');

        for (int i = 0; i < InActiveObject.Length; i++)
        {
            InActiveObject[i].SetActive(false);
        }

        if (GameInfomation.isConsecutivePlay)
            StartCoroutine(ConsecutivePlayGame());
        else
            StartCoroutine(TitleEffect());
    }

    WaitForSeconds wait = new WaitForSeconds(0.1f);
    WaitForSeconds wait2 = new WaitForSeconds(1f);
    private IEnumerator TitleEffect()
    {
        yield return wait2;
        foreach (var p in titleArray)
        {
            sb.Append(p);
            _titleText.text = sb.ToString();
            AudioManager.Instance.SE(25);
            yield return wait;
        }
        yield return wait2;
        if (!GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>().isFirstActivation)
        {
            for (int i = 0; i < InActiveObject.Length; i++)
                InActiveObject[i].SetActive(true);
            AudioManager.Instance.SetBGM(0);
        }
        else
            ShowTutorial(true);
        yield break;
    }

    private IEnumerator ConsecutivePlayGame()
    {
        TitleTexts.SetActive(false);
        yield return wait;
        GameInfomation.isConsecutivePlay = false;
        stageNum = PlayerPrefs.GetInt("isStageNumber", 0);
        switch (stageNum)
        {
            case 51:
                Instantiate(StagePrefab[0], transform.parent.transform);
                break;
            case 52:
                Instantiate(StagePrefab[1], transform.parent.transform);
                break;
            case 53:
                Instantiate(StagePrefab[6], transform.parent.transform);
                break;
            case 61:
                Instantiate(StagePrefab[2], transform.parent.transform);
                break;
            case 62:
                Instantiate(StagePrefab[3], transform.parent.transform);
                break;
            case 63:
                Instantiate(StagePrefab[7], transform.parent.transform);
                break;
            case 71:
                Instantiate(StagePrefab[4], transform.parent.transform);
                break;
            case 72:
                Instantiate(StagePrefab[5], transform.parent.transform);
                break;
            case 73:
                Instantiate(StagePrefab[8], transform.parent.transform);
                break;
            case 74:
                Instantiate(StagePrefab[9], transform.parent.transform);
                break;
            case 75:
                Instantiate(StagePrefab[10], transform.parent.transform);
                break;
        }
        Destroy(gameObject);
    }

    public void ShowTutorial(bool type = false)
    {
        if(!type)
            AudioManager.Instance.SE(0);
        AudioManager.Instance.SetBGM(1);
        for (int i = 0; i < InActiveObject.Length; i++)
            InActiveObject[i].SetActive(false);
        for (int i = 0; i < Panels.Length; i++)
            Panels[i].SetActive(false);
        var tutorial = Instantiate(Tutorial, transform.parent);
        tutorial.GetComponent<TutorialManager>()._titleManager = this;
    }

    public void FinishTutorial()
    {
        for (int i = 0; i < InActiveObject.Length; i++)
            InActiveObject[i].SetActive(true);
        AudioManager.Instance.SetBGM(0);
    }

    public void GameStartButton()
    {
        //矢印を消す+現在のボードをStageSelectManagerから取得
        stageNum = _stageSelectManager.GameStartEffect();
        if (stageNum == 1)
        {
            _optionManager.GameStartEffect(true);
            _practice.GoAttackScene();
        }
        else
        {
            if (stageNum == 100)//不正(課金していないのに課金ステージが選択されている)
                return;
            if (!once) return;
            once = false;
            StartCoroutine(GameStartEffect());
        }
        AudioManager.Instance.StopBGM(1);
    }

    private IEnumerator GameStartEffect()
    {
        AudioManager.Instance.SE(3);
        yield return null;
        //オプションパネルの当たり判定が消える
        _optionManager.GameStartEffect();
        _fade.FadeOut();
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < InActiveObject.Length; i++)
            InActiveObject[i].SetActive(false);
        TitleTexts.SetActive(false);
        yield return wait;
        _fade.FadeIn();
        yield return wait;
        switch (stageNum)
        {
            case 51:
                Instantiate(StagePrefab[0], transform.parent.transform);
                break;
            case 52:
                Instantiate(StagePrefab[1], transform.parent.transform);
                break;
            case 53:
                Instantiate(StagePrefab[6], transform.parent.transform);
                break;
            case 61:
                Instantiate(StagePrefab[2], transform.parent.transform);
                break;
            case 62:
                Instantiate(StagePrefab[3], transform.parent.transform);
                break;
            case 63:
                Instantiate(StagePrefab[7], transform.parent.transform);
                break;
            case 71:
                Instantiate(StagePrefab[4], transform.parent.transform);
                break;
            case 72:
                Instantiate(StagePrefab[5], transform.parent.transform);
                break;
            case 73:
                Instantiate(StagePrefab[8], transform.parent.transform);
                break;
            case 74:
                Instantiate(StagePrefab[9], transform.parent.transform);
                break;
            case 75:
                Instantiate(StagePrefab[10], transform.parent.transform);
                break;
        }
        yield return wait2;
        Destroy(gameObject);
        yield break;
    }
}
