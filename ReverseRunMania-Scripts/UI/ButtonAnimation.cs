using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] int ButtonMode = 0;

    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (transform == null || _canvasGroup == null) return;
        switch (ButtonMode)
        {
            case 0:
                transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(0.8f, 0.24f).SetEase(Ease.OutCubic);

                AudioManager.instance.SE(20);
                break;
            case 1: 
                transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(0.8f, 0.24f).SetEase(Ease.OutCubic);

                AudioManager.instance.SE(20);
                break;
            case 2:
                transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(0.8f, 0.24f).SetEase(Ease.OutCubic);

                AudioManager.instance.SE(20);
                break;
            default:
                break;
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (transform == null || _canvasGroup == null) return;
        switch (ButtonMode)
        {
            case 0:
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            case 1: 
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);

                _canvasGroup.DOFade(0f, 1.5f).SetEase(Ease.OutCubic);

                StartCoroutine(Wait());
                break;
            case 2:
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            default:
                break;
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (transform == null || _canvasGroup == null) return;
        switch (ButtonMode)
        {
            case 0:
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            case 1: 
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);

                _canvasGroup.DOFade(0f, 1.5f).SetEase(Ease.OutCubic);

                StartCoroutine(Wait());
                break;
            case 2:
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            default:
                break;
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (transform == null || _canvasGroup == null) return;
        switch (ButtonMode)
        {
            case 0:
                transform.DOScale(1.1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1.05f, 0.24f).SetEase(Ease.OutCubic);
                break;
            case 1: 
                transform.DOScale(1.2f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1.1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            case 2: 
                transform.DOScale(1.3f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1.15f, 0.24f).SetEase(Ease.OutCubic);
                break;
            default:
                break;
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (transform == null || _canvasGroup == null) return;
        switch (ButtonMode)
        {
            case 0:
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            case 1: 
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            case 2: 
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            default:
                break;
        }
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.0f);
        this.gameObject.SetActive(false);
        yield break;
    }
}