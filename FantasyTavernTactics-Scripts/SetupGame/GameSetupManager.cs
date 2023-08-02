using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameSetupManager : MonoBehaviour
{
    public enum PhaseState
    {
        StartAnimation,
        DiceRoll,
        AttributeDetermination,
        ReducePiece,
        MyPieceDetermination,
        Finish
    }
    public PhaseState _currentState { private set; get; }

    [SerializeField] GameObject FighterDetermination;
    [SerializeField] Image GameSetupCompletedTelopFlame;
    [SerializeField] GameObject GameSetupCompletedTelop;
    [SerializeField] Sprite _flameImage;

    [SerializeField] GameObject ReducePieceTelop;

    private GameInfomation _gameInfomation;
    private ExplanationText _explanationText;
    private TurnManager _turnManager;
    private TileManager _tileManager;

    void Start()
    {
        _gameInfomation = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>();
        _explanationText = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExplanationText>();
        _turnManager = GetComponentInChildren<TurnManager>();
        _tileManager = GetComponentInChildren<TileManager>();
        GetComponentInParent<BattleSceneManagement>().Set();

        SetState(PhaseState.StartAnimation);
        FighterDetermination.SetActive(false);
        GameSetupCompletedTelopFlame.transform.localScale = new Vector3(0, 0.075f, 1);
        GameSetupCompletedTelop.transform.localScale = new Vector3(1, 0, 1);
        GameSetupCompletedTelopFlame.gameObject.SetActive(false);
        GameSetupCompletedTelop.gameObject.SetActive(false);

        if (!_gameInfomation.isShowExplanation)
            _explanationText.OffExplanationText();
    }

    public void SetState(PhaseState state)
    {
        _currentState = state;
        switch (state)
        {
            case PhaseState.StartAnimation:
                break;
            case PhaseState.DiceRoll:
                switch (_gameInfomation.isAILevel)
                {
                    case 0:
                        _turnManager.gameObject.AddComponent<AIBrain1>();
                        break;
                    case 1:
                        _turnManager.gameObject.AddComponent<AIBrain2>();
                        break;
                    case 2:
                        _turnManager.gameObject.AddComponent<AIBrain3>();
                        break;
                    default:
                        Debug.Log("AIlevelSettingMiss");
                        break;
                }
                break;
            case PhaseState.AttributeDetermination:
                break;
            case PhaseState.ReducePiece:
                //テロップ表示
                ReducePieceTelop.SetActive(true);
                ReducePieceTelop.transform.DOScaleY(1f, 1f).SetEase(Ease.InQuart).OnComplete(() => { StartCoroutine(ReducePiece()); });
                ReducePieceTelop.transform.DOLocalMoveY(0.15f, 3f).SetDelay(1.5f);
                var text = ReducePieceTelop.GetComponent<TextMeshProUGUI>();
                text.DOFade(0f, 3f).SetDelay(1.5f).OnComplete(() => { ReducePieceTelop.SetActive(false); SetState(PhaseState.MyPieceDetermination); });
                break;
            case PhaseState.MyPieceDetermination:
                FighterDetermination.SetActive(true);
                break;
            case PhaseState.Finish:
                AudioManager.Instance.StopBGM(1);
                GameSetupCompletedTelopFlame.gameObject.SetActive(true);
                GameSetupCompletedTelopFlame.transform.DOScaleX(1f, 1f).SetEase(Ease.InQuad).SetDelay(0.5f)
                    .OnComplete(() => { GameSetupFinish(); });
                break;
        }
    }

    //駒減らし
    private IEnumerator ReducePiece()
    {
        List<PieceInfomation> reducePiece = new List<PieceInfomation>();
        PieceInfomation[] allPiece = GetComponentsInChildren<PieceInfomation>();
        yield return null;
        for (int i = 0; i < _gameInfomation.isReduceYourPiece; i++)
        {
            reducePiece.Clear();
            for (int j = 0; j < allPiece.Length; j++)
            {
                if (!allPiece[j].Side) continue;
                if (allPiece[j].Role == 4) continue;
                if (allPiece[j].isDie) continue;
                reducePiece.Add(allPiece[j]);
            }
            yield return null;
            int ran = Random.Range(0, reducePiece.Count);
            _tileManager.CheckDown(reducePiece[ran], 100);
            yield return null;
            reducePiece[ran].GetComponentInChildren<PieceAnimation>().Attacked(100);
            yield return null;
        }
    }

    private void GameSetupFinish()
    {
        AudioManager.Instance.SE(10);
        _explanationText.ChangeActive(false);
        GameSetupCompletedTelopFlame.sprite = _flameImage;
        GameSetupCompletedTelopFlame.transform.DOScaleY(1f, 0.75f).SetEase(Ease.InQuad).SetDelay(0.1f);
        GameSetupCompletedTelop.gameObject.SetActive(true);
        GameSetupCompletedTelop.transform.DOScaleY(1f, 0.75f).SetEase(Ease.InQuad).SetDelay(0.1f);

        GameSetupCompletedTelopFlame.transform.DOScaleX(0f, 1.5f).SetEase(Ease.OutQuad).SetDelay(2f);
        GameSetupCompletedTelop.transform.DOScaleX(0f, 1.5f).SetEase(Ease.OutQuad).SetDelay(2f)
            .OnComplete(() => { GameSetupCompletedTelopFlame.gameObject.SetActive(false); GameSetupCompletedTelop.SetActive(false);
                _turnManager.SetState(TurnManager.GameState.StartGame); });
    }
}
