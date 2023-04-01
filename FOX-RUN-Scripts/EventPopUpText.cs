using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EventPopUpText : MonoBehaviour
{
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = this.gameObject.GetComponent<Text>();
        // DOTweenのシーケンスを作成
        var sequence = DOTween.Sequence();

        // 最初に拡大表示する
        sequence.Append(transform.DOScale(1.2f, 0.5f));

        // 次に上へ移動させる
        sequence.Append(transform.DOMoveY(5.0f, 1.25f).SetRelative());

        // 現在の色を取得
        var color = text.color;

        // アルファ値を0に指定して文字を透明にする
        color.a = 0.0f;

        // 上に移動と同時に半透明にして消えるようにする
        sequence.Join(DOTween.To(() => text.color, c => text.color = c, color, 2.5f).SetEase(Ease.InOutQuart));

        // すべてのアニメーションが終わったら、自分自身を削除する
        sequence.OnComplete(() => Destroy(gameObject));
    }
}
