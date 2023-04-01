using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BroomEffect : MonoBehaviour
{
    private bool canSwipe = true;

    private float wait = 0.1f;
    private Vector3 rotate = new Vector3(0, 0, 30);
    private Vector3 active = new Vector3(1, 1, 1);
    private Vector3 reset = new Vector3(0, 0, 0);

    private Transform _parentTransform;
    private Transform _transform;
    [SerializeField] Transform[] smogTransform;
    private Stage10Controller _controller;

    void Awake()
    {
        _parentTransform = transform.parent.gameObject.GetComponent<Transform>();
        _transform = GetComponent<Transform>();
        _transform.localScale = reset;
        for (int i = 0; i < smogTransform.Length; i++)
            smogTransform[i].localScale = reset;
        _controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Stage10Controller>();
    }

    public void effect(GameObject target,int mode)
    {
        if (!canSwipe) return;
        canSwipe = false;

        AudioManager.instance.SE(11);

        switch (mode)
        {
            case 0:
                _parentTransform.localPosition = target.transform.localPosition;
                break;
            case 1:
                _parentTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);
                break;
        }

        _transform.DOScale(active, 0.1f);
        for (int i = 0; i < 2; i++)
        {
            _transform.DOLocalRotate(rotate, wait).SetDelay(wait * 2 *i);
            _transform.DOLocalRotate(-rotate, wait).SetDelay(wait + wait * 2 * i);
        }
        _transform.DOLocalRotate(reset, 0.05f).SetDelay(1.7f/4);
        _transform.DOScale(reset, 0.1f).SetDelay(1.8f/4);

        for (int i = 0; i < smogTransform.Length; i++)
        {
            smogTransform[i].localScale = new Vector3(0, 0, 0);
            smogTransform[i].DOScale(active, 0.2f).SetEase(Ease.OutBack).SetEase(Ease.OutQuint).SetDelay(wait * i);
            smogTransform[i].DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.1f).SetDelay(wait * i + 0.2f);
            smogTransform[i].DOScale(reset, 0.01f).SetDelay(wait * i + 0.3f);
        }

        StartCoroutine(invisible(target));
    }

    private IEnumerator invisible(GameObject target)
    {
        yield return new WaitForSeconds(0.5f);
        _controller.SwipeObj();
        target.SetActive(false);
        canSwipe = true;
        yield break;
    }
}
