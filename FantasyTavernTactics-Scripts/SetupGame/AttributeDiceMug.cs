using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeDiceMug : MonoBehaviour
{
    //マグの角度で上部分のコライダーを消す(ダイスが外に出るようになる)
    //ダイスがマグの中から無くなったら全てのダイスが止まるまで待つ＋マグとダイスの判定を消す(しなくてもいい)
    //全てのダイスが止まったら机上に集める
    //その後ダイスを属性順に並べ替える

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
            //全てのダイスの属性が決まったか
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
            //結果の表示
            if (!showResult)
            {
                showResult = true;
                SendResult();
            }
            return;
        }

        //最初の掴み時にダイスが動くようにする+矢印を消す
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

        //マグの上蓋の仕様
        if (mugLid.activeSelf)
        {
            mugAngle = transform.eulerAngles;
            if (mugAngle.x < -30f || mugAngle.x > 30 || mugAngle.z < -30f || mugAngle.z > 30)
            {
                //マグが傾けられたので蓋を消す
                if (mugLid.activeSelf)
                    mugLid.SetActive(false);
            }
        }

        //全てのダイスが出ていないならreturn
        if (!isAllDiceOut) return;
        _determination.HideExplanationText();

        //全てのダイスが止まったかのboolを格納
        for (int i = 0; i < isDiceStop.Length; i++)
        {
            isDiceStop[i] = diceMove[i].isDiceStopping;
        }

        //全てのダイスが止まったかの判定
        num = 0;
        while (num < isDiceStop.Length)
        {
            if (!isDiceStop[num]) return;
            num++;
        }

        if (once)
        {
            once = false;
            // 全ての子オブジェクトのレイヤーを変更する
            Transform[] children = GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                child.gameObject.layer = 10;//ダイスと当たらないようにする(不正防止)
            }

            //全てのダイスを机上に集める
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
        //それぞれの属性の個数を格納
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
