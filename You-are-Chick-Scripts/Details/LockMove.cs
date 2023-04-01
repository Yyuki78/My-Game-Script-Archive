using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LockMove : MonoBehaviour
{
    [SerializeField] GameObject Lock;
    private Transform _transform;
    [SerializeField] GameObject ReleaseEffectObj;
    private Coroutine coroutine;
    private Stage7Controller _controller;

    void Start()
    {
        _transform = Lock.GetComponent<Transform>();
        _controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Stage7Controller>();
    }

    void OnMouseEnter()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutine = StartCoroutine(lockEffect());
    }

    void OnMouseExit()
    {
        StopCoroutine(coroutine);
        _transform.DOKill();
        AudioManager.instance.StopSE(7);
    }

    private IEnumerator lockEffect()
    {
        float wait = 0.1f;
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.SE(7);
        wait = 1f;
        _transform.DOShakeRotation(wait, 30f, 10, 0, false).SetDelay(wait);
        _transform.DOShakeRotation(wait, 60f, 20, 0, false).SetDelay(wait * 2);
        _transform.DOShakeRotation(wait, 90f, 30, 0, false).SetDelay(wait * 3);
        _transform.DOShakeRotation(wait, 120f, 60, 10, false).SetDelay(wait * 4);

        yield return new WaitForSeconds(4f);

        Lock.SetActive(false);
        ReleaseEffectObj.SetActive(true);
        _controller.GateOpen();
        yield break;
    }
}
