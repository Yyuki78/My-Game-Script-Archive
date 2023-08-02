using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfomation : MonoBehaviour
{
    public static bool isConsecutivePlay = false;//リトライ用

    public int isStageNumber = 0;
    public int BoardSize  = 5;
    public bool isShowAttackMessage = false;

    [Header("BGM選択状況")]
    public int isGameBGM = 0;
    public int isBattleBGM = 0;

    [Header("ステージ進捗状況")]
    public bool isFirstActivation = false;
    public bool isClear5 = false;
    public bool isClear6 = false;
    public bool isClear7 = false;

    //OptionManagerで変更
    [Header("オプション選択状況")]
    public int isAILevel;
    public bool isShowExplanation = false;
    public bool isHalfwayBoost = false;
    public bool isStrongWind = false;
    public bool isHighStakesBattle = false;

    [Header("課金オプション選択状況")]
    public bool isPay;
    public int isIncreasedFighterSelection;
    public bool isRandomAttribute;
    public int isEnemyHP;
    public int isReduceYourPiece;
    public bool isAppearMysteryBox;
    public bool isPlusMoveCountTile;
    public bool isPowerBuffTile;
    public bool isHealingTile;

    public enum GameState
    {
        Title,
        SetupGame,
        InGame,
        GameResult,
    }
    public GameState _currentState { private set; get; }

    void Awake()
    {
        Application.targetFrameRate = 72;
        SetState(GameState.Title);
        isStageNumber = PlayerPrefs.GetInt("isStageNumber", 0);
        BoardSize = PlayerPrefs.GetInt("BoardSize", 0);
        isGameBGM = PlayerPrefs.GetInt("isGameBGM", 0);
        isBattleBGM = PlayerPrefs.GetInt("isBattleBGM", 0);
        if (PlayerPrefs.GetInt("isFirstActivation", 0) == 0)
        {
            PlayerPrefs.SetInt("isFirstActivation", 1);
            isFirstActivation = true;
        }
        if (PlayerPrefs.GetInt("isClear5", 0) != 0)
            isClear5 = true;
        if (PlayerPrefs.GetInt("isClear6", 0) != 0)
            isClear6 = true;
        if (PlayerPrefs.GetInt("isClear7", 0) != 0)
            isClear7 = true;
        isAILevel = PlayerPrefs.GetInt("isAILevel", 0);
        if (PlayerPrefs.GetInt("isShowExplanation", 1) != 0)
            isShowExplanation = true;
        if (PlayerPrefs.GetInt("isHalfwayBoost", 1) != 0)
            isHalfwayBoost = true;
        if (PlayerPrefs.GetInt("isStrongWind", 0) != 0)
            isStrongWind = true;
        if (PlayerPrefs.GetInt("isHighStakesBattle", 0) != 0)
            isHighStakesBattle = true;
        if (PlayerPrefs.GetInt("isAppearMysteryBox", 0) != 0)
            isAppearMysteryBox = true;

        if (PlayerPrefs.GetInt("isPay", 0) != 0)
            isPay = true;
        isIncreasedFighterSelection = PlayerPrefs.GetInt("isIncreasedFighterSelection", 0);
        isEnemyHP = PlayerPrefs.GetInt("isEnemyHP", 0);
        isReduceYourPiece = PlayerPrefs.GetInt("isReduceYourPiece", 0);
        if (PlayerPrefs.GetInt("isRandomAttribute", 0) != 0)
            isRandomAttribute = true;
        if (PlayerPrefs.GetInt("isPlusMoveCountTile", 0) != 0)
            isPlusMoveCountTile = true;
        if (PlayerPrefs.GetInt("isPowerBuffTile", 0) != 0)
            isPowerBuffTile = true;
        if (PlayerPrefs.GetInt("isHealingTile", 0) != 0)
            isHealingTile = true;
    }

    public void SetState(GameState state)
    {
        _currentState = state;
        switch (state)
        {
            case GameState.Title:
                break;
            case GameState.SetupGame:
                break;
            case GameState.InGame:
                break;
            case GameState.GameResult:
                break;
        }
    }
}
