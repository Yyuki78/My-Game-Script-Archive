using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HiyokoJump : MonoBehaviour
{
    [SerializeField] GameObject[] GateCollider;
    private HiyokoInfomation _info;
    private Transform _transform;

    void Start()
    {
        _info = GetComponent<HiyokoInfomation>();
        _transform = GetComponent<Transform>();
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (_info._currentState != HiyokoInfomation.State.Normal) return;
            _info.SetState(HiyokoInfomation.State.Jump);
            StartCoroutine(jumpMove());
        }
    }

    private IEnumerator jumpMove()
    {
        var wait = new WaitForSeconds(0.05f);
        yield return wait;
        GateCollider[0].SetActive(false);
        GateCollider[1].SetActive(false);

        AudioManager.instance.SE(12);

        _transform.DOMoveY(1f, 0.5f).SetRelative(true).SetEase(Ease.OutQuart);
        int ran = Random.Range(0, 10);
        if (ran < 2)
            _transform.DORotate(new Vector3(0, 0, 0), 0.25f, RotateMode.FastBeyond360).SetDelay(0.5f);
        else if (ran < 5)
            _transform.DORotate(new Vector3(0, 0, 360), 0.25f, RotateMode.FastBeyond360).SetDelay(0.5f);
        else if (ran < 8)
            _transform.DORotate(new Vector3(0, 0, -360), 0.25f, RotateMode.FastBeyond360).SetDelay(0.5f);
        else
            _transform.DORotate(new Vector3(0, 360, 0), 0.25f, RotateMode.FastBeyond360).SetDelay(0.5f);

        _transform.DOMoveY(-1f, 0.5f).SetDelay(0.75f).SetRelative(true).SetEase(Ease.InQuart);
        yield return new WaitForSeconds(1.25f);
        _info.SetState(HiyokoInfomation.State.Normal);
        GateCollider[0].SetActive(true);
        GateCollider[1].SetActive(true);

        yield break;
    }
}
