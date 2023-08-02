using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] int ButtonMode = 0;

    [SerializeField] Transform objectTransform;
    [SerializeField] Transform maskTransform;
    private Transform _transform;
    private Image _image;

    private Sequence sequence;
    private Vector3 initialPos;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        if (GetComponent<Image>() != null)
            _image = GetComponent<Image>();
        initialPos = transform.position;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        switch (ButtonMode)
        {
            case 0:
                objectTransform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic);
                _transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic);
                break;
            case 1:
                break;
            case 2:
                _transform.DORotate(new Vector3(90, 0, 240), 0.24f).SetEase(Ease.OutCubic);
                break;
            case 3:
                _transform.DORotate(new Vector3(0, -90, 240), 0.24f).SetEase(Ease.OutCubic);
                break;
            case 4:
                _transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic);
                break;
            default:
                break;
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        switch (ButtonMode)
        {
            case 0:
                objectTransform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            case 1:
                break;
            case 2:
                _transform.DORotate(new Vector3(90, 0, 360), 0.24f).SetEase(Ease.OutCubic);
                break;
            case 3:
                _transform.DORotate(new Vector3(0, -90, 360), 0.24f).SetEase(Ease.OutCubic);
                break;
            case 4:
                _transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            default:
                break;
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        switch (ButtonMode)
        {
            case 0:
                objectTransform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            case 1:
                if (maskTransform == null) return;
                maskTransform.DORestart();
                maskTransform.DOKill();
                objectTransform.DOScaleY(0.01f, 0.48f).SetEase(Ease.OutCubic);
                _transform.DOMoveY(-0.001f, 0.48f).SetEase(Ease.OutCubic).SetRelative(true);
                break;
            case 2:
                _transform.DORotate(new Vector3(90, 0, 240), 0.24f).SetEase(Ease.OutCubic);
                break;
            case 3:
                _transform.DORotate(new Vector3(0, -90, 240), 0.24f).SetEase(Ease.OutCubic);
                break;
            case 4:
                _transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            default:
                break;
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        switch (ButtonMode)
        {
            case 0:
                objectTransform.DOScale(1.1f, 0.24f).SetEase(Ease.OutCubic);
                //_transform.DOScale(1.1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            case 1:
                if (maskTransform == null) return;
                maskTransform.DOMoveZ(0.5f, 1f).SetDelay(1f).SetRelative(true)
                    .SetLoops(-1, LoopType.Restart);
                break;
            case 2:
                _transform.DORotate(new Vector3(90, 0, 120), 0.24f).SetEase(Ease.OutCubic);
                break;
            case 3:
                _transform.DORotate(new Vector3(0, -90, 120), 0.24f).SetEase(Ease.OutCubic);
                break;
            case 4:
                _transform.DOScale(1.1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            default:
                break;
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        switch (ButtonMode)
        {
            case 0:
                objectTransform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            case 1:
                if (maskTransform == null) return;
                maskTransform.DORestart();
                maskTransform.DOKill();
                break;
            case 2:
                _transform.DORotate(new Vector3(90, 0, 0), 0.24f).SetEase(Ease.OutCubic);
                break;
            case 3:
                _transform.DORotate(new Vector3(0, -90, 0), 0.24f).SetEase(Ease.OutCubic);
                break;
            case 4:
                _transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        if (ButtonMode == 1)
        {
            objectTransform.DOScaleY(1f, 0f);
            _transform.position = initialPos;
        }
    }
}