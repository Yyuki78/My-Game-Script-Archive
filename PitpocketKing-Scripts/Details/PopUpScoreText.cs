using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpScoreText : MonoBehaviour
{
    public int score = 0;
    public int comboNum = 0;
    public int isInaLow = 0;

    private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(popUp());
    }

    private IEnumerator popUp()
    {
        _text.text = "";
        var col = _text.color;

        yield return new WaitForSeconds(0.1f);

        if (comboNum >= 2)
            _text.text = "+" + score + " ×" + (1 + Mathf.Min(comboNum - 1, 10) * 0.3f) + "倍";
        else
            _text.text = "+" + score;

        if (isInaLow != 0)
        {
            _text.text = "+" + score + " ×" + ((1 + Mathf.Min(comboNum - 1, 10) * 0.3f) * (Mathf.Pow(2, isInaLow))) + "倍";
            _text.color = new Color(1, 0, 0, 1);
        }


        if (score < 0)
        {
            _text.text = "" + score;
            _text.color = col;
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 100; i++)
        {
            transform.localPosition += new Vector3(0, 0.1f, 0);
            _text.color -= new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.015f);
        }

        _text.text = "";

        Destroy(gameObject);
    }
}
