using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Localization.Settings;

public class MyPieceDetermination : MonoBehaviour
{
    [SerializeField] GameObject FighterPickingPhaseTelop;
    [SerializeField] TextMeshProUGUI _pickingNumText;
    [SerializeField] GameObject FighterSelectUndoButton;
    [SerializeField] GameObject FighterSelectConfirmButton;
    //ファイターを全て選ぶ前にConfirmを押した際のボタン
    [SerializeField] GameObject FighterSelectContinueButton;
    [SerializeField] GameObject FighterSelectFinishButton;

    private string remainingChoicesTexts;

    private GameInfomation _infomation;
    private GameSetupManager _setupManager;
    private ExplanationText _explanationText;

    private PieceInfomation[] _info;
    private int selectNumber = 0;

    private void Awake()
    {
        _infomation = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>();
        _setupManager = GetComponentInParent<GameSetupManager>();
        _explanationText = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExplanationText>();

        FighterSelectUndoButton.SetActive(false);
        FighterSelectConfirmButton.SetActive(false);
        FighterSelectContinueButton.SetActive(false);
        FighterSelectFinishButton.SetActive(false);
        _pickingNumText.gameObject.SetActive(false);
        _pickingNumText.gameObject.transform.localScale = new Vector3(1, 0, 1);

        FighterPickingPhaseTelop.transform.localScale = new Vector3(1, 0, 1);
        FighterPickingPhaseTelop.gameObject.SetActive(false);
    }

    private void Start()
    {
        FighterPickingPhaseTelop.SetActive(true);
        FighterPickingPhaseTelop.transform.DOScaleY(1f, 1f).SetEase(Ease.InQuart).OnComplete(() => { ShowText(); });
        FighterPickingPhaseTelop.transform.DOLocalMoveY(0.15f, 3f).SetDelay(1.5f);
        var text = FighterPickingPhaseTelop.GetComponent<TextMeshProUGUI>();
        text.DOFade(0f, 3f).SetDelay(1.5f).OnComplete(() => { FighterPickingPhaseTelop.SetActive(false); });
        switch (_infomation.BoardSize)
        {
            case 5:
                _info = new PieceInfomation[1 + _infomation.isIncreasedFighterSelection];
                break;
            case 6:
            case 7:
                _info = new PieceInfomation[2 + _infomation.isIncreasedFighterSelection];
                break;
        }
        remainingChoicesTexts = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "FighterChoice");
        _pickingNumText.text = remainingChoicesTexts + _info.Length.ToString();
    }

    private void ShowText()
    {
        _pickingNumText.gameObject.SetActive(true);
        _pickingNumText.gameObject.transform.DOScaleY(1f, 1f).SetEase(Ease.InQuart).SetDelay(1.5f)
            .OnComplete(() => { FighterSelectConfirmButton.SetActive(true); });
        _explanationText.ChangeText(4);
        _explanationText.ChangeActive(true);
        _explanationText.ChangeSize(2);
    }

    //駒が掴まれた時
    public void GrabPiece(PieceInfomation info)
    {
        if (_setupManager._currentState != GameSetupManager.PhaseState.MyPieceDetermination) return;
        if (FighterSelectContinueButton.activeSelf) return;
        if (info.Role == 4) return;
        if (info.isFighter) return;
        if (selectNumber == _info.Length) return;

        if(!FighterSelectConfirmButton.activeSelf)
            FighterSelectConfirmButton.SetActive(true);

        _info[selectNumber] = info;
        info.SetMyPiece();

        selectNumber++;
        _pickingNumText.text = remainingChoicesTexts + (_info.Length - selectNumber);

        if (!FighterSelectUndoButton.activeSelf)
            FighterSelectUndoButton.SetActive(true);
    }

    //一つ戻るが押された時
    public void RedoFighterSelect()
    {
        if (_setupManager._currentState != GameSetupManager.PhaseState.MyPieceDetermination) return;
        AudioManager.Instance.SE(1);
        selectNumber--;
        _info[selectNumber].ResetMyPiece();

        _pickingNumText.text = remainingChoicesTexts + (_info.Length - selectNumber);

        if (selectNumber == 0)
            FighterSelectUndoButton.SetActive(false);
    }

    //ファイター選択確定ボタンが押された時
    public void FinishFighterSelect()
    {
        if (_setupManager._currentState != GameSetupManager.PhaseState.MyPieceDetermination) return;
        AudioManager.Instance.SE(0);
        if (selectNumber != _info.Length)//全部選んでない
        {
            AudioManager.Instance.SE(27);
            _explanationText.ChangeText(5);
            FighterSelectUndoButton.SetActive(false);
            FighterSelectConfirmButton.SetActive(false);
            FighterSelectContinueButton.SetActive(true);
            FighterSelectFinishButton.SetActive(true);
        }
        else
        {
            _explanationText.ChangeActive(false);
            _explanationText.ChangeSize(0);
            _setupManager.SetState(GameSetupManager.PhaseState.Finish);
            gameObject.SetActive(false);
        }
    }

    //ファイター選択終了ボタンが押された時
    public void AbsoluteFinishFighterSelect()
    {
        if (_setupManager._currentState != GameSetupManager.PhaseState.MyPieceDetermination) return;
        AudioManager.Instance.SE(0);
        _explanationText.ChangeActive(false);
        _explanationText.ChangeSize(0);
        _setupManager.SetState(GameSetupManager.PhaseState.Finish);
        gameObject.SetActive(false);
    }

    //ファイター選択を続けるボタンが押された時
    public void ContinueFighterSelect()
    {
        if (_setupManager._currentState != GameSetupManager.PhaseState.MyPieceDetermination) return;
        AudioManager.Instance.SE(0);
        _explanationText.ChangeText(4);
        _explanationText.ChangeActive(true);
        if (selectNumber != 0)
            FighterSelectUndoButton.SetActive(true);
        FighterSelectConfirmButton.SetActive(true);
        FighterSelectContinueButton.SetActive(false);
        FighterSelectFinishButton.SetActive(false);
    }
}
