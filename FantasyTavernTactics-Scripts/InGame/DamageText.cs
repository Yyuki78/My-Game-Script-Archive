using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    public bool IsActive => gameObject.activeSelf;
    private Transform Camera;
    private Image _image;
    private TextMeshProUGUI _text;
    private RectTransform rectTransform;

    private void Awake()
    {
        if (Camera == null)
            Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }

    Color resetCol = new Color(1, 1, 1, 1);
    public void Init(Vector3 pos, int score, Sprite sprite)
    {
        rectTransform.localPosition = pos;
        Vector3 p = Camera.position;
        p.y = rectTransform.position.y;
        rectTransform.LookAt(p);

        _image.sprite = sprite;

        // DOTweenのシーケンスを作成
        var sequence = DOTween.Sequence();
        _text.color = resetCol;
        _image.color = resetCol;
        sequence.Append(rectTransform.DOScale(0.8f, 0.01f));

        _text.text = score.ToString();

        // 最初に拡大表示する
        sequence.Append(rectTransform.DOScale(1.0f, 0.5f));

        // 次に上へ移動させる
        sequence.Append(rectTransform.DOMoveY(0.05f, 1f).SetRelative());

        // 現在の色を取得
        var color = _text.color;

        // アルファ値を0に指定して文字を透明にする
        color.a = 0.0f;

        // 上に移動と同時に半透明にして消えるようにする
        sequence.Join(DOTween.To(() => _text.color, c => _text.color = c, color, 1f).SetEase(Ease.InOutQuart));
        sequence.Join(DOTween.To(() => _image.color, c => _image.color = c, color, 1f).SetEase(Ease.InOutQuart));

        // すべてのアニメーションが終わったら、自分自身を削除する
        sequence.OnComplete(() => gameObject.SetActive(false));
    }
}
