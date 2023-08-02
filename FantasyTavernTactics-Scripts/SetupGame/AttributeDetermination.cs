using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Localization.Settings;

public class AttributeDetermination : MonoBehaviour
{
    //リロールの管理や属性を駒に渡したりする
    //ダイスロール終了前(Mug)、終了後、リロール時の挙動(this)、振る属性が決定した時の挙動(this)
    //属性を振る時の挙動(this)、振り終わった際の挙動(this)、振りなおす際の挙動(this)

    private int redNum, blueNum, greenNum;
    private int[] attributes;

    private GameSetupManager _setupManager;
    private ExplanationText _explanationText;
    private GameInfomation _gameInfomation;

    [SerializeField] GameObject AttributeResults;
    [SerializeField] GameObject RerollButton;
    [SerializeField] TextMeshProUGUI _attributeNumText;
    [SerializeField] TextMeshProUGUI _attributeNumText2;

    private int rerollNum = 2;
    [SerializeField] TextMeshProUGUI _rerollCountText;
    [SerializeField] GameObject AttributeDiceMug;
    [SerializeField] GameObject AttributeDiceMugPrefab;

    [SerializeField] GameObject AttributeAllocationTelop;
    private TextMeshProUGUI _attributeAllocationText;

    private string[] attributeDeterminationTexts = new string[5];

    [SerializeField] Material HandsMaterial;
    [SerializeField] Color[] HandsChangeColor;

    private PieceInfomation[] AlreadySelectedPieceInfomation = new PieceInfomation[10];
    private int selectNumber = 0;

    [SerializeField] GameObject AttributeSelectUndoButton;
    [SerializeField] GameObject AttributeSelectConfirmButton;

    void Start()
    {
        _setupManager = GetComponent<GameSetupManager>();
        _explanationText = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExplanationText>();
        _gameInfomation = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>();
        AttributeResults.SetActive(false);
        _attributeAllocationText = AttributeAllocationTelop.GetComponent<TextMeshProUGUI>();
        AttributeAllocationTelop.SetActive(false);
        AttributeAllocationTelop.transform.localScale = new Vector3(1, 0, 1);
        AttributeSelectUndoButton.SetActive(false);
        AttributeSelectConfirmButton.SetActive(false);
        for (int i = 0; i < attributeDeterminationTexts.Length; i++)
            attributeDeterminationTexts[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "AttributeDeterminationText" + (i + 1).ToString());
    }

    //ダイスが全て外に出たので、一度説明文を隠す
    public void HideExplanationText()
    {
        _explanationText.ChangeActive(false);
        _explanationText.ChangeSize(0);
    }

    //ダイスが全部中央に集まって結果が表示される
    public void ShowDiceResult(int red,int blue,int green)
    {
        redNum = red; blueNum = blue; greenNum = green;
        _explanationText.ChangeText(1);
        _explanationText.ChangeActive(true);
        _explanationText.ChangeSize(2);
        _attributeNumText.text = attributeDeterminationTexts[0] + redNum + attributeDeterminationTexts[1] + blueNum + attributeDeterminationTexts[2] + greenNum;
        _attributeNumText2.text = _attributeNumText.text;
        _rerollCountText.text = attributeDeterminationTexts[3] + rerollNum;

        AttributeResults.SetActive(true);
    }

    //リロールが選択された時
    public void RerollDice()
    {
        AudioManager.Instance.SE(1);
        AttributeResults.SetActive(false);

        rerollNum--;
        if (rerollNum <= 0)
            RerollButton.SetActive(false);

        Destroy(AttributeDiceMug);
        GameObject[] dices = GameObject.FindGameObjectsWithTag("AttributeDice");
        for (int i = 0; i < dices.Length; i++)
            Destroy(dices[i]);
        AttributeDiceMug = Instantiate(AttributeDiceMugPrefab, transform);
    }

    //Confirmが選択された時
    public void ConfirmAttribute()
    {
        AudioManager.Instance.SE(0);
        AttributeResults.SetActive(false);
        Destroy(AttributeDiceMug);
        GameObject[] dices = GameObject.FindGameObjectsWithTag("AttributeDice");
        for (int i = 0; i < dices.Length; i++)
            Destroy(dices[i]);

        if (_gameInfomation.isRandomAttribute)
            _attributeAllocationText.text = attributeDeterminationTexts[4];
        else
            _attributeAllocationText.text = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "GameSettingTelop2");

        //テロップ表示
        AttributeAllocationTelop.SetActive(true);
        AttributeAllocationTelop.transform.DOScaleY(1f, 1f).SetEase(Ease.InQuart).OnComplete(() => { SetSelectPiecePhase(); });
        AttributeAllocationTelop.transform.DOLocalMoveY(0.15f, 3f).SetDelay(1.5f);
        var text = AttributeAllocationTelop.GetComponent<TextMeshProUGUI>();
        text.DOFade(0f, 3f).SetDelay(1.5f).OnComplete(() => { AttributeAllocationTelop.SetActive(false); });
    }

    private void SetSelectPiecePhase()
    {
        attributes = new int[redNum + blueNum + greenNum];
        for (int i = 0; i < redNum; i++)
            attributes[i] = 0;
        for (int i = redNum; i < redNum + blueNum; i++)
            attributes[i] = 1;
        for (int i = redNum + blueNum; i < attributes.Length; i++)
            attributes[i] = 2;

        if (_gameInfomation.isRandomAttribute)
        {
            StartCoroutine(randomAttribute(attributes));
        }
        else
        {
            //両手の色を変更する
            HandsMaterial.color = HandsChangeColor[attributes[0]];

            //駒を掴むことで属性を与えることが出来るようになる
            _setupManager.SetState(GameSetupManager.PhaseState.AttributeDetermination);

            //説明文を変更する
            _explanationText.ChangeText(2);

            //属性数を表示する
            _attributeNumText2.gameObject.SetActive(true);
        }
    }

    //ランダムに属性を振る
    WaitForSeconds wait = new WaitForSeconds(0.25f);
    private IEnumerator randomAttribute(int[] attributes)
    {
        _explanationText.ChangeActive(false);
        _explanationText.ChangeSize(0);

        List<PieceInfomation> assignPiece = new List<PieceInfomation>();
        PieceInfomation[] allPiece = GetComponentsInChildren<PieceInfomation>();
        yield return null;
        for(int i = 0; i < allPiece.Length; i++)
        {
            if (!allPiece[i].Side) continue;
            if (allPiece[i].Role == 4) continue;
            assignPiece.Add(allPiece[i]);
        }
        yield return null;
        for (var i = attributes.Length - 1; i > 0; --i)
        {
            int ran = Random.Range(0, i + 1);
            var tmp = attributes[i];
            attributes[i] = attributes[ran];
            attributes[ran] = tmp;
        }
        yield return null;
        for (int i = 0; i < assignPiece.Count; i++)
        {
            assignPiece[i].SetAttribute(attributes[i]);
            yield return wait;
        }
        AudioManager.Instance.SE(24);
        if (_gameInfomation.isReduceYourPiece != 0)
            _setupManager.SetState(GameSetupManager.PhaseState.ReducePiece);
        else
            _setupManager.SetState(GameSetupManager.PhaseState.MyPieceDetermination);
        yield break;
    }

    //駒が掴まれた時
    public void GrabPiece(PieceInfomation info)
    {
        if (_setupManager._currentState != GameSetupManager.PhaseState.AttributeDetermination) return;
        if (selectNumber == 0)
            AttributeSelectUndoButton.SetActive(true);
        info.SetAttribute(attributes[selectNumber]);
        AlreadySelectedPieceInfomation[selectNumber] = info;
        selectNumber++;

        if (selectNumber == attributes.Length)
        {
            //終了
            HandsMaterial.color = HandsChangeColor[3];
            AttributeSelectConfirmButton.SetActive(true);
            return;
        }
        
        //両手の色を変更する
        HandsMaterial.color = HandsChangeColor[attributes[selectNumber]];
    }

    //一つ戻るが押された時
    public void RedoAttributeSelect()
    {
        if (_setupManager._currentState != GameSetupManager.PhaseState.AttributeDetermination) return;
        if (selectNumber <= 0) return;
        if (AttributeSelectConfirmButton.activeSelf)
            AttributeSelectConfirmButton.SetActive(false);

        AudioManager.Instance.SE(1);

        selectNumber--;
        AlreadySelectedPieceInfomation[selectNumber].ResetAttribute();
        //両手の色を変更する
        HandsMaterial.color = HandsChangeColor[attributes[selectNumber]];

        if (selectNumber <= 0)
            AttributeSelectUndoButton.SetActive(false);
    }

    //属性選択確定ボタンが押された時
    public void FinishAttributeSelect()
    {
        AudioManager.Instance.SE(0);
        AudioManager.Instance.SE(24);
        AttributeSelectConfirmButton.SetActive(false);
        AttributeSelectUndoButton.SetActive(false);
        _explanationText.ChangeActive(false);
        _explanationText.ChangeSize(0);
        _attributeNumText2.gameObject.SetActive(false);
        if (_gameInfomation.isReduceYourPiece != 0)
            _setupManager.SetState(GameSetupManager.PhaseState.ReducePiece);
        else
            _setupManager.SetState(GameSetupManager.PhaseState.MyPieceDetermination);
    }
}
