using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GetCoinText : MonoBehaviour
{
    public bool IsActive => gameObject.activeSelf;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void Init(int num)
    {
        // DOTweenのシーケンスを作成
        var sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(1f, 0.01f));
        sequence.Append(transform.DOMoveY(-10.0f, 0.01f).SetRelative());
        if (num > 0)
        {
            _text.text = "+" + num.ToString();
            _text.color = new Color(1, 1, 0, 1);
        }
        else
        {
            _text.text = num.ToString();
            _text.color = new Color(1, 1, 1, 1);
        }

        // 最初に拡大表示する
        sequence.Append(transform.DOScale(1.2f, 0.25f));

        // 次に上へ移動させる
        sequence.Append(transform.DOMoveY(10.0f, 0.5f).SetRelative());

        // 現在の色を取得
        var color = _text.color;

        // アルファ値を0に指定して文字を透明にする
        color.a = 0.0f;

        // 上に移動と同時に半透明にして消えるようにする
        sequence.Join(DOTween.To(() => _text.color, c => _text.color = c, color, 1f).SetEase(Ease.InOutQuart));

        // すべてのアニメーションが終わったら、自分自身を削除する
        sequence.OnComplete(() => gameObject.SetActive(false));
    }
}
