using UnityEngine;
using TMPro;
using DG.Tweening;

public class AchieveNotification : MonoBehaviour
{
    public bool isAchieve = false;

    private Vector3 pos = new Vector3(-200f, -50f, 0f);

    private TextMeshProUGUI _text;
    private CanvasGroup _canvasGroup;
    private Sequence m_Sequence;

    private void Start()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    public void PlayNotification(string Text)
    {
        isAchieve = true;

        m_Sequence?.Kill();
        _text.text = "トロフィーを獲得しました！\n     " + Text;

        m_Sequence = DOTween.Sequence()
            .OnStart(() =>
            {
                _canvasGroup.transform.localPosition = pos;
                _canvasGroup.alpha = 1f;
            })
            .Append(_canvasGroup.transform.DOLocalMoveX(-50, 0.5f).SetEase(Ease.OutQuart))
            .AppendInterval(2.5f)
            .Append(_canvasGroup.DOFade(0f, 0.7f));

        m_Sequence.Play();
    }
}