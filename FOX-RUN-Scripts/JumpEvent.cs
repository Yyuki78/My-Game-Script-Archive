using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class JumpEvent : MonoBehaviour
{
    public bool JumpEve = false;//ポップアップテキスト用のbool
    private bool EnemyJumpEvent = false;
    private bool once = false;//1回だけtrueにするようのbool
    //カメラ切り替えによるBallの移動方向を変えない為の変数
    //public bool CameraSwitch = false;
    //[SerializeField] private CinemachineVirtualCamera _JumpCamera;

    [SerializeField] GameObject Fox;
    private EnemyMove _move;

    private float span = 0.5f;
    private float passTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        _move = Fox.GetComponent<EnemyMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyJumpEvent == true)
        {
            Debug.Log("Jumpイベント");
            //_JumpCamera.Priority = 30;
            //CameraSwitch = true;
            passTime += Time.deltaTime;
            if (passTime > span && once == false)
            {
                JumpEve = true;
                once = true;
                Debug.Log("Jumpイベントが成功しました。");
                _move.fill += 50;
                passTime = 0f;
                //_JumpCamera.Priority = 10;
                //CameraSwitch = false;

                //Jumpイベント関連物を停止
                EnemyJumpEvent = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        //キツネが通ったかの判定を行い、Trueの場合にゲージ上昇をする。
        if (other.gameObject.tag == "Enemy")
        {
            EnemyJumpEvent = true;
            Debug.Log("キツネのJump!?");
        }
    }
}
