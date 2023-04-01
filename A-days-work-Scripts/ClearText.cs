using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class ClearText : MonoBehaviour
{
    [SerializeField] TextMeshPro _text;
    private TaskManager _manager;
    private string rank;
    private int clearMinute = 0;
    private int clearSecond = 0;

    // Start is called before the first frame update
    void Awake()
    {
        _manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TaskManager>();

        if (_manager.ElapsedTime < 120)
        {
            rank = "X";
        }
        else if (_manager.ElapsedTime < 150)
        {
            rank = "S+";
        }
        else if (_manager.ElapsedTime < 180)
        {
            rank = "S";
        }
        else if (_manager.ElapsedTime < 210)
        {
            rank = "S-";
        }
        else if (_manager.ElapsedTime < 240)
        {
            rank = "A+";
        }
        else if (_manager.ElapsedTime < 270)
        {
            rank = "A";
        }
        else if (_manager.ElapsedTime < 300)
        {
            rank = "A-";
        }
        else if (_manager.ElapsedTime < 360)
        {
            rank = "B+";
        }
        else if (_manager.ElapsedTime < 420)
        {
            rank = "B";
        }
        else if (_manager.ElapsedTime < 480)
        {
            rank = "B-";
        }
        else
        {
            rank = "C";
        }

        clearMinute = (int)_manager.ElapsedTime / 60;
        clearSecond = (int)_manager.ElapsedTime % 60;

        _text.text = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "LocalizationText", tableEntryReference: "ResultText1")
            + clearMinute + LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "LocalizationText", tableEntryReference: "ResultText2")
            + clearSecond + LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "LocalizationText", tableEntryReference: "ResultText3")
            + rank + LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "LocalizationText", tableEntryReference: "ResultText4");
    }
}
