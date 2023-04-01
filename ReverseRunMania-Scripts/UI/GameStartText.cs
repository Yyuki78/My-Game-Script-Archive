using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameStartText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.text = "";
        _text.fontSize = 96;
        StartCoroutine(StartEffect());
    }

    private IEnumerator StartEffect()
    {
        var wait = new WaitForSeconds(0.9f);
        yield return new WaitForSeconds(0.4f);
        StartText();
        yield return new WaitForSeconds(0.2f);
        AudioManager.instance.SE(19);
        _text.text = "3";
        yield return wait;
        _text.text = "2";
        StartText();
        yield return wait;
        _text.text = "1";
        StartText();
        yield return wait;
        _text.fontSize = 64;
        _text.text = "ゲームスタート！";
        LastStartText();

        yield break;
    }
    
    private void StartText()
    {
        // DOTweenのシーケンスを作成
        var sequence = DOTween.Sequence()
            .Append(transform.DOScale(0.5f, 0.01f))
            .Join(transform.DORotate(new Vector3(0, 90, 0), 0.01f))
            .AppendInterval(0.01f)

            .Append(transform.DOScale(1f, 0.15f))
            .SetEase(Ease.OutBack)
            .Join(transform.DORotate(new Vector3(0, 0, 0), 0.075f))
            .SetEase(Ease.OutQuad)
            .AppendInterval(0.4f)

            .Append(transform.DOScale(0.5f, 0.15f))
            .Join(transform.DORotate(new Vector3(0, 90, 0), 0.15f))
            .SetEase(Ease.InQuad);
    }

    private void LastStartText()
    {
        // DOTweenのシーケンスを作成
        var sequence = DOTween.Sequence()
            .Append(transform.DOScale(0.5f, 0.01f))
            .Join(transform.DORotate(new Vector3(0, 90, 0), 0.01f))
            .AppendInterval(0.01f)

            .Append(transform.DOScale(1f, 0.15f))
            .SetEase(Ease.OutBack)
            .Join(transform.DORotate(new Vector3(0, 0, 0), 0.075f))
            .SetEase(Ease.OutQuad)
            .AppendInterval(0.6f)

            .Append(transform.DOLocalMoveX(-1000f, 1.5f))
            .SetEase(Ease.InQuad);

        sequence.OnComplete(() => Destroy(gameObject));
    }
}
