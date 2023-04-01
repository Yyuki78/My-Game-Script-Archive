using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DictionaryManager : MonoBehaviour
{
    private int currentPanelNum = 0;

    [SerializeField] GameObject[] DictionaryPanel;

    [SerializeField] Image[] buttonImage;
    [SerializeField] Sprite[] buttonSprite;

    [SerializeField] Image[] _image;
    [SerializeField] Sprite[] dictionarySprite;
    [SerializeField] TextMeshProUGUI[] _nameText;
    [SerializeField] [TextArea] string[] nameString;
    [SerializeField] TextMeshProUGUI[] _explanationText;
    [SerializeField] [TextArea] string[] explanationString1;
    [SerializeField] [TextArea] string[] explanationString2;
    [SerializeField] string[] saveName;
    private int itemNum = 13;

    void Start()
    {
        for(int i = 0; i < _image.Length; i++)
        {
            if (!PlayerPrefs.HasKey(saveName[i]))
                _image[i].sprite = dictionarySprite[_image.Length];
            else
                _image[i].sprite = dictionarySprite[i];
        }
        for (int i = 0; i < saveName.Length; i++)
        {
            if (!PlayerPrefs.HasKey(saveName[i]))
            {
                _nameText[i].text = "???";
                if (i >= itemNum)
                {
                    _explanationText[i - itemNum].text = explanationString1[i - itemNum];
                }
            }
            else
            {
                _nameText[i].text = nameString[i];
                if (i >= itemNum)
                {
                    _explanationText[i - itemNum].text = explanationString2[i - itemNum];
                }
            }
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < DictionaryPanel.Length; i++)
        {
            DictionaryPanel[i].SetActive(false);
        }
        DictionaryPanel[currentPanelNum].SetActive(true);
    }

    //ページ変更ボタン
    public void OnClickChangePage(int num)
    {
        AudioManager.instance.SE(20);
        currentPanelNum += num;
        if (currentPanelNum == -1 || currentPanelNum == 4)
            currentPanelNum -= num;

        buttonImage[0].sprite = buttonSprite[0];
        buttonImage[1].sprite = buttonSprite[0];
        if (currentPanelNum == 0)
            buttonImage[0].sprite = buttonSprite[1];
        if (currentPanelNum == 3)
            buttonImage[1].sprite = buttonSprite[1];

        for (int i = 0; i < DictionaryPanel.Length; i++)
        {
            DictionaryPanel[i].SetActive(false);
        }
        DictionaryPanel[currentPanelNum].SetActive(true);
    }
}
