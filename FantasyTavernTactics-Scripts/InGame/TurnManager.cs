using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.Localization.Settings;

public class TurnManager : MonoBehaviour
{
    public enum GameState
    {
        WaitStart,
        StartGame,
        MoveCountSelection,
        Move,
        Attack,
        Finish,
        EnemyMoveCountSelection,
        EnemyMove,
        EnemyAttack,
        EnemyFinish,
        Event,
        EnemyEvent,
        GameFinish
    }
    public GameState _currentState { private set; get; }

    [SerializeField] int TurnNum = 0;
    public int MoveCount { private set; get; }
    private int saveMoveNum = 0;//リセット用
    public bool canMove { private set; get; }
    private bool isNextPlusMoveCount = false;//1が出た次のターンの移動回数が全て+1の値になる

    [SerializeField] GameObject Board;
    [SerializeField] TextMeshProUGUI _turnText;
    [SerializeField] TextMeshProUGUI _phaseText;
    [SerializeField] TextMeshProUGUI _turnNumText;
    [SerializeField] TextMeshProUGUI _moveCountText;

    [SerializeField] GameObject[] Telops;
    private TextMeshProUGUI[] _telopTexts = new TextMeshProUGUI[5];

    [SerializeField] [TextArea] string[] TurnText;
    [SerializeField] [TextArea] string[] PhaseText;
    private string moveCountText;
    private string turnText;
    private string[] isWinText = new string[2];
    private ExplanationText _explanationText;

    private ThreeCupsGame _threeCupsGame;

    [SerializeField] GameObject MoveResetButton;
    [SerializeField] GameObject MoveFinishButton;
    [SerializeField] GameObject TurnFinishButton;
    [SerializeField] GameObject MoveFinishButton2;
    [SerializeField] GameObject TurnFinishButton2;

    [SerializeField] GameObject TurnContinueButton;
    [SerializeField] GameObject FinishDecisionButton;

    [SerializeField] TileManager _tileManager;
    [SerializeField] IAI _ai;

    private BuffTileManager _buffTileManager;
    private bool canPlusMoveTileEvent = false;
    private bool nextTurnPlusMoveCountAlly = false;
    private bool nextTurnPlusMoveCountEnemy = false;
    private bool canBuffAttackTileEvent = false;
    private bool canHealingTileEvent = false;

    private MysteryBoxManager _mysteryBoxManager;
    private bool canMysteryBoxEvent = false;

    private bool canStrongWindEvent = false;
    [SerializeField] ParticleSystem StrongWindParticle;
    [SerializeField] GameObject StrongWindText;

    [SerializeField] GameObject HalfwayText;
    [SerializeField] GameObject HalfwayEffect;
    [SerializeField] GameObject HalfwayExplanation;

    [SerializeField] TextMeshProUGUI GameFinishText;
    [SerializeField] GameObject WinEffectObj;
    [SerializeField] GameObject LoseEffectObj;

    private OVRScreenFade _fade;
    [SerializeField] GameObject BackTitleButton;
    [SerializeField] GameObject RetryButton;

    private PieceInfoDisplay _pieceInfoDisplay;
    [SerializeField] GameObject AttributeCorrelationChart;

    void Awake()
    {
        _explanationText = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExplanationText>();
        _threeCupsGame = GetComponentInChildren<ThreeCupsGame>();
        Board.SetActive(false);
        _turnText.gameObject.SetActive(false);
        _phaseText.gameObject.SetActive(false);
        _turnNumText.gameObject.SetActive(false);
        _moveCountText.gameObject.SetActive(false);

        MoveResetButton.gameObject.SetActive(false);
        MoveFinishButton.gameObject.SetActive(false);
        TurnFinishButton.gameObject.SetActive(false);
        MoveFinishButton2.gameObject.SetActive(false);
        TurnFinishButton2.gameObject.SetActive(false);

        TurnContinueButton.gameObject.SetActive(false);
        FinishDecisionButton.gameObject.SetActive(false);

        _buffTileManager = GetComponent<BuffTileManager>();
        canPlusMoveTileEvent = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>().isPlusMoveCountTile;
        canBuffAttackTileEvent = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>().isPowerBuffTile;
        canHealingTileEvent = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>().isHealingTile;

        _mysteryBoxManager = GetComponent<MysteryBoxManager>();
        canMysteryBoxEvent = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>().isAppearMysteryBox;

        canStrongWindEvent = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>().isStrongWind;

        StrongWindText.transform.localScale = new Vector3(1, 0, 1);
        StrongWindText.transform.localPosition = new Vector3(-1.145f, -0.05f, 0);
        StrongWindText.SetActive(false);

        HalfwayText.transform.localScale = new Vector3(1, 0, 1);
        HalfwayText.SetActive(false);
        HalfwayEffect.SetActive(false);
        HalfwayExplanation.SetActive(false);

        BackTitleButton.SetActive(false);
        RetryButton.SetActive(false);

        _pieceInfoDisplay = GetComponent<PieceInfoDisplay>();

        for (int i = 0; i < Telops.Length; i++)
        {
            _telopTexts[i] = Telops[i].GetComponent<TextMeshProUGUI>();
            _telopTexts[i].DOFade(1f, 0f);
            Telops[i].transform.localScale = new Vector3(1, 0, 1);
            Telops[i].SetActive(false);
        }
        _fade = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<OVRScreenFade>();
    }

    void Start()
    {
        SetState(GameState.WaitStart);
        for (int i = 0; i < TurnText.Length; i++)
            TurnText[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "TurnText" + (i + 1).ToString());
        for (int i = 0; i < PhaseText.Length; i++)
            PhaseText[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "Phase" + (i + 1).ToString());
        moveCountText = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "MoveCount");
        turnText = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "TurnNum");
        for (int i = 0; i < isWinText.Length; i++)
            isWinText[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "GameFinishText" + (i + 1).ToString());
    }

    public void SetState(GameState state)
    {
        _currentState = state;
        switch (state)
        {
            case GameState.WaitStart:
                break;
            case GameState.StartGame:
                //Debug.Log("ゲームを始めます");
                StartCoroutine(GameStart());
                break;
            case GameState.MoveCountSelection:
                AudioManager.Instance.SE(11);
                _turnText.text = TurnText[0];
                _phaseText.text = PhaseText[0];
                Telops[0].SetActive(true);
                Telops[0].transform.DOScaleY(1f, 1f).SetEase(Ease.InQuart);
                Telops[0].transform.DOLocalMoveY(0.15f, 3f).SetDelay(1.5f);
                _telopTexts[0].DOFade(0f, 3f).SetDelay(1.5f).OnComplete(() => { Telops[0].SetActive(false); });
                if (isNextPlusMoveCount)
                {
                    isNextPlusMoveCount = false;
                    _threeCupsGame.SetGame(true);
                }
                else 
                    _threeCupsGame.SetGame(false);
                break;
            case GameState.Move:
                _phaseText.text = PhaseText[1];
                Telops[1].SetActive(true);
                Telops[1].transform.DOScaleY(1f, 1f).SetEase(Ease.InQuart);
                Telops[1].transform.DOLocalMoveY(0.15f, 3f).SetDelay(1.5f);
                _telopTexts[1].DOFade(0f, 3f).SetDelay(1.5f)
                    .OnComplete(() => { Telops[1].SetActive(false);
                        if (_currentState == GameState.Move)
                        {
                            MoveResetButton.gameObject.SetActive(true);
                            MoveFinishButton.gameObject.SetActive(true);
                            MoveFinishButton2.gameObject.SetActive(true);
                        }
                    });
                _moveCountText.gameObject.SetActive(true);
                _moveCountText.text = "MoveCount:" + MoveCount.ToString();
                _explanationText.ChangeActive(true);
                _explanationText.ChangeText(6);
                break;
            case GameState.Attack:
                MoveResetButton.gameObject.SetActive(false);
                MoveFinishButton.gameObject.SetActive(false);
                MoveFinishButton2.gameObject.SetActive(false);
                _moveCountText.gameObject.SetActive(false);
                _phaseText.text = PhaseText[2];
                Telops[2].SetActive(true);
                Telops[2].transform.DOScaleY(1f, 1f).SetEase(Ease.InQuart);
                Telops[2].transform.DOLocalMoveY(0.15f, 3f).SetDelay(1.5f);
                _telopTexts[2].DOFade(0f, 3f).SetDelay(1.5f).OnComplete(() => { Telops[2].SetActive(false); });
                TurnFinishButton.SetActive(true);
                TurnFinishButton2.SetActive(true);
                _explanationText.ChangeText(7);
                break;
            case GameState.Finish:
                _tileManager.StopBlinking(1);
                AudioManager.Instance.SE(12);
                Telops[3].SetActive(true);
                Telops[3].transform.DOScaleY(1f, 1f).SetEase(Ease.InQuart);
                Telops[3].transform.DOLocalMoveY(0.15f, 3f).SetDelay(1.5f);
                _explanationText.ChangeActive(false);
                SetState(GameState.EnemyMoveCountSelection);
                break;
            case GameState.EnemyMoveCountSelection:
                _turnText.text = TurnText[1];
                _phaseText.text = PhaseText[3];
                if (_ai == null)
                    _ai = GetComponent<IAI>();
                _ai.StartMoveCountSelection();
                break;
            case GameState.EnemyMove:
                _phaseText.text = PhaseText[1];
                _moveCountText.gameObject.SetActive(true);
                _moveCountText.text = moveCountText + MoveCount.ToString();
                break;
            case GameState.EnemyAttack:
                _moveCountText.gameObject.SetActive(false);
                _phaseText.text = PhaseText[2];
                for (int i = 0; i < Telops.Length - 1; i++)
                {
                    _telopTexts[i] = Telops[i].GetComponent<TextMeshProUGUI>();
                    _telopTexts[i].DOFade(1f, 0f);
                    Telops[i].transform.localScale = new Vector3(1, 0, 1);
                    Telops[i].transform.position -= new Vector3(0, 0.15f, 0);
                    Telops[i].SetActive(false);
                }
                break;
            case GameState.EnemyFinish:
                Telops[3].transform.localScale = new Vector3(1, 0, 1);
                Telops[3].transform.position -= new Vector3(0, 0.15f, 0);
                Telops[3].SetActive(false);
                TurnNum++;
                _turnNumText.text = turnText + TurnNum;
                SetState(GameState.MoveCountSelection);
                break;
            case GameState.Event:
            case GameState.EnemyEvent:
                break;
            case GameState.GameFinish:
                break;
        }
    }

    WaitForSeconds wait = new WaitForSeconds(0.5f);
    private IEnumerator GameStart()
    {
        yield return wait;
        Board.SetActive(true);
        _turnText.gameObject.SetActive(true);
        _phaseText.gameObject.SetActive(true);
        _turnNumText.gameObject.SetActive(true);
        TurnNum++;
        _turnNumText.text = turnText + TurnNum;
        if (canPlusMoveTileEvent)
            _buffTileManager.SetBuffTile(false, 0);
        SetState(GameState.MoveCountSelection);
        yield return wait;
        AudioManager.Instance.SetBGM(7);
        yield break;
    }

    public void DecideMoveNum(int num)
    {
        canMove = true;
        MoveCount = num;
        if (_currentState == GameState.MoveCountSelection)
        {
            if (MoveCount == 1)//1なので次のターンの移動回数を増やす
                isNextPlusMoveCount = true;
            if (nextTurnPlusMoveCountAlly)
            {
                nextTurnPlusMoveCountAlly = false;
                MoveCount++;
                num++;
            }
            _threeCupsGame.ResetGame();
            saveMoveNum = num;
            _tileManager.SavePiecePosition();
            SetState(GameState.Move);
        }
        else if (_currentState == GameState.EnemyMoveCountSelection)
        {
            if (nextTurnPlusMoveCountEnemy)
            {
                nextTurnPlusMoveCountEnemy = false;
                MoveCount++;
            }
            SetState(GameState.EnemyMove);
        }
    }

    public void MinusMoveCount()
    {
        if (_currentState == GameState.Move)
        {
            if (!MoveResetButton.activeSelf)
            {
                MoveResetButton.gameObject.SetActive(true);
                MoveFinishButton.gameObject.SetActive(true);
                MoveFinishButton2.gameObject.SetActive(true);
            }
        }
        MoveCount--;
        if (MoveCount <= 0)
            canMove = false;
        _moveCountText.text = moveCountText + MoveCount.ToString();
    }

    public void GetPlusMoveTile(bool Side)
    {
        if (_currentState == GameState.Move)
        {
            saveMoveNum++;
            MoveCount++;
            canMove = true;
            _moveCountText.text = moveCountText + MoveCount.ToString();
        }
        else if(_currentState == GameState.EnemyMove)
        {
            MoveCount++;
            _moveCountText.text = moveCountText + MoveCount.ToString();
        }
        else//強風イベントでの入手
        {
            if (Side)
                nextTurnPlusMoveCountAlly = true;
            else
                nextTurnPlusMoveCountEnemy = true;
        }
    }

    public void OnClickResetMove()
    {
        AudioManager.Instance.SE(1);
        canMove = true;
        MoveCount = saveMoveNum;
        _tileManager.ResetMove();
        _moveCountText.text = moveCountText + MoveCount.ToString();
    }

    public void OnClickGoAttack()
    {
        if (_currentState == GameState.Move)
        {
            AudioManager.Instance.SE(0);
            _tileManager.StopBlinking(0);
            SetState(GameState.Attack);
        }
        if (_currentState == GameState.EnemyMove)
        {
            SetState(GameState.EnemyAttack);
        }
    }

    bool once = true;
    public void OnClickTurnFinish(bool type = false)
    {
        TurnFinishButton.SetActive(false);
        TurnFinishButton2.SetActive(false);
        //攻撃可能駒数が0でないなら確認メッセージを出す
        if (_tileManager.CanAttackPieceCount != 0 && !type)
        {
            TurnContinueButton.gameObject.SetActive(true);
            FinishDecisionButton.gameObject.SetActive(true);
            _explanationText.ChangeText(8);
            AudioManager.Instance.SE(27);
            return;
        }

        if (_currentState == GameState.Attack)
            AudioManager.Instance.SE(0);

        _tileManager.StopBlinking(1);
        if (_tileManager.isGameFinish)
        {
            GameFinishEffect();
            return;
        }

        //2ターン目に攻撃力アップタイルを確定で置く
        if (TurnNum == 1 && canBuffAttackTileEvent && _currentState == GameState.EnemyAttack)
        {
            AudioManager.Instance.SE(16);
            _buffTileManager.SetBuffTile(true, 1);
        }
        //3ターン目にHP回復タイルを確定で置く
        if (TurnNum == 2 && canHealingTileEvent && _currentState == GameState.EnemyAttack)
        {
            AudioManager.Instance.SE(16);
            _buffTileManager.SetBuffTile(true, 2);
        }

        if (_currentState == GameState.EnemyAttack)
            _mysteryBoxManager.MinusCountDown();
        //2～6ターン目の何処かでミステリーボックスを出現させる
        if (TurnNum >= 1 && canMysteryBoxEvent && _currentState == GameState.EnemyAttack)
        {
            int ran = Random.Range(0, 4);
            if (ran == 0 || TurnNum == 5)
            {
                canMysteryBoxEvent = false;
                _mysteryBoxManager.SetMysteryBox();
            }
        }

        //特殊マスの発生抽選
        if (canPlusMoveTileEvent && TurnNum >= 3)
        {
            int ran = Random.Range(0, 4);
            if (ran == 0)
            {
                AudioManager.Instance.SE(16);
                _buffTileManager.SetBuffTile(true, 0);
            }
        }
        if (canBuffAttackTileEvent && TurnNum >= 3)
        {
            int ran = Random.Range(0, 6);
            if (ran == 0)
            {
                AudioManager.Instance.SE(16);
                _buffTileManager.SetBuffTile(true, 1);
            }
        }
        if (canHealingTileEvent && TurnNum >= 3)
        {
            int ran = Random.Range(0, 6);
            if (ran == 0)
            {
                AudioManager.Instance.SE(16);
                _buffTileManager.SetBuffTile(true, 2);
            }
        }

        //強風イベントをするか否か
        if (canStrongWindEvent && TurnNum >= 2)
        {
            int ran = Random.Range(0, 8);
            if (ran == 0)
            {
                StartCoroutine(StrongWindEffect());
                return;
            }
        }

        if (_tileManager.isHalfPieceDown && once)
            StartCoroutine(HalfPieceDownEffect());
        else
        {
            if (_currentState == GameState.Attack)
            {
                SetState(GameState.Finish);
            }
            if (_currentState == GameState.EnemyAttack)
            {
                SetState(GameState.EnemyFinish);
            }
        }
    }

    //確認メッセージのどちらかが押された
    public void OnClickConfirmMessage(bool type)
    {
        if (type)
        {
            TurnContinueButton.SetActive(false);
            FinishDecisionButton.SetActive(false);
            OnClickTurnFinish(true);
        }
        else
        {
            AudioManager.Instance.SE(0);
            TurnFinishButton.SetActive(true);
            TurnFinishButton2.SetActive(true);
            TurnContinueButton.SetActive(false);
            FinishDecisionButton.SetActive(false);
            _explanationText.ChangeText(7);
        }
    }

    //強風イベント
    private IEnumerator StrongWindEffect()
    {
        if (_currentState == GameState.Attack)
            SetState(GameState.Event);
        if (_currentState == GameState.EnemyAttack)
            SetState(GameState.EnemyEvent);
        int direction = Random.Range(0, 4);

        Telops[3].SetActive(false);
        _turnText.text = TurnText[2];
        _phaseText.text = PhaseText[5];

        StrongWindText.SetActive(true);
        StrongWindText.transform.DOScaleY(1f, 0.5f).SetEase(Ease.InQuart);
        switch (direction)
        {
            case 0://上
                StrongWindText.transform.DOMoveX(0.5f, 2f).SetEase(Ease.InQuart).SetDelay(2f);
                StrongWindParticle.transform.localEulerAngles = new Vector3(-90, 180, 0);
                break;
            case 1://下
                StrongWindText.transform.DOMoveX(-0.5f, 2f).SetEase(Ease.InQuart).SetDelay(2f);
                StrongWindParticle.transform.localEulerAngles = new Vector3(-90, 0, 0);
                break;
            case 2://左
                StrongWindText.transform.DOMoveZ(-0.5f, 2f).SetEase(Ease.InQuart).SetDelay(2f);
                StrongWindParticle.transform.localEulerAngles = new Vector3(-90, 90, 0);
                break;
            case 3://右
                StrongWindText.transform.DOMoveZ(0.5f, 2f).SetEase(Ease.InQuart).SetDelay(2f);
                StrongWindParticle.transform.localEulerAngles = new Vector3(-90, -90, 0);
                break;
        }
        StrongWindText.transform.DOShakeRotation(1.5f, 90f, 30, 90, false).SetEase(Ease.InQuart).SetDelay(2.5f);

        yield return wait;
        AudioManager.Instance.SE(14);
        StrongWindParticle.Play();
        yield return wait;
        yield return wait;
        _tileManager.StrongWindEvent(direction);

        yield return new WaitForSeconds(2.5f);

        StrongWindText.SetActive(false);
        StrongWindText.transform.localScale = new Vector3(1, 0, 1);
        StrongWindText.transform.localPosition = new Vector3(-1.145f, -0.05f, 0);

        if (_tileManager.isHalfPieceDown && once)
            StartCoroutine(HalfPieceDownEffect());
        else
        {
            if (_currentState == GameState.Event)
                SetState(GameState.Finish);
            if (_currentState == GameState.EnemyEvent)
                SetState(GameState.EnemyFinish);
        }
        yield break;
    }

    //半分の駒が倒れた時の演出
    private IEnumerator HalfPieceDownEffect()
    {
        if (_currentState == GameState.Attack)
            SetState(GameState.Event);
        if (_currentState == GameState.EnemyAttack)
            SetState(GameState.EnemyEvent);
        AudioManager.Instance.SE(15);
        once = false;

        Telops[3].SetActive(false);
        _turnText.text = TurnText[2];
        _phaseText.text = PhaseText[4];

        HalfwayText.SetActive(true);
        HalfwayText.transform.DOScaleY(1f, 1f).SetEase(Ease.InQuart);
        HalfwayText.transform.DOLocalRotate(new Vector3(0, 720, 0), 4f, RotateMode.FastBeyond360).SetEase(Ease.InOutQuint).SetDelay(1.5f);
        yield return wait;
        HalfwayEffect.SetActive(true);
        yield return new WaitForSeconds(5f);
        HalfwayText.SetActive(false);
        if (_currentState == GameState.Event)
            SetState(GameState.Finish);
        if (_currentState == GameState.EnemyEvent)
            SetState(GameState.EnemyFinish);
        yield return wait;
        HalfwayEffect.SetActive(false);
        yield return wait;
        //HalfwayExplanation.transform.eulerAngles = new Vector3(0, -45, 90);
        HalfwayExplanation.SetActive(true);
        yield return null;
        //HalfwayExplanation.transform.DORotate(new Vector3(0, -45, 20), 2f).SetEase(Ease.InQuad);

        yield return new WaitForSeconds(10f);
        HalfwayExplanation.SetActive(false);
        yield break;
    }

    //攻撃シーンに遷移するかの質問時にボタンを一時的に非表示にする
    public void ActiveChangeButton(bool type)
    {
        TurnFinishButton.SetActive(type);
        TurnFinishButton2.SetActive(type);
    }

    public void GameFinish()
    {
        StartCoroutine(GameFinishEffect());
    }

    private IEnumerator GameFinishEffect()
    {
        //Debug.Log("ゲーム終了　勝ったのは" + _tileManager.isWinSide);
        SetState(GameState.GameFinish);

        _pieceInfoDisplay.InActiveDisplay();
        AttributeCorrelationChart.SetActive(false);

        Board.SetActive(false);
        _turnText.gameObject.SetActive(false);
        _phaseText.gameObject.SetActive(false);
        _turnNumText.gameObject.SetActive(false);
        _moveCountText.gameObject.SetActive(false);

        TurnFinishButton.gameObject.SetActive(false);
        TurnFinishButton2.gameObject.SetActive(false);
        Telops[Telops.Length - 1].SetActive(false);
        _tileManager.StopBlinking(1);

        yield return new WaitForSeconds(0.5f);

        if (!_tileManager.isWinSide)
        {
            GameFinishText.color = Color.blue;
            GameFinishText.text = isWinText[1];
            LoseEffectObj.SetActive(true);
            AudioManager.Instance.SetBGM(6);
        }
        else
        {
            GameFinishText.text = isWinText[0];
            WinEffectObj.SetActive(true);
            switch (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>().BoardSize)
            {
                case 5:
                    PlayerPrefs.SetInt("isClear5", 1);
                    break;
                case 6:
                    PlayerPrefs.SetInt("isClear6", 1);
                    break;
                case 7:
                    PlayerPrefs.SetInt("isClear7", 1);
                    break;
            }
            AudioManager.Instance.SetBGM(5);
        }
        GameFinishText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        BackTitleButton.SetActive(true);
        RetryButton.SetActive(true);
        yield break;
    }

    public void OnClickOneMoreGame()
    {
        GameInfomation.isConsecutivePlay = true;
        StartCoroutine(ChangeScene());
    }

    public void OnClickGoTitle()
    {
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        AudioManager.Instance.SE(0);
        AudioManager.Instance.StopBGM(1);
        BackTitleButton.SetActive(false);
        RetryButton.SetActive(false);
        _fade.FadeOut();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("SampleScene");
    }
}
