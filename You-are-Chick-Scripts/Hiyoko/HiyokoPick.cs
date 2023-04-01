using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HiyokoPick : MonoBehaviour
{
    [SerializeField] float[] LimitPos = new float[4];
    private HiyokoInfomation _info;
    private Transform _transform;

    void Start()
    {
        _info = GetComponent<HiyokoInfomation>();
        _transform = GetComponent<Transform>();
    }
    /*
    //�͂񂾎��̃A�j���[�V����
    private IEnumerator PickAnimtion()
    {
        transform.localScale = new Vector3(1.3f, 1.3f, 1f);
        if (Sound)
        {
            _audio.Play();
        }
        while (true)
        {
            if (state != State.Pick)
            {
                this.transform.localRotation = new Quaternion(0, 0, 0, 0);
                transform.localScale = new Vector3(1f, 1f, 1f);
                _audio.Stop();
                yield break;
            }
            for (int i = 0; i < 12; i++)
            {
                if (i < 3)
                {
                    this.transform.Rotate(0, 0, 5);
                    yield return new WaitForSeconds(0.015f);
                }
                else if (3 <= i && i < 9)
                {
                    this.transform.Rotate(0, 0, -5);
                    yield return new WaitForSeconds(0.015f);
                }
                else
                {
                    this.transform.Rotate(0, 0, 5);
                    yield return new WaitForSeconds(0.015f);
                }
            }
        }
    }

    /*
    // �N���b�N���ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    public void OnPointClick()
    {
        if (!_info.IsMove) return;
        print($"�I�u�W�F�N�g {name} ���N���b�N���ꂽ��I");
        _info.SetState(HiyokoInfomation.State.Pick);
        Hat.gameObject.layer = 9;
        Number.gameObject.layer = 9;
        this.gameObject.layer = 9;
        StartCoroutine(PickAnimtion());
    }

    // �N���b�N�݂̂ŏI������Ƃ��ɌĂяo����郁�\�b�h
    public void OnPointClickEnd()
    {
        if (state == State.Finish || state == State.Die) return;
        state = State.Normal;
        Hat.gameObject.layer = 0;
        Number.gameObject.layer = 0;
        this.gameObject.layer = 0;
        StopCoroutine(PickAnimtion());
    }*/

    // �h���b�O���ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    public void OnMouseDrag()
    {
        if (!_info.IsPick) return;
        _info.SetState(HiyokoInfomation.State.Pick);
        //Hat.gameObject.layer = 9;
        //this.gameObject.layer = 9;
        Vector3 position = Input.mousePosition;
        position.z = 1f;
        Vector3 screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
        if (screenToWorldPointPosition.x < LimitPos[0] || screenToWorldPointPosition.x > LimitPos[1]) return;
        if (screenToWorldPointPosition.y < LimitPos[2] || screenToWorldPointPosition.y > LimitPos[3]) return;
        _transform.position = screenToWorldPointPosition;
    }

    // �h���b�v���ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    public void OnMouseUp()
    {
        _info.SetState(HiyokoInfomation.State.Put);
        //Hat.gameObject.layer = 0;
        //this.gameObject.layer = 8;//CheckLayer�ɕύX
        //_rigidbody.velocity = rememberVelocity;
        //StartCoroutine(checkTime());
    }
}
