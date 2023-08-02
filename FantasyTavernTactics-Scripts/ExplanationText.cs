using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Localization.Settings;

public class ExplanationText : MonoBehaviour
{
    [SerializeField] bool isShowExplanation = true;
    private int setFirst;

    [SerializeField] [TextArea] string[] AllTexts;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] TextMeshProUGUI _confirmText;

    void Start()
    {
        setFirst = Random.Range(0, 5);
    }

    public void ChangeText(int num)
    {
        if (num == 5 || num == 8)
        {
            _text.text = "";
            _confirmText.gameObject.SetActive(true);
            _confirmText.text = AllTexts[num];
            _confirmText.fontSize = 0.036f;
            return;
        }
        else
            _confirmText.text = "";
        if (num == 0)//ÉQÅ[ÉÄäJénå„èâÇﬂÇ…åƒÇŒÇÍÇÈ
        {
            for (int i = 0; i < AllTexts.Length; i++)
                AllTexts[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "InGameExplanationText" + (i + 1).ToString());
        }
        if (!isShowExplanation) return;
        _text.text = AllTexts[num];
        switch (num)
        {
            case 0:
            case 5:
                _text.fontSize = 0.04f;
                break;
            case 4:
                _text.fontSize = 0.0284f;
                break;
            default:
                _text.fontSize = 0.03f;
                break;
        }
    }

    public void ChangeActive(bool active)
    {
        if (_confirmText.gameObject.activeSelf)
            _confirmText.gameObject.SetActive(false);
        if (!isShowExplanation) return;
        _text.gameObject.SetActive(active);
    }

    public void ChangeSize(int num)
    {
        if (!isShowExplanation) return;
        switch (num)
        {
            case 0:
                _text.gameObject.transform.localScale = new Vector3(1, 0, 1);
                break;
            case 1:
                _text.gameObject.transform.DOScaleY(1f, 0.75f).SetEase(Ease.InQuart).SetDelay(1.5f);
                break;
            case 2:
                _text.gameObject.transform.DOScaleY(1f, 0.75f).SetEase(Ease.InQuart).SetDelay(0.5f);
                break;
        }
    }

    public void ShowTips()
    {
        if (!isShowExplanation) return;
    }

    public void OffExplanationText()
    {
        isShowExplanation = false;
        _text.gameObject.SetActive(false);
    }
}
