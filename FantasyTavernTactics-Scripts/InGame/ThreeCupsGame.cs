using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class ThreeCupsGame : MonoBehaviour
{
    [SerializeField] CupGrabBehavior[] Cups = new CupGrabBehavior[3];
    [SerializeField] TextMeshProUGUI[] NumberTexts = new TextMeshProUGUI[3];
    [SerializeField] GameObject[] Directions = new GameObject[3];

    private TurnManager _manager;

    private bool alreadyDecided = false;
    private bool isFirst = true;

    private bool isPlusMoveNum = false;

    private string cupSecretText;

    void Awake()
    {
        _manager = GetComponentInParent<TurnManager>();
        for (int i = 0; i < Cups.Length; i++)
        {
            Cups[i].gameObject.SetActive(false);
            NumberTexts[i].fontSize = 0.02f;
            NumberTexts[i].fontStyle = FontStyles.Underline;
            cupSecretText = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "CupBelowText");
            NumberTexts[i].gameObject.SetActive(false);
            Directions[i].gameObject.SetActive(false);
        }
        GetComponentInChildren<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    
    public void SetGame(bool isPlus)
    {
        alreadyDecided = false;
        isPlusMoveNum = isPlus;
        for (int i = 0; i < Cups.Length; i++)
        {
            Cups[i].gameObject.SetActive(true);
            NumberTexts[i].gameObject.SetActive(true);
            if(isFirst)
                Directions[i].gameObject.SetActive(true);
        }
        isFirst = false;
        cupSecretText = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "CupBelowText");
    }

    public void ShowResult(int num)
    {
        if (alreadyDecided) return;
        alreadyDecided = true;

        int[] moveNum = { 1, 2, 3 };
        if (isPlusMoveNum)
            moveNum = new int[]{ 2, 3, 4 };
        isPlusMoveNum = false;

        for (int i = 0; i < moveNum.Length; i++)
        {
            int temp = moveNum[i];
            int randomIndex = Random.Range(0, moveNum.Length);
            moveNum[i] = moveNum[randomIndex];
            moveNum[randomIndex] = temp;
        }

        for (int i = 0; i < Cups.Length; i++)
        {
            if (i == num)
                NumberTexts[i].text = moveNum[i].ToString();
            else
                NumberTexts[i].text = "<color=black>" + moveNum[i].ToString() + "</color>";
            NumberTexts[i].fontSize = 0.1f;
            NumberTexts[i].fontStyle = FontStyles.Normal;
        }

        for (int i = 0; i < Cups.Length; i++)
        {
            Cups[i].gameObject.layer = 7;
            if (Directions[i].activeSelf)
                Directions[i].gameObject.SetActive(false);
        }
        _manager.DecideMoveNum(moveNum[num]);
    }

    public void ResetGame()
    {
        StartCoroutine(resetGame());
    }

    private IEnumerator resetGame()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < Cups.Length; i++)
        {
            Cups[i].Finish();
        }
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < Cups.Length; i++)
        {
            Cups[i].gameObject.layer = 9;

            NumberTexts[i].fontSize = 0.02f;
            NumberTexts[i].fontStyle = FontStyles.Underline;
            NumberTexts[i].text = cupSecretText;
            NumberTexts[i].gameObject.SetActive(false);
        }
        yield break;
    }
}
