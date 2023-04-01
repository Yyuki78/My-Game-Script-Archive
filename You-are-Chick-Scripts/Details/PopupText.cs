using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopupText : MonoBehaviour
{
    [SerializeField] bool isStartEffect = true;
    private Transform effect;
    void Start()
    {
        effect = transform.GetChild(0);
        transform.localScale = new Vector3(0, 0, 0);
        effect.transform.localScale = new Vector3(0, 0, 0);
        if (isStartEffect)
            popUpText();
    }

    public void popUpText()
    {
        transform.localScale = new Vector3(0, 0, 0);
        effect.transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(new Vector3(1, 1, 1), 0.3f).SetEase(Ease.OutBack).SetEase(Ease.OutQuint);
        effect.transform.DOScale(new Vector3(1, 1, 1), 0.4f).SetEase(Ease.OutBack).SetEase(Ease.OutQuint);
        effect.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f).SetDelay(0.4f);
        effect.transform.DOScale(new Vector3(0, 0, 0), 0.01f).SetDelay(0.6f);
    }
}
