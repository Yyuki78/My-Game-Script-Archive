using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageSelectSystem : MonoBehaviour
{
    [SerializeField] GameObject[] ClearMark;
    [SerializeField] GameObject[] dontClick;
    [SerializeField] TextMeshProUGUI[] _text;
    private Color Black = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1);
    private Color Gray = new Color(170f / 255f, 170f / 255f, 170f / 255f, 1);

    void Awake()
    {
        isClear(0);
        for (int i = 1; i < 10; i++)
        {
            isClear(i);
            Judge(i);
        }
    }

    private void isClear(int num)
    {
        if (GameManager.isClearStage[num])
            ClearMark[num].SetActive(true);
        else
            ClearMark[num].SetActive(false);
    }
    
    private void Judge(int num)
    {
        if (GameManager.isClearStage[num - 1])
        {
            _text[num - 1].color = Black;
            dontClick[num - 1].SetActive(false);
        }
        else
        {
            _text[num - 1].color = Gray;
            dontClick[num - 1].SetActive(true);
        }
    }
}
