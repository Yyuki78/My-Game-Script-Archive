using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ClearPanel : MonoBehaviour
{
    public bool isFirstClear = false;
    public float clearTime;
    [SerializeField] Transform clearPanel;
    [SerializeField] Transform ClearText;
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] Transform[] Buttons;
    [SerializeField] TextMeshProUGUI _releaseText;
    void Start()
    {
        ClearText.gameObject.SetActive(false);
        _timerText.gameObject.SetActive(false);
        for(int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].gameObject.SetActive(false);
        }
        _releaseText.gameObject.SetActive(false);
        StartCoroutine(effect());
    }

    private IEnumerator effect()
    {
        clearPanel.localScale = new Vector3(1, 0, 1);
        ClearText.localScale = new Vector3(0, 0, 1);
        yield return null;
        clearPanel.gameObject.SetActive(true);
        clearPanel.DOScale(new Vector3(1, 1, 1), 0.75f);
        yield return new WaitForSeconds(1f);
        ClearText.gameObject.SetActive(true);
        ClearText.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.OutBack).SetEase(Ease.OutQuint);
        yield return new WaitForSeconds(0.75f);
        _timerText.text = "クリアタイム：" + clearTime.ToString("f2") + "秒";
        _timerText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        if (isFirstClear)
        {
            _releaseText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.25f);
        }
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].gameObject.SetActive(true);
        }
        yield break;
    }
}
