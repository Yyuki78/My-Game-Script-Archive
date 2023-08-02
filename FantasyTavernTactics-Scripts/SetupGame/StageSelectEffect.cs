using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class StageSelectEffect : MonoBehaviour
{
    [SerializeField] GameObject Board;

    [SerializeField] GameObject[] AllyPiece;
    [SerializeField] GameObject[] EnemyPiece;
    private AudioSource[] _allyAudio;
    private AudioSource[] _enemyAudio;

    [SerializeField] GameObject AttributeCorrelationChart;
    [SerializeField] GameObject AttributeDiceMug;
    [SerializeField] GameObject AttributeDiceShakeTelop;
    private TextMeshProUGUI _text;
    private GameSetupManager _setupManager;
    private ExplanationText _explanationText;
    private InGameSettingPanel _inGameSettingPanel;

    void Start()
    {
        Board.SetActive(false);
        _allyAudio = new AudioSource[AllyPiece.Length];
        _enemyAudio = new AudioSource[AllyPiece.Length];
        for (int i = 0; i < AllyPiece.Length; i++)
        {
            _allyAudio[i] = AllyPiece[i].GetComponentInChildren<AudioSource>();
            _enemyAudio[i] = EnemyPiece[i].GetComponentInChildren<AudioSource>();
            AllyPiece[i].SetActive(false);
            EnemyPiece[i].SetActive(false);
        }
        AttributeCorrelationChart.SetActive(false);
        AttributeDiceMug.SetActive(false);
        AttributeDiceShakeTelop.SetActive(false);
        AttributeDiceShakeTelop.transform.localScale = new Vector3(1, 0, 1);
        _text = AttributeDiceShakeTelop.GetComponent<TextMeshProUGUI>();
        _setupManager = GetComponent<GameSetupManager>();
        _explanationText = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExplanationText>();
        _explanationText.ChangeText(0);
        _explanationText.ChangeActive(false);
        _explanationText.ChangeSize(0);
        _inGameSettingPanel = GameObject.FindGameObjectWithTag("GameController").GetComponent<InGameSettingPanel>();
        BoardAnimation();
        AudioManager.Instance.SetBGM(1);
    }

    private void BoardAnimation()
    {
        Board.transform.localScale = new Vector3(0f, 0f, 0f);
        Board.SetActive(true);
        var scaleHash = new Hashtable();
        Board.transform.DOScale(new Vector3(1f,0.05f,1f),1.5f).SetDelay(0.75f).SetEase(Ease.OutBack)
            .OnComplete(() => PieceAnimation());
        /*
        scaleHash.Add("x", 1f);
        scaleHash.Add("y", 0.05f);
        scaleHash.Add("z", 1f);
        scaleHash.Add("time", 1.5f);
        scaleHash.Add("delay", 0.75f);
        scaleHash.Add("easeType", "easeOutBack");
        scaleHash.Add("oncomplete", "PieceAnimation");
        scaleHash.Add("oncompletetarget", this.gameObject);
        iTween.ScaleTo(Board, scaleHash);
        */
    }

    private void PieceAnimation()
    {
        StartCoroutine(SetPiece());
        //StartCoroutine(SetSound());
    }

    WaitForSeconds wait = new WaitForSeconds(0.05f);
    WaitForSeconds wait2 = new WaitForSeconds(0.1f);
    WaitForSeconds wait3 = new WaitForSeconds(0.3f);
    private IEnumerator SetPiece()
    {
        Vector3 plusY = new Vector3(0, 0.075f, 0);

        var shakeHash = new Hashtable();
        shakeHash.Add("y", 1f);
        shakeHash.Add("x", 1f);
        shakeHash.Add("z", 1f);
        shakeHash.Add("time", 0.05f);
        shakeHash.Add("delay", 0.35f);

        yield return wait3;
        for (int i = AllyPiece.Length - 1; i > -1; i--)
        {
            AllyPiece[i].transform.localPosition += plusY;
            EnemyPiece[i].transform.localPosition += plusY;
            yield return wait;
            AllyPiece[i].SetActive(true);
            iTween.MoveTo(AllyPiece[i], iTween.Hash("position", AllyPiece[i].transform.localPosition - plusY, "islocal", true, "time", 1/*, "oncomplete", "SetSound", "oncompletetarget", this.gameObject*/));
            iTween.ShakeRotation(AllyPiece[i], shakeHash);
            EnemyPiece[i].SetActive(true);
            iTween.MoveTo(EnemyPiece[i], iTween.Hash("position", EnemyPiece[i].transform.localPosition - plusY, "islocal", true, "time", 1));
            iTween.ShakeRotation(EnemyPiece[i], shakeHash);
            _allyAudio[i].PlayOneShot(_allyAudio[i].clip);
            _enemyAudio[i].PlayOneShot(_enemyAudio[i].clip);
            yield return wait;
            yield return wait3;
        }
        yield return wait3;
        _setupManager.SetState(GameSetupManager.PhaseState.DiceRoll);
        MugAnimation();
        yield return wait3;
        Destroy(this);
        yield break;
    }
    
    private IEnumerator SetSound()
    {
        for (int i = AllyPiece.Length - 1; i > -1; i--)
        {
            _allyAudio[i].PlayOneShot(_allyAudio[i].clip);
            _enemyAudio[i].PlayOneShot(_enemyAudio[i].clip);
            yield return wait3;
        }
    }

    private void MugAnimation()
    {
        AttributeCorrelationChart.SetActive(true);
        AttributeDiceMug.SetActive(true);
        AttributeDiceShakeTelop.SetActive(true);
        AttributeDiceShakeTelop.transform.DOScaleY(1f, 1f).SetEase(Ease.InQuart);
        AttributeDiceShakeTelop.transform.DOLocalMoveY(0.15f, 3f).SetDelay(1.5f);
        _text.DOFade(0f, 3f).SetDelay(1.5f).OnComplete(() => { AttributeDiceShakeTelop.SetActive(false); _inGameSettingPanel.ChangeActiveButton(); });

        _explanationText.ChangeActive(true);
        _explanationText.ChangeSize(2);
    }
}
