using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class SlideInOut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var rectTransform = GetComponent<RectTransform>();

        var sequence = DOTween.Sequence();

        sequence.Append(rectTransform.DOLocalMoveX(0.0f, 1.0f));

        sequence.AppendInterval(3.0f);

        sequence.Append(rectTransform.DOLocalMoveX(-1000.0f, 1.0f));
    }
}
