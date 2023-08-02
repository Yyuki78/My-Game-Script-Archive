using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Localization.Settings;

public class AttributeDetermination : MonoBehaviour
{
    //�����[���̊Ǘ��⑮������ɓn�����肷��
    //�_�C�X���[���I���O(Mug)�A�I����A�����[�����̋���(this)�A�U�鑮�������肵�����̋���(this)
    //������U�鎞�̋���(this)�A�U��I������ۂ̋���(this)�A�U��Ȃ����ۂ̋���(this)

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

    //�_�C�X���S�ĊO�ɏo���̂ŁA��x���������B��
    public void HideExplanationText()
    {
        _explanationText.ChangeActive(false);
        _explanationText.ChangeSize(0);
    }

    //�_�C�X���S�������ɏW�܂��Č��ʂ��\�������
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

    //�����[�����I�����ꂽ��
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

    //Confirm���I�����ꂽ��
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

        //�e���b�v�\��
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
            //����̐F��ύX����
            HandsMaterial.color = HandsChangeColor[attributes[0]];

            //���͂ނ��Ƃő�����^���邱�Ƃ��o����悤�ɂȂ�
            _setupManager.SetState(GameSetupManager.PhaseState.AttributeDetermination);

            //��������ύX����
            _explanationText.ChangeText(2);

            //��������\������
            _attributeNumText2.gameObject.SetActive(true);
        }
    }

    //�����_���ɑ�����U��
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

    //��͂܂ꂽ��
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
            //�I��
            HandsMaterial.color = HandsChangeColor[3];
            AttributeSelectConfirmButton.SetActive(true);
            return;
        }
        
        //����̐F��ύX����
        HandsMaterial.color = HandsChangeColor[attributes[selectNumber]];
    }

    //��߂邪�����ꂽ��
    public void RedoAttributeSelect()
    {
        if (_setupManager._currentState != GameSetupManager.PhaseState.AttributeDetermination) return;
        if (selectNumber <= 0) return;
        if (AttributeSelectConfirmButton.activeSelf)
            AttributeSelectConfirmButton.SetActive(false);

        AudioManager.Instance.SE(1);

        selectNumber--;
        AlreadySelectedPieceInfomation[selectNumber].ResetAttribute();
        //����̐F��ύX����
        HandsMaterial.color = HandsChangeColor[attributes[selectNumber]];

        if (selectNumber <= 0)
            AttributeSelectUndoButton.SetActive(false);
    }

    //�����I���m��{�^���������ꂽ��
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
