using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeDiceMug : MonoBehaviour
{
    //�}�O�̊p�x�ŏ㕔���̃R���C�_�[������(�_�C�X���O�ɏo��悤�ɂȂ�)
    //�_�C�X���}�O�̒����疳���Ȃ�����S�Ẵ_�C�X���~�܂�܂ő҂{�}�O�ƃ_�C�X�̔��������(���Ȃ��Ă�����)
    //�S�Ẵ_�C�X���~�܂��������ɏW�߂�
    //���̌�_�C�X�𑮐����ɕ��בւ���

    public bool isAllDiceOut = false;
    public int[] AttributeNum;
    public int greenNum { private set; get; }
    public int redNum { private set; get; }
    public int blueNum { private set; get; }

    [SerializeField] GameObject mugLid;
    [SerializeField] AttributeDiceMove[] diceMove = new AttributeDiceMove[10];
    [SerializeField] Vector3[] GatherPos = new Vector3[10];
    [SerializeField] GameObject DirectionalArrow;
    [SerializeField] MeshRenderer[] _mugRenderer;

    private Vector3 mugAngle;
    private bool[] isDiceStop;
    private bool once = true;
    private bool showResult = false;

    private OVRGrabbable _grab;
    private bool firstGrab = true;

    private AttributeDetermination _determination;
    private Rigidbody _rigid;
    private AudioSource _audio;

    void Start()
    {
        _grab = GetComponent<OVRGrabbable>();
        _determination = GetComponentInParent<AttributeDetermination>();
        _rigid = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
        GameInfomation info = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>();
        switch (info.BoardSize)
        {
            case 5:
                AttributeNum = new int[6];
                isDiceStop = new bool[6];
                for(int i = 0; i < 4; i++)
                    diceMove[9 - i].gameObject.SetActive(false);
                break;
            case 6:
                AttributeNum = new int[8];
                isDiceStop = new bool[8];
                for (int i = 0; i < 2; i++)
                    diceMove[9 - i].gameObject.SetActive(false);
                break;
            case 7:
                AttributeNum = new int[10];
                isDiceStop = new bool[10];
                break;
        }
        for (int i = 0; i < isDiceStop.Length; i++)
        {
            AttributeNum[i] = -1;
        }
    }

    int num = 0;
    void Update()
    {
        if (showResult) return;
        if (!once)
        {
            //�S�Ẵ_�C�X�̑��������܂�����
            num = 0;
            for (int i = 0; i < isDiceStop.Length; i++)
            {
                AttributeNum[i] = diceMove[i].AttributeResult;
            }
            while (num < isDiceStop.Length)
            {
                if (!diceMove[num].isFinish) return;
                num++;
            }
            //���ʂ̕\��
            if (!showResult)
            {
                showResult = true;
                SendResult();
            }
            return;
        }

        //�ŏ��̒͂ݎ��Ƀ_�C�X�������悤�ɂ���+��������
        if (_grab.isGrabbed && firstGrab)
        {
            firstGrab = false;
            DirectionalArrow.SetActive(false);
            for (int i = 0; i < isDiceStop.Length; i++)
            {
                diceMove[i].StartMove();
            }
            _audio.PlayOneShot(_audio.clip);
        }

        //�}�O�̏�W�̎d�l
        if (mugLid.activeSelf)
        {
            mugAngle = transform.eulerAngles;
            if (mugAngle.x < -30f || mugAngle.x > 30 || mugAngle.z < -30f || mugAngle.z > 30)
            {
                //�}�O���X����ꂽ�̂ŊW������
                if (mugLid.activeSelf)
                    mugLid.SetActive(false);
            }
        }

        //�S�Ẵ_�C�X���o�Ă��Ȃ��Ȃ�return
        if (!isAllDiceOut) return;
        _determination.HideExplanationText();

        //�S�Ẵ_�C�X���~�܂�������bool���i�[
        for (int i = 0; i < isDiceStop.Length; i++)
        {
            isDiceStop[i] = diceMove[i].isDiceStopping;
        }

        //�S�Ẵ_�C�X���~�܂������̔���
        num = 0;
        while (num < isDiceStop.Length)
        {
            if (!isDiceStop[num]) return;
            num++;
        }

        if (once)
        {
            once = false;
            // �S�Ă̎q�I�u�W�F�N�g�̃��C���[��ύX����
            Transform[] children = GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                child.gameObject.layer = 10;//�_�C�X�Ɠ�����Ȃ��悤�ɂ���(�s���h�~)
            }

            //�S�Ẵ_�C�X������ɏW�߂�
            for (int i = 0; i < isDiceStop.Length; i++)
            {
                diceMove[i].goTable(GatherPos[i]);
            }

            if (_grab.isGrabbed)
                _grab.grabbedBy.ForceRelease(_grab);
            _rigid.isKinematic = true;
            _mugRenderer[0].enabled = false;
            _mugRenderer[1].enabled = false;
        }
    }

    private void SendResult()
    {
        Array.Sort(AttributeNum);
        //���ꂼ��̑����̌����i�[
        int n = 0;
        while (AttributeNum[n] == 0)
        {
            n++;
            if (n >= isDiceStop.Length - 1)
                break;
        }
        redNum = n;

        while (AttributeNum[n] == 1)
        {
            n++;
            if (n >= isDiceStop.Length - 1)
                break;
        }
        blueNum = n - redNum;
        greenNum = isDiceStop.Length - n;
        _determination.ShowDiceResult(redNum, blueNum, greenNum);

        if (_grab.isGrabbed)
            _grab.grabbedBy.ForceRelease(_grab);
        gameObject.SetActive(false);
    }
}
