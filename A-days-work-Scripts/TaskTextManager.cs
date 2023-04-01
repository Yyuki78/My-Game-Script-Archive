using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

public class TaskTextManager : MonoBehaviour
{
    [SerializeField] [TextArea] string[] sTaskText;
    [SerializeField] [TextArea] string[] mTaskText;
    [SerializeField] [TextArea] string[] lTaskText;

    [SerializeField] TextMeshProUGUI _currentTaskText;
    [SerializeField] TextMeshProUGUI _finishTaskText;

    private int[] TaskNums = new int[4];

    public void set()
    {
        StartCoroutine(setTaskText());
    }

    private IEnumerator setTaskText()
    {
        var wait = new WaitForSeconds(0.01f);
        for (int i = 0; i < 10; i++)
        {
            sTaskText[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "LocalizationText", tableEntryReference: "st" + (i + 1).ToString());
            yield return wait;
        }
        for (int i = 0; i < 6; i++)
        {
            mTaskText[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "LocalizationText", tableEntryReference: "mt" + (i + 1).ToString());
            yield return wait;
        }
        for (int i = 0; i < 5; i++)
        {
            lTaskText[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "LocalizationText", tableEntryReference: "lt" + (i + 1).ToString());
            yield return wait;
        }
        yield break;
    }

    public void Prepare(int Snum1, int Snum2, int Mnum, int Lnum)
    {
        TaskNums[0] = Snum1; TaskNums[1] = Snum2; TaskNums[2] = Mnum; TaskNums[3] = Lnum;
        _currentTaskText.text = sTaskText[Snum1] + sTaskText[Snum2] + mTaskText[Mnum] + lTaskText[Lnum];
    }

    public void FinishTask(int finishTaskType,int nextTaskNum)
    {
        //finishTaskType‚Í0`9‚Íshort,11‚Ímiddle,12‚Ílong
        //nextTaskNum‚ª100‚ÌŽž‚Í’Ç‰Á‚·‚éƒ^ƒXƒN–³‚µ
        if (nextTaskNum != 100)
        {
            if (finishTaskType < 10)
            {
                if(TaskNums[0]== finishTaskType)
                {
                    TaskNums[0] = nextTaskNum;
                }
                else
                {
                    TaskNums[1] = nextTaskNum;
                }
                _finishTaskText.text += sTaskText[finishTaskType];
            }else if (finishTaskType == 11)
            {
                _finishTaskText.text += mTaskText[TaskNums[2]];
                TaskNums[2] = nextTaskNum;
            }
            else if (finishTaskType == 12)
            {
                _finishTaskText.text += lTaskText[TaskNums[3]];
                TaskNums[3] = nextTaskNum;
            }
        }
        else
        {
            if (finishTaskType < 10)
            {
                if (TaskNums[0] == finishTaskType)
                {
                    TaskNums[0] = 10;
                }
                else
                {
                    TaskNums[1] = 10;
                }
                _finishTaskText.text += sTaskText[finishTaskType];
            }
            else if (finishTaskType == 11)
            {
                _finishTaskText.text += mTaskText[TaskNums[2]];
                TaskNums[2] = 6;
            }
            else if (finishTaskType == 12)
            {
                _finishTaskText.text += lTaskText[TaskNums[3]];
                TaskNums[3] = 5;
            }
        }
        _currentTaskText.text = sTaskText[TaskNums[0]] + sTaskText[TaskNums[1]] + mTaskText[TaskNums[2]] + lTaskText[TaskNums[3]];
    }
}
