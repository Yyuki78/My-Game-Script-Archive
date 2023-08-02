using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ScoreText : MonoBehaviour
{
    public bool IsActive => gameObject.activeSelf;
    [SerializeField] Transform Camera;
    private TextMeshProUGUI _text;
    private RectTransform rectTransform;
    private Transform _transform;

    private void Awake()
    {
        if (Camera == null)
            Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        _transform = GetComponent<Transform>();
    }

    public void Init(Vector3 pos, float score, int type = 0)
    {
        rectTransform.localPosition = pos;
        Vector3 p = Camera.position;
        p.y = rectTransform.position.y;
        rectTransform.LookAt(p);

        // DOTweenのシーケンスを作成
        var sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOScale(1f, 0.01f));
        if (type == 0)
            _text.color = new Color(1, 110f / 255f, 0, 1);
        else
            _text.color = new Color(0, 110f / 255f, 1, 1);
        if (score > 0)
            _text.text = "+" + score.ToString("f3");
        else
        {
            _text.color = new Color(1, 0, 0, 1);
            _text.text = "-" + score.ToString();
        }

        // 最初に拡大表示する
        sequence.Append(rectTransform.DOScale(1.2f, 0.5f));

        // 次に上へ移動させる
        sequence.Append(rectTransform.DOMoveY(0.1f, 1f).SetRelative());

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
