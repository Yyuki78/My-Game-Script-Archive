using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FillButton : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] Stage5Controller _controller;

    private const float DURATION = 1.5f;

    private void Awake()
    {
        fillImage.fillAmount = 0;
    }

    public void OnClick()
    {
        AudioManager.instance.SE(14);
        fillImage.DOFillAmount(1, DURATION)
            .OnComplete(() =>
            {
                _controller.GateOpen();
            });
    }
}