using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

public class ChangeTextLanguage : MonoBehaviour
{
    [SerializeField]
    string TextKey;

    private TextMeshPro _text;

    void Start()
    {
        _text = GetComponent<TextMeshPro>();
        string text = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "LocalizationText", tableEntryReference: TextKey);
        _text.text = text;
    }

    public void ChangeText()
    {
        string text = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "LocalizationText", tableEntryReference: TextKey);
        _text.text = text;
    }
}
