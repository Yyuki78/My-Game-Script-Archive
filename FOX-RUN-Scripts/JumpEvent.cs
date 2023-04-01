using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class JumpEvent : MonoBehaviour
{
    public bool JumpEve = false;//�|�b�v�A�b�v�e�L�X�g�p��bool
    private bool EnemyJumpEvent = false;
    private bool once = false;//1�񂾂�true�ɂ���悤��bool
    //�J�����؂�ւ��ɂ��Ball�̈ړ�������ς��Ȃ��ׂ̕ϐ�
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
            Debug.Log("Jump�C�x���g");
            //_JumpCamera.Priority = 30;
            //CameraSwitch = true;
            passTime += Time.deltaTime;
            if (passTime > span && once == false)
            {
                JumpEve = true;
                once = true;
                Debug.Log("Jump�C�x���g���������܂����B");
                _move.fill += 50;
                passTime = 0f;
                //_JumpCamera.Priority = 10;
                //CameraSwitch = false;

                //Jump�C�x���g�֘A�����~
                EnemyJumpEvent = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        //�L�c�l���ʂ������̔�����s���ATrue�̏ꍇ�ɃQ�[�W�㏸������B
        if (other.gameObject.tag == "Enemy")
        {
            EnemyJumpEvent = true;
            Debug.Log("�L�c�l��Jump!?");
        }
    }
}
