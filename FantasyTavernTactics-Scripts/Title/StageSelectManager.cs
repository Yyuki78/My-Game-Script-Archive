using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] int currentBoard;//戦闘練習が1、それ以外は盤のサイズ
    [SerializeField] int currentStage;//ステージナンバー

    [SerializeField] TextMeshProUGUI _currentBoardText;
    [SerializeField][TextArea]
    string[] BoardNumberTexts;

    [SerializeField] GameObject BattlePractice;
    [SerializeField] GameObject[] Stage5;
    [SerializeField] GameObject[] Stage6;
    [SerializeField] GameObject[] Stage7;

    [SerializeField] GameObject[] ChangeBoardSizeButtonRight = new GameObject[2];
    [SerializeField] GameObject[] ChangeBoardSizeButtonLeft = new GameObject[2];
    [SerializeField] GameObject[] ChangeStageButtonRight = new GameObject[2];
    [SerializeField] GameObject[] ChangeStageButtonLeft = new GameObject[2];

    [SerializeField] GameObject LockObject;
    [SerializeField] GameObject GameStartButton;
    [SerializeField] GameObject GameStart3DButton;

    [SerializeField] GameObject CurrentStage;

    private GameInfomation _gameInfomation;

    void Start()
    {
        _gameInfomation = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>();
        Stage5[0].SetActive(false);
        LockObject.SetActive(false);
        switch (_gameInfomation.isStageNumber)
        {
            case 0:
            case 51:
                ChangeStageButtonLeft[0].SetActive(false);
                ChangeStageButtonLeft[1].SetActive(false);
                if (_gameInfomation.isClear5)
                {
                    ChangeBoardSizeButtonRight[0].SetActive(true);
                    ChangeBoardSizeButtonRight[1].SetActive(true);
                    ChangeBoardSizeButtonLeft[0].SetActive(true);
                    ChangeBoardSizeButtonLeft[1].SetActive(true);
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                }
                else
                {
                    ChangeBoardSizeButtonRight[0].SetActive(false);
                    ChangeBoardSizeButtonRight[1].SetActive(false);
                    ChangeBoardSizeButtonLeft[0].SetActive(false);
                    ChangeBoardSizeButtonLeft[1].SetActive(false);
                    ChangeStageButtonRight[0].SetActive(false);
                    ChangeStageButtonRight[1].SetActive(false);
                }
                currentBoard = 5;
                CurrentStage = Stage5[0];
                Stage5[0].SetActive(true);
                break;
            case 1:
                ChangeBoardSizeButtonRight[0].SetActive(true);
                ChangeBoardSizeButtonRight[1].SetActive(true);
                ChangeBoardSizeButtonLeft[0].SetActive(false);
                ChangeBoardSizeButtonLeft[1].SetActive(false);
                ChangeStageButtonRight[0].SetActive(false);
                ChangeStageButtonRight[1].SetActive(false);
                ChangeStageButtonLeft[0].SetActive(false);
                ChangeStageButtonLeft[1].SetActive(false);
                currentBoard = 1;
                CurrentStage = BattlePractice;
                BattlePractice.SetActive(true);
                break;
            case 52:
                ChangeBoardSizeButtonRight[0].SetActive(true);
                ChangeBoardSizeButtonRight[1].SetActive(true);
                ChangeBoardSizeButtonLeft[0].SetActive(true);
                ChangeBoardSizeButtonLeft[1].SetActive(true);
                ChangeStageButtonRight[0].SetActive(true);
                ChangeStageButtonRight[1].SetActive(true);
                ChangeStageButtonLeft[0].SetActive(true);
                ChangeStageButtonLeft[1].SetActive(true);
                currentBoard = 5;
                CurrentStage = Stage5[1];
                Stage5[1].SetActive(true);
                break;
            case 53:
                ChangeBoardSizeButtonRight[0].SetActive(true);
                ChangeBoardSizeButtonRight[1].SetActive(true);
                ChangeBoardSizeButtonLeft[0].SetActive(true);
                ChangeBoardSizeButtonLeft[1].SetActive(true);
                ChangeStageButtonRight[0].SetActive(false);
                ChangeStageButtonRight[1].SetActive(false);
                ChangeStageButtonLeft[0].SetActive(true);
                ChangeStageButtonLeft[1].SetActive(true);
                currentBoard = 5;
                CurrentStage = Stage5[2];
                Stage5[2].SetActive(true);
                break;
            case 61:
                ChangeBoardSizeButtonLeft[0].SetActive(true);
                ChangeBoardSizeButtonLeft[1].SetActive(true);
                ChangeStageButtonLeft[0].SetActive(false);
                ChangeStageButtonLeft[1].SetActive(false);
                if (_gameInfomation.isClear6)
                {
                    ChangeBoardSizeButtonRight[0].SetActive(true);
                    ChangeBoardSizeButtonRight[1].SetActive(true);
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                }
                else
                {
                    ChangeBoardSizeButtonRight[0].SetActive(false);
                    ChangeBoardSizeButtonRight[1].SetActive(false);
                    ChangeStageButtonRight[0].SetActive(false);
                    ChangeStageButtonRight[1].SetActive(false);
                }
                currentBoard = 6;
                CurrentStage = Stage6[0];
                Stage6[0].SetActive(true);
                break;
            case 62:
                ChangeBoardSizeButtonRight[0].SetActive(true);
                ChangeBoardSizeButtonRight[1].SetActive(true);
                ChangeBoardSizeButtonLeft[0].SetActive(true);
                ChangeBoardSizeButtonLeft[1].SetActive(true);
                ChangeStageButtonRight[0].SetActive(true);
                ChangeStageButtonRight[1].SetActive(true);
                ChangeStageButtonLeft[0].SetActive(true);
                ChangeStageButtonLeft[1].SetActive(true);
                currentBoard = 6;
                CurrentStage = Stage6[1];
                Stage6[1].SetActive(true);
                break;
            case 63:
                ChangeBoardSizeButtonRight[0].SetActive(true);
                ChangeBoardSizeButtonRight[1].SetActive(true);
                ChangeBoardSizeButtonLeft[0].SetActive(true);
                ChangeBoardSizeButtonLeft[1].SetActive(true);
                ChangeStageButtonRight[0].SetActive(false);
                ChangeStageButtonRight[1].SetActive(false);
                ChangeStageButtonLeft[0].SetActive(true);
                ChangeStageButtonLeft[1].SetActive(true);
                currentBoard = 6;
                CurrentStage = Stage6[2];
                Stage6[2].SetActive(true);
                break;
            case 71:
                ChangeBoardSizeButtonRight[0].SetActive(false);
                ChangeBoardSizeButtonRight[1].SetActive(false);
                ChangeBoardSizeButtonLeft[0].SetActive(true);
                ChangeBoardSizeButtonLeft[1].SetActive(true);
                ChangeStageButtonLeft[0].SetActive(false);
                ChangeStageButtonLeft[1].SetActive(false);
                if (_gameInfomation.isClear7)
                {
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                }
                else
                {
                    ChangeStageButtonRight[0].SetActive(false);
                    ChangeStageButtonRight[1].SetActive(false);
                }
                currentBoard = 7;
                CurrentStage = Stage7[0];
                Stage7[0].SetActive(true);
                break;
            case 72:
                ChangeBoardSizeButtonRight[0].SetActive(false);
                ChangeBoardSizeButtonRight[1].SetActive(false);
                ChangeBoardSizeButtonLeft[0].SetActive(true);
                ChangeBoardSizeButtonLeft[1].SetActive(true);
                ChangeStageButtonRight[0].SetActive(true);
                ChangeStageButtonRight[1].SetActive(true);
                ChangeStageButtonLeft[0].SetActive(true);
                ChangeStageButtonLeft[1].SetActive(true);
                currentBoard = 7;
                CurrentStage = Stage7[1];
                Stage7[1].SetActive(true);
                break;
            case 73:
                ChangeBoardSizeButtonRight[0].SetActive(false);
                ChangeBoardSizeButtonRight[1].SetActive(false);
                ChangeBoardSizeButtonLeft[0].SetActive(true);
                ChangeBoardSizeButtonLeft[1].SetActive(true);
                ChangeStageButtonRight[0].SetActive(true);
                ChangeStageButtonRight[1].SetActive(true);
                ChangeStageButtonLeft[0].SetActive(true);
                ChangeStageButtonLeft[1].SetActive(true);
                currentBoard = 7;
                CurrentStage = Stage7[2];
                Stage7[2].SetActive(true);
                break;
            case 74:
                ChangeBoardSizeButtonRight[0].SetActive(false);
                ChangeBoardSizeButtonRight[1].SetActive(false);
                ChangeBoardSizeButtonLeft[0].SetActive(true);
                ChangeBoardSizeButtonLeft[1].SetActive(true);
                ChangeStageButtonRight[0].SetActive(true);
                ChangeStageButtonRight[1].SetActive(true);
                ChangeStageButtonLeft[0].SetActive(true);
                ChangeStageButtonLeft[1].SetActive(true);
                currentBoard = 7;
                CurrentStage = Stage7[3];
                Stage7[3].SetActive(true);
                break;
            case 75:
                ChangeBoardSizeButtonRight[0].SetActive(false);
                ChangeBoardSizeButtonRight[1].SetActive(false);
                ChangeBoardSizeButtonLeft[0].SetActive(true);
                ChangeBoardSizeButtonLeft[1].SetActive(true);
                ChangeStageButtonRight[0].SetActive(false);
                ChangeStageButtonRight[1].SetActive(false);
                ChangeStageButtonLeft[0].SetActive(true);
                ChangeStageButtonLeft[1].SetActive(true);
                currentBoard = 7;
                CurrentStage = Stage7[4];
                Stage7[4].SetActive(true);
                break;
        }
        if (_gameInfomation.isStageNumber != 0)
            currentStage = _gameInfomation.isStageNumber;
        else
            currentStage = 51;
        SetText();
    }

    //LanguageでTextを変える
    public void SetText()
    {
        for (int i = 0; i < BoardNumberTexts.Length; i++)
            BoardNumberTexts[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "TitleBoardText" + (i + 1).ToString());
        switch (currentBoard)
        {
            case 1:
                _currentBoardText.text = BoardNumberTexts[0];
                break;
            case 5:
                _currentBoardText.text = BoardNumberTexts[1];
                break;
            case 6:
                _currentBoardText.text = BoardNumberTexts[2];
                break;
            case 7:
                _currentBoardText.text = BoardNumberTexts[3];
                break;
        }
    }

    public void ChangeBoardSizeButton(bool isRight)
    {
        AudioManager.Instance.SE(2);
        LockObject.SetActive(false);
        GameStartButton.SetActive(true);
        GameStart3DButton.SetActive(true);
        switch (currentBoard)
        {
            case 1:
                if (!isRight) return;
                currentBoard = 5;
                _currentBoardText.text = BoardNumberTexts[1];
                currentStage = 51;
                CurrentStage = Stage5[0];
                BattlePractice.SetActive(false);
                Stage5[0].SetActive(true);
                ChangeBoardSizeButtonLeft[0].SetActive(true);
                ChangeBoardSizeButtonLeft[1].SetActive(true);
                ChangeStageButtonRight[0].SetActive(true);
                ChangeStageButtonRight[1].SetActive(true);
                ChangeStageButtonLeft[0].SetActive(false);
                ChangeStageButtonLeft[1].SetActive(false);
                break;
            case 5:
                if (!_gameInfomation.isClear5) return;
                Stage5[0].SetActive(false);
                Stage5[1].SetActive(false);
                Stage5[2].SetActive(false);
                if (isRight)
                {
                    currentBoard = 6;
                    _currentBoardText.text = BoardNumberTexts[2];
                    currentStage = 61;
                    CurrentStage = Stage6[0];
                    Stage6[0].SetActive(true);
                    if (!_gameInfomation.isClear6)
                    {
                        ChangeBoardSizeButtonRight[0].SetActive(false);
                        ChangeBoardSizeButtonRight[1].SetActive(false);
                        ChangeStageButtonRight[0].SetActive(false);
                        ChangeStageButtonRight[1].SetActive(false);
                    }
                    else
                    {
                        ChangeStageButtonRight[0].SetActive(true);
                        ChangeStageButtonRight[1].SetActive(true);
                    }
                    ChangeStageButtonLeft[0].SetActive(false);
                    ChangeStageButtonLeft[1].SetActive(false);
                }
                else
                {
                    currentBoard = 1;
                    _currentBoardText.text = BoardNumberTexts[0];
                    currentStage = 1;
                    CurrentStage = BattlePractice;
                    BattlePractice.SetActive(true);
                    ChangeBoardSizeButtonLeft[0].SetActive(false);
                    ChangeBoardSizeButtonLeft[1].SetActive(false);
                    ChangeStageButtonRight[0].SetActive(false);
                    ChangeStageButtonRight[1].SetActive(false);
                    ChangeStageButtonLeft[0].SetActive(false);
                    ChangeStageButtonLeft[1].SetActive(false);
                }
                break;
            case 6:
                Stage6[0].SetActive(false);
                Stage6[1].SetActive(false);
                Stage6[2].SetActive(false);
                if (isRight)
                {
                    currentBoard = 7;
                    _currentBoardText.text = BoardNumberTexts[3];
                    currentStage = 71;
                    CurrentStage = Stage7[0];
                    Stage7[0].SetActive(true);
                    ChangeBoardSizeButtonRight[0].SetActive(false);
                    ChangeBoardSizeButtonRight[1].SetActive(false);
                    if (!_gameInfomation.isClear7)
                    {
                        ChangeStageButtonRight[0].SetActive(false);
                        ChangeStageButtonRight[1].SetActive(false);
                    }
                    else
                    {
                        ChangeStageButtonRight[0].SetActive(true);
                        ChangeStageButtonRight[1].SetActive(true);
                    }
                    ChangeStageButtonLeft[0].SetActive(false);
                    ChangeStageButtonLeft[1].SetActive(false);
                }
                else
                {
                    currentBoard = 5;
                    _currentBoardText.text = BoardNumberTexts[1];
                    currentStage = 51;
                    CurrentStage = Stage5[0];
                    Stage5[0].SetActive(true);
                    ChangeBoardSizeButtonRight[0].SetActive(true);
                    ChangeBoardSizeButtonRight[1].SetActive(true);
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                    ChangeStageButtonLeft[0].SetActive(false);
                    ChangeStageButtonLeft[1].SetActive(false);
                }
                break;
            case 7:
                if (isRight) return;
                for(int i = 0; i < Stage7.Length; i++)
                    Stage7[i].SetActive(false);
                currentBoard = 6;
                _currentBoardText.text = BoardNumberTexts[2];
                currentStage = 61;
                CurrentStage = Stage6[0];
                Stage6[0].SetActive(true);
                ChangeBoardSizeButtonRight[0].SetActive(true);
                ChangeBoardSizeButtonRight[1].SetActive(true);
                ChangeStageButtonRight[0].SetActive(true);
                ChangeStageButtonRight[1].SetActive(true);
                ChangeStageButtonLeft[0].SetActive(false);
                ChangeStageButtonLeft[1].SetActive(false);
                break;
            default:
                Debug.Log("ボード変更ミス");
                break;
        }
    }

    public void ChangeStageButton(bool isRight)
    {
        AudioManager.Instance.SE(2);
        switch (currentStage)
        {
            case 1:
                break;
            case 51:
                if (!_gameInfomation.isClear5) return;
                if (isRight)
                {
                    currentStage = 52;
                    CurrentStage = Stage5[1];
                    Stage5[0].SetActive(false);
                    Stage5[1].SetActive(true);
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                    ChangeStageButtonLeft[0].SetActive(true);
                    ChangeStageButtonLeft[1].SetActive(true);
                }
                break;
            case 52:
                if (isRight)
                {
                    if (!_gameInfomation.isPay)
                    {
                        LockObject.SetActive(true);
                        GameStartButton.SetActive(false);
                        GameStart3DButton.SetActive(false);
                    }
                    currentStage = 53;
                    CurrentStage = Stage5[2];
                    Stage5[2].SetActive(true);
                    Stage5[1].SetActive(false);
                    ChangeStageButtonRight[0].SetActive(false);
                    ChangeStageButtonRight[1].SetActive(false);
                    ChangeStageButtonLeft[0].SetActive(true);
                    ChangeStageButtonLeft[1].SetActive(true);
                }
                else
                {
                    currentStage = 51;
                    CurrentStage = Stage5[0];
                    Stage5[0].SetActive(true);
                    Stage5[1].SetActive(false);
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                    ChangeStageButtonLeft[0].SetActive(false);
                    ChangeStageButtonLeft[1].SetActive(false);
                }
                break;
            case 53:
                if (!isRight)
                {
                    if (!_gameInfomation.isPay)
                    {
                        LockObject.SetActive(false);
                        GameStartButton.SetActive(true);
                        GameStart3DButton.SetActive(true);
                    }
                    currentStage = 52;
                    CurrentStage = Stage5[1];
                    Stage5[2].SetActive(false);
                    Stage5[1].SetActive(true);
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                    ChangeStageButtonLeft[0].SetActive(true);
                    ChangeStageButtonLeft[1].SetActive(true);
                }
                break;
            case 61:
                if (isRight)
                {
                    currentStage = 62;
                    CurrentStage = Stage6[1];
                    Stage6[0].SetActive(false);
                    Stage6[1].SetActive(true);
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                    ChangeStageButtonLeft[0].SetActive(true);
                    ChangeStageButtonLeft[1].SetActive(true);
                }
                break;
            case 62:
                if (isRight)
                {
                    if (!_gameInfomation.isPay)
                    {
                        LockObject.SetActive(true);
                        GameStartButton.SetActive(false);
                        GameStart3DButton.SetActive(false);
                    }
                    currentStage = 63;
                    CurrentStage = Stage6[2];
                    Stage6[1].SetActive(false);
                    Stage6[2].SetActive(true);
                    ChangeStageButtonRight[0].SetActive(false);
                    ChangeStageButtonRight[1].SetActive(false);
                    ChangeStageButtonLeft[0].SetActive(true);
                    ChangeStageButtonLeft[1].SetActive(true);
                }
                else
                {
                    currentStage = 61;
                    CurrentStage = Stage6[0];
                    Stage6[0].SetActive(true);
                    Stage6[1].SetActive(false);
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                    ChangeStageButtonLeft[0].SetActive(false);
                    ChangeStageButtonLeft[1].SetActive(false);
                }
                break;
            case 63:
                if (!isRight)
                {
                    if (!_gameInfomation.isPay)
                    {
                        LockObject.SetActive(false);
                        GameStartButton.SetActive(true);
                        GameStart3DButton.SetActive(true);
                    }
                    currentStage = 62;
                    CurrentStage = Stage6[1];
                    Stage6[2].SetActive(false);
                    Stage6[1].SetActive(true);
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                    ChangeStageButtonLeft[0].SetActive(true);
                    ChangeStageButtonLeft[1].SetActive(true);
                }
                break;
            case 71:
                if (isRight)
                {
                    currentStage = 72;
                    CurrentStage = Stage7[1];
                    Stage7[0].SetActive(false);
                    Stage7[1].SetActive(true);
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                    ChangeStageButtonLeft[0].SetActive(true);
                    ChangeStageButtonLeft[1].SetActive(true);
                }
                break;
            case 72:
                if (isRight)
                {
                    if (!_gameInfomation.isPay)
                    {
                        LockObject.SetActive(true);
                        GameStartButton.SetActive(false);
                        GameStart3DButton.SetActive(false);
                    }
                    currentStage = 73;
                    CurrentStage = Stage7[2];
                    Stage7[1].SetActive(false);
                    Stage7[2].SetActive(true);
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                    ChangeStageButtonLeft[0].SetActive(true);
                    ChangeStageButtonLeft[1].SetActive(true);
                }
                else
                {
                    currentStage = 71;
                    CurrentStage = Stage7[0];
                    Stage7[0].SetActive(true);
                    Stage7[1].SetActive(false);
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                    ChangeStageButtonLeft[0].SetActive(false);
                    ChangeStageButtonLeft[1].SetActive(false);
                }
                break;
            case 73:
                if (isRight)
                {
                    currentStage = 74;
                    CurrentStage = Stage7[3];
                    Stage7[2].SetActive(false);
                    Stage7[3].SetActive(true);
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                    ChangeStageButtonLeft[0].SetActive(true);
                    ChangeStageButtonLeft[1].SetActive(true);
                }
                else
                {
                    if (!_gameInfomation.isPay)
                    {
                        LockObject.SetActive(false);
                        GameStartButton.SetActive(true);
                        GameStart3DButton.SetActive(true);
                    }
                    currentStage = 72;
                    CurrentStage = Stage7[1];
                    Stage7[1].SetActive(true);
                    Stage7[2].SetActive(false);
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                    ChangeStageButtonLeft[0].SetActive(true);
                    ChangeStageButtonLeft[1].SetActive(true);
                }
                break;
            case 74:
                if (isRight)
                {
                    currentStage = 75;
                    CurrentStage = Stage7[4];
                    Stage7[3].SetActive(false);
                    Stage7[4].SetActive(true);
                    ChangeStageButtonRight[0].SetActive(false);
                    ChangeStageButtonRight[1].SetActive(false);
                    ChangeStageButtonLeft[0].SetActive(true);
                    ChangeStageButtonLeft[1].SetActive(true);
                }
                else
                {
                    currentStage = 73;
                    CurrentStage = Stage7[2];
                    Stage7[2].SetActive(true);
                    Stage7[3].SetActive(false);
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                    ChangeStageButtonLeft[0].SetActive(true);
                    ChangeStageButtonLeft[1].SetActive(true);
                }
                break;
            case 75:
                if (!isRight)
                {
                    currentStage = 74;
                    CurrentStage = Stage7[3];
                    Stage7[3].SetActive(true);
                    Stage7[4].SetActive(false);
                    ChangeStageButtonRight[0].SetActive(true);
                    ChangeStageButtonRight[1].SetActive(true);
                    ChangeStageButtonLeft[0].SetActive(true);
                    ChangeStageButtonLeft[1].SetActive(true);
                }
                break;
            default:
                Debug.Log("ステージ変更ミス");
                break;
        }
    }

    public int GameStartEffect()
    {
        int BoardSize = 0;
        switch (currentStage)
        {
            case 1:
                BoardSize = 1;
                break;
            case 51:
            case 52:
            case 53:
                BoardSize = 5;
                break;
            case 61:
            case 62:
            case 63:
                BoardSize = 6;
                break;
            case 71:
            case 72:
            case 73:
            case 74:
            case 75:
                BoardSize = 7;
                break;
        }
        if (!_gameInfomation.isPay)
        {
            if (currentStage == 53 || currentStage == 63 || currentStage == 73 || currentStage == 74 || currentStage == 75)
            {
                return 100;
            }
        }
        _gameInfomation.BoardSize = BoardSize;
        PlayerPrefs.SetInt("BoardSize", BoardSize);
        _gameInfomation.isStageNumber = currentStage;
        PlayerPrefs.SetInt("isStageNumber", currentStage);
        ChangeBoardSizeButtonRight[0].SetActive(false);
        ChangeBoardSizeButtonRight[1].SetActive(false);
        ChangeBoardSizeButtonLeft[0].SetActive(false);
        ChangeBoardSizeButtonLeft[1].SetActive(false);
        ChangeStageButtonRight[0].SetActive(false);
        ChangeStageButtonRight[1].SetActive(false);
        ChangeStageButtonLeft[0].SetActive(false);
        ChangeStageButtonLeft[1].SetActive(false);
        return currentStage;
    }

    public void Purchase()
    {
        LockObject.SetActive(false);
        GameStartButton.SetActive(true);
        GameStart3DButton.SetActive(true);
    }
}
