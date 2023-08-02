using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;

public class OptionManager : MonoBehaviour
{
    private bool isGameStart = false;

    [Header("AI")]
    public int AILevel;
    [SerializeField] GameObject LevelUpButton;
    [SerializeField] GameObject LevelDownButton;
    [SerializeField] TextMeshProUGUI _aiLevelText;
    [SerializeField] string[] levelTexts = new string[3];

    [Header("LeftSideOptions")]
    [SerializeField] Image[] _checkImage = new Image[3];
    [SerializeField] Sprite[] CheckMark = new Sprite[2];

    [Header("ExtraOptions")]
    [SerializeField] GameObject ExtraOptionPanel;
    [SerializeField] Image ActivePanelButtonImage;
    [SerializeField] Sprite[] _panelButtonSprites = new Sprite[2];
    [SerializeField] Image[] _checkImage2 = new Image[13];
    [SerializeField] GameObject LockImage;

    private GameInfomation _gameInfomation;

    void Start()
    {
        _gameInfomation = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>();
        SetText();
        switch (_gameInfomation.isAILevel)
        {
            case 0:
                _aiLevelText.text = levelTexts[0];
                if (_gameInfomation.isClear5)
                    LevelUpButton.SetActive(true);
                else
                    LevelUpButton.SetActive(false);
                LevelDownButton.SetActive(false);
                break;
            case 1:
                _aiLevelText.text = levelTexts[1];
                LevelUpButton.SetActive(true);
                LevelDownButton.SetActive(true);
                break;
            case 2:
                _aiLevelText.text = levelTexts[2];
                LevelUpButton.SetActive(false);
                LevelDownButton.SetActive(true);
                break;
        }
        AILevel = _gameInfomation.isAILevel;

        for (int i = 0; i < _checkImage.Length; i++)
            _checkImage[i].sprite = CheckMark[0];
        if (_gameInfomation.isShowExplanation)
            _checkImage[0].sprite = CheckMark[1];
        if (_gameInfomation.isHalfwayBoost)
            _checkImage[1].sprite = CheckMark[1];
        if (_gameInfomation.isStrongWind)
            _checkImage[2].sprite = CheckMark[1];

        for (int i = 0; i < _checkImage2.Length; i++)
            _checkImage2[i].sprite = CheckMark[0];
        if (_gameInfomation.isHighStakesBattle)
            _checkImage2[0].sprite = CheckMark[1];
        if (_gameInfomation.isIncreasedFighterSelection != 0)
            _checkImage2[_gameInfomation.isIncreasedFighterSelection].sprite = CheckMark[1];
        if (_gameInfomation.isRandomAttribute)
            _checkImage2[4].sprite = CheckMark[1];
        if (_gameInfomation.isEnemyHP != 0)
            _checkImage2[_gameInfomation.isEnemyHP + 4].sprite = CheckMark[1];
        if (_gameInfomation.isReduceYourPiece != 0)
            _checkImage2[_gameInfomation.isReduceYourPiece + 7].sprite = CheckMark[1];
        if (_gameInfomation.isAppearMysteryBox)
            _checkImage2[10].sprite = CheckMark[1];
        if (_gameInfomation.isPlusMoveCountTile)
            _checkImage2[11].sprite = CheckMark[1];
        if (_gameInfomation.isPowerBuffTile)
            _checkImage2[12].sprite = CheckMark[1];
        if (_gameInfomation.isHealingTile)
            _checkImage2[13].sprite = CheckMark[1];

        //‰Û‹à‚³‚ê‚Ä‚¢‚È‚¢Žž
        if (!_gameInfomation.isPay)
        {
            for (int i = 1; i < _checkImage2.Length; i++)
                _checkImage2[i].gameObject.SetActive(false);
            LockImage.SetActive(true);
        }

        ExtraOptionPanel.SetActive(false);
    }

    //Language‚ÅText‚ð•Ï‚¦‚é
    public void SetText()
    {
        for (int i = 0; i < levelTexts.Length; i++)
            levelTexts[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "AIOption" + (i + 1).ToString());
        switch (AILevel)
        {
            case 0:
                _aiLevelText.text = levelTexts[0];
                break;
            case 1:
                _aiLevelText.text = levelTexts[1];
                break;
            case 2:
                _aiLevelText.text = levelTexts[2];
                break;
        }
    }

    public void OnClickLevelChange(bool up)
    {
        if (isGameStart) return;
        switch (AILevel)
        {
            case 0:
                if (!up) return;
                _aiLevelText.text = levelTexts[1];
                LevelDownButton.SetActive(true);
                AILevel = 1;
                break;
            case 1:
                if (up)
                {
                    _aiLevelText.text = levelTexts[2];
                    LevelUpButton.SetActive(false);
                    LevelDownButton.SetActive(true);
                    AILevel = 2;
                }
                else
                {
                    _aiLevelText.text = levelTexts[0];
                    LevelUpButton.SetActive(true);
                    LevelDownButton.SetActive(false);
                    AILevel = 0;
                }
                break;
            case 2:
                if (up) return;
                _aiLevelText.text = levelTexts[1];
                LevelUpButton.SetActive(true);
                AILevel = 1;
                break;
        }
        AudioManager.Instance.SE(0);
    }

    public void OnClickLeftOption(int num)
    {
        if (isGameStart) return;
        AudioManager.Instance.SE(0);
        switch (num)
        {
            case 0:
                if (_gameInfomation.isShowExplanation)
                    _checkImage[0].sprite = CheckMark[0];
                else
                    _checkImage[0].sprite = CheckMark[1];
                _gameInfomation.isShowExplanation = !_gameInfomation.isShowExplanation;
                break;
            case 1:
                if (_gameInfomation.isHalfwayBoost)
                    _checkImage[1].sprite = CheckMark[0];
                else
                    _checkImage[1].sprite = CheckMark[1];
                _gameInfomation.isHalfwayBoost = !_gameInfomation.isHalfwayBoost;
                break;
            case 2:
                if (_gameInfomation.isStrongWind)
                    _checkImage[2].sprite = CheckMark[0];
                else
                    _checkImage[2].sprite = CheckMark[1];
                _gameInfomation.isStrongWind = !_gameInfomation.isStrongWind;
                break;
        }
    }

    public void OnClickShowPanel()
    {
        AudioManager.Instance.SE(0);
        if (ExtraOptionPanel.activeSelf)
        {
            ActivePanelButtonImage.sprite = _panelButtonSprites[0];
            ExtraOptionPanel.SetActive(false);
        }
        else
        {
            ActivePanelButtonImage.sprite = _panelButtonSprites[1];
            ExtraOptionPanel.SetActive(true);
        }
    }

    public void OnClickExtraOption(int num)
    {
        if (isGameStart) return;
        AudioManager.Instance.SE(0);
        if (!_gameInfomation.isPay && num != 0) return;
        switch (num)
        {
            case 0:
                if (_gameInfomation.isHighStakesBattle)
                    _checkImage2[num].sprite = CheckMark[0];
                else
                    _checkImage2[num].sprite = CheckMark[1];
                _gameInfomation.isHighStakesBattle = !_gameInfomation.isHighStakesBattle;
                break;
            case 1:
            case 2:
            case 3:
                for (int i = 1; i < 4; i++)
                    _checkImage2[i].sprite = CheckMark[0];
                if (_gameInfomation.isIncreasedFighterSelection == num)
                    _gameInfomation.isIncreasedFighterSelection = 0;
                else
                {
                    _checkImage2[num].sprite = CheckMark[1];
                    _gameInfomation.isIncreasedFighterSelection = num;
                }
                break;
            case 4:
                if (_gameInfomation.isRandomAttribute)
                    _checkImage2[num].sprite = CheckMark[0];
                else
                    _checkImage2[num].sprite = CheckMark[1];
                _gameInfomation.isRandomAttribute = !_gameInfomation.isRandomAttribute;
                break;
            case 5:
            case 6:
            case 7:
                for (int i = 5; i < 8; i++)
                    _checkImage2[i].sprite = CheckMark[0];
                if (_gameInfomation.isEnemyHP == num - 4)
                    _gameInfomation.isEnemyHP = 0;
                else
                {
                    _checkImage2[num].sprite = CheckMark[1];
                    _gameInfomation.isEnemyHP = num - 4;
                }
                break;
            case 8:
            case 9:
                for (int i = 8; i < 10; i++)
                    _checkImage2[i].sprite = CheckMark[0];
                if (_gameInfomation.isReduceYourPiece == num - 7)
                    _gameInfomation.isReduceYourPiece = 0;
                else
                {
                    _checkImage2[num].sprite = CheckMark[1];
                    _gameInfomation.isReduceYourPiece = num - 7;
                }
                break;
            case 10:
                if (_gameInfomation.isAppearMysteryBox)
                    _checkImage2[num].sprite = CheckMark[0];
                else
                    _checkImage2[num].sprite = CheckMark[1];
                _gameInfomation.isAppearMysteryBox = !_gameInfomation.isAppearMysteryBox;
                break;
            case 11:
                if (_gameInfomation.isPlusMoveCountTile)
                    _checkImage2[num].sprite = CheckMark[0];
                else
                    _checkImage2[num].sprite = CheckMark[1];
                _gameInfomation.isPlusMoveCountTile = !_gameInfomation.isPlusMoveCountTile;
                break;
            case 12:
                if (_gameInfomation.isPowerBuffTile)
                    _checkImage2[num].sprite = CheckMark[0];
                else
                    _checkImage2[num].sprite = CheckMark[1];
                _gameInfomation.isPowerBuffTile = !_gameInfomation.isPowerBuffTile;
                break;
            case 13:
                if (_gameInfomation.isHealingTile)
                    _checkImage2[num].sprite = CheckMark[0];
                else
                    _checkImage2[num].sprite = CheckMark[1];
                _gameInfomation.isHealingTile = !_gameInfomation.isHealingTile;
                break;
        }
    }


    public void GameStartEffect(bool isPractice = false)
    {
        if (!isPractice)
        {
            isGameStart = true;
            if (ExtraOptionPanel.activeSelf)
                ExtraOptionPanel.SetActive(false);
        }
        _gameInfomation.isAILevel = AILevel;
        PlayerPrefs.SetInt("isAILevel", AILevel);
        if(_gameInfomation.isShowExplanation)
            PlayerPrefs.SetInt("isShowExplanation", 1);
        else
            PlayerPrefs.SetInt("isShowExplanation", 0);
        if (_gameInfomation.isHalfwayBoost)
            PlayerPrefs.SetInt("isHalfwayBoost", 1);
        else
            PlayerPrefs.SetInt("isHalfwayBoost", 0);
        if (_gameInfomation.isStrongWind)
            PlayerPrefs.SetInt("isStrongWind", 1);
        else
            PlayerPrefs.SetInt("isStrongWind", 0);

        if (_gameInfomation.isHighStakesBattle)
            PlayerPrefs.SetInt("isHighStakesBattle", 1);
        else
            PlayerPrefs.SetInt("isHighStakesBattle", 0);
        if (_gameInfomation.isIncreasedFighterSelection != 0)
            PlayerPrefs.SetInt("isIncreasedFighterSelection", _gameInfomation.isIncreasedFighterSelection);
        else
            PlayerPrefs.SetInt("isIncreasedFighterSelection", 0);
        if (_gameInfomation.isRandomAttribute)
            PlayerPrefs.SetInt("isRandomAttribute", 1);
        else
            PlayerPrefs.SetInt("isRandomAttribute", 0);
        if (_gameInfomation.isEnemyHP != 0)
            PlayerPrefs.SetInt("isEnemyHP", _gameInfomation.isEnemyHP);
        else
            PlayerPrefs.SetInt("isEnemyHP", 0);
        if (_gameInfomation.isReduceYourPiece != 0)
            PlayerPrefs.SetInt("isReduceYourPiece", _gameInfomation.isReduceYourPiece);
        else
            PlayerPrefs.SetInt("isReduceYourPiece", 0);
        if (_gameInfomation.isAppearMysteryBox)
            PlayerPrefs.SetInt("isAppearMysteryBox", 1);
        else
            PlayerPrefs.SetInt("isAppearGiftBox", 0);
        if (_gameInfomation.isPlusMoveCountTile)
            PlayerPrefs.SetInt("isPlusMoveCountTile", 1);
        else
            PlayerPrefs.SetInt("isPlusMoveCountTile", 0);
        if(_gameInfomation.isPowerBuffTile)
            PlayerPrefs.SetInt("isPowerBuffTile", 1);
        else
            PlayerPrefs.SetInt("isPowerBuffTile", 0);
        if(_gameInfomation.isHealingTile)
            PlayerPrefs.SetInt("isHealingTile", 1);
        else
            PlayerPrefs.SetInt("isHealingTile", 0);
    }

    public void Purchase()
    {
        for (int i = 1; i < _checkImage2.Length; i++)
            _checkImage2[i].gameObject.SetActive(true);
        LockImage.SetActive(false);
    }
}
