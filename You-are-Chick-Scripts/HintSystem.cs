using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class HintSystem : MonoBehaviour
{
    [SerializeField] GameObject[] HintButton = new GameObject[3];
    [SerializeField] [TextArea] string[] HintText = new string[3];
    [SerializeField] GameObject[] HintSign = new GameObject[3];
    private TextMeshProUGUI[] _text = new TextMeshProUGUI[3];
    private Coroutine coroutine;
    [SerializeField] GameObject dontTouchImage;

    void Start()
    {
        _text[0] = HintSign[0].GetComponentInChildren<TextMeshProUGUI>();
        _text[1] = HintSign[1].GetComponentInChildren<TextMeshProUGUI>();
        _text[2] = HintSign[2].GetComponentInChildren<TextMeshProUGUI>();
        coroutine = StartCoroutine(hint());
        HintButton[0].SetActive(false);
        HintButton[1].SetActive(false);
    }

    private IEnumerator hint()
    {
        yield return new WaitForSeconds(20f);
        HintButton[0].SetActive(true);

        yield return new WaitForSeconds(40f);
        HintButton[1].SetActive(true);
        yield return new WaitForSeconds(60f);
        dontTouchImage.SetActive(false);
    }

    public void ClickButton(int num)
    {
        if (num != 2)
            HintButton[num].SetActive(false);
        _text[num].text = HintText[num];
        HintSign[num].transform.DOLocalRotate(new Vector3(0, 0, 0), 2f);
        AudioManager.instance.SE(0);
    }
    
    public void stopHint()
    {
        StopCoroutine(coroutine);
    }
}
