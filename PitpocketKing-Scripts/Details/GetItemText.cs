using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetItemText : MonoBehaviour
{
    [SerializeField] int Number = 0;
    private int textType;

    public bool isUsed { get; private set; } = false;

    private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();

        ResetText();
    }

    public void ResetText()
    {
        _text.text = "";
        _text.color = new Color(0, 0, 0, 0);
        transform.localScale = new Vector3(0f, 0f, 0f);
        transform.localPosition = new Vector3(0, 200, 0);
    }

    public void GetItem(int type)
    {
        textType = type;
        StartCoroutine(TextAnimation());
    }

    private IEnumerator TextAnimation()
    {
        isUsed = true;
        switch (textType)
        {
            case 0:
                _text.color = new Color(0, 1, 1, 1);
                _text.text = "獲得金額がアップします！";
                break;
            case 1:
                _text.color = new Color(0, 1, 0, 1);
                _text.text = "歩行者の数が増加します！";
                break;
            case 2:
                _text.color = new Color(1, 0.5f, 0, 1);
                _text.text = "超お金持ちが出現します！";
                break;
            default:
                break;
        }
        for(int i = 0; i < 55; i++)
        {
            transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
        for(int i = 0; i < 5; i++)
        {
            transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(2.5f);
        float reachHight;
        reachHight = 17 - (Number * 3);

        for (int i = 0; i < 100; i++)
        {
            transform.localScale -= new Vector3(0.006f, 0.006f, 0.006f);
            transform.localPosition += new Vector3(49f / 10f, reachHight / 10f, 0);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(10f);

        ResetText();
        isUsed = false;
        yield break;
    }
}
