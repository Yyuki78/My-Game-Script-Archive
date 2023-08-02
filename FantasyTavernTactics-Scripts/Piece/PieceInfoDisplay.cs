using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;

public class PieceInfoDisplay : MonoBehaviour
{
    private StringBuilder sb = new StringBuilder();

    [SerializeField] GameObject InfoBoard;
    [SerializeField] GameObject pieceInfoDisplay;
    [SerializeField] TextMeshProUGUI _infoText;
    [SerializeField] Image moveRangeImage;
    [SerializeField] Image attackRangeImage;
    [SerializeField] Sprite[] _moveRange;
    [SerializeField] Sprite[] _attackRange;

    private TileManager _tileManager;
    private Coroutine playCoroutine;

    private string[] pieceInfoTexts = new string[12];
    private string[] roleTexts = new string[5];

    private PieceInfomation currentPieceInfo;

    void Start()
    {
        _tileManager = GetComponent<TileManager>();
        InfoBoard.SetActive(false);
        pieceInfoDisplay.SetActive(false);
        for (int i = 0; i < pieceInfoTexts.Length; i++)
            pieceInfoTexts[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "PieceInfoText" + (i + 1).ToString());
        for (int i = 0; i < roleTexts.Length; i++)
            roleTexts[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "Role" + (i + 1).ToString());
    }

    public void ShowPieceInfo(PieceInfomation info)
    {
        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }
        playCoroutine = StartCoroutine(showPieceInfo(info,false));
    }

    public void ChangePieceInfo(PieceInfomation info)//buffŽž‚ÉŒÄ‚Î‚ê‚é
    {
        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }
        playCoroutine = StartCoroutine(showPieceInfo(info, true));
    }

    public void InActiveDisplay()
    {
        InfoBoard.SetActive(false);
        pieceInfoDisplay.SetActive(false);
    }

    private IEnumerator showPieceInfo(PieceInfomation info, bool type)
    {
        yield return null;
        if(!type)
            currentPieceInfo = info;
        if (!pieceInfoDisplay.activeSelf)
        {
            InfoBoard.SetActive(true);
            pieceInfoDisplay.SetActive(true);
        }

        sb.Clear();
        yield return null;

        if (currentPieceInfo.Side)
            sb.Append(pieceInfoTexts[0] + "\n");
        else
            sb.Append(pieceInfoTexts[1] + "\n");
        switch (currentPieceInfo.Role)
        {
            case 0:
                sb.Append(pieceInfoTexts[2] + roleTexts[0] + "\n");
                break;
            case 1:
                sb.Append(pieceInfoTexts[2] + roleTexts[1] + "\n");
                break;
            case 2:
                sb.Append(pieceInfoTexts[2] + roleTexts[2] + "\n");
                break;
            case 3:
                sb.Append(pieceInfoTexts[2] + roleTexts[3] + "\n");
                break;
            case 4:
                sb.Append(pieceInfoTexts[2] + roleTexts[4] + "\n");
                break;
        }
        if (currentPieceInfo.HP > 60 && currentPieceInfo.HP <= 200)
            sb.Append("HP:<color=green>");
        else if (currentPieceInfo.HP > 30 && currentPieceInfo.HP <= 60)
            sb.Append("HP:<color=yellow>");
        else
            sb.Append("HP:<color=red>");
        sb.Append(currentPieceInfo.HP.ToString() + "/" + currentPieceInfo.MaxHP.ToString() + "</color>\n");

        yield return null;
        switch (currentPieceInfo.Attribute)
        {
            case -1:
                sb.Append(pieceInfoTexts[3] + "\n");
                break;
            case 0:
                sb.Append(pieceInfoTexts[4] + "\n");
                break;
            case 1:
                sb.Append(pieceInfoTexts[5] + "\n");
                break;
            case 2:
                sb.Append(pieceInfoTexts[6] + "\n");
                break;
            case 3:
                sb.Append(pieceInfoTexts[7] + "\n");
                break;
        }

        if (currentPieceInfo.Role == 2)
            sb.Append(pieceInfoTexts[8] + currentPieceInfo.Attack.ToString() + "\n");
        else
            sb.Append(pieceInfoTexts[9] + currentPieceInfo.Attack.ToString() + "\n");

        sb.Append(pieceInfoTexts[10] + currentPieceInfo.Defense.ToString() + "\n");
        sb.Append(pieceInfoTexts[11] + currentPieceInfo.MagicDefense.ToString());

        _infoText.text = sb.ToString();

        switch (currentPieceInfo.Role)
        {
            case 0:
                moveRangeImage.sprite = _moveRange[0];
                attackRangeImage.sprite = _attackRange[0];
                break;
            case 1:
                moveRangeImage.sprite = _moveRange[0];
                attackRangeImage.sprite = _attackRange[1];
                break;
            case 2:
                moveRangeImage.sprite = _moveRange[0];
                attackRangeImage.sprite = _attackRange[2];
                break;
            case 3:
                moveRangeImage.sprite = _moveRange[2];
                attackRangeImage.sprite = _attackRange[0];
                break;
            case 4:
                moveRangeImage.sprite = _moveRange[1];
                attackRangeImage.sprite = _attackRange[1];
                break;
        }
        if (_tileManager.isHalfPieceDown)
            moveRangeImage.sprite = _moveRange[2];

        moveRangeImage.SetNativeSize();
        attackRangeImage.SetNativeSize();

        yield break;
    }
}
