using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DokanEvent : MonoBehaviour
{
    //����Script�p�̃C�x���g�Ǘ��ϐ�
    public bool DokanEventBool = false;
    private bool once = false;//1�񂾂�true�ɂ���悤��bool
    //�J�����؂�ւ��ɂ��Ball�̈ړ�������ς��Ȃ��ׂ̕ϐ�
    public bool CameraSwitch = false;

    [SerializeField] private GameObject _EnterGO;
    [SerializeField] private GameObject _ExitGO;
    [SerializeField] private CinemachineVirtualCamera _DokanCamera;

    private DokanHitDetect _DHD1;
    private DokanHitDetect _DHD2;

    private float span = 10f;
    private float passTime1 = 0f;
    private float passTime2 = 0f;
    // Start is called before the first frame update
    void Start()
    {
        _DHD1 = _EnterGO.GetComponent<DokanHitDetect>();
        _DHD2 = _ExitGO.GetComponent<DokanHitDetect>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_DHD1.DokanEnemyPass == true)
        {
            _DokanCamera.Priority = 30;
            CameraSwitch = true;
            passTime1 += Time.deltaTime;
            if (passTime1 > span)
            {
                Debug.LogFormat("�y�ǂɑO��������Ă���{0}�b�o��", span);
                passTime1 = 0f;
                _DHD1.DokanEnemyPass = false;
                _DokanCamera.Priority = 10;
                CameraSwitch = false;
            }
        }
        if (_DHD2.DokanEnemyPass == true)
        {
            _DokanCamera.Priority = 30;
            CameraSwitch = true;
            passTime2 += Time.deltaTime;
            if (passTime2 > span)
            {
                Debug.LogFormat("�y�ǂɌ�납������Ă���{0}�b�o��", span);
                passTime2 = 0f;
                _DHD2.DokanEnemyPass = false;
                _DokanCamera.Priority = 10;
                CameraSwitch = false;
            }
        }
        if (_DHD1.DokanEnemyPass == true && _DHD2.DokanEnemyPass == true)
        {
            Debug.Log("�y�ǃC�x���g���������܂����B");
            if (once == false)
            {
                DokanEventBool = true;
                once = true;
            }
            else
            {
                DokanEventBool = false;
            }
            //�S�Ă̓y�ǃC�x���g�֘A�����~
            _DokanCamera.Priority = 10;
            CameraSwitch = false;
            _EnterGO.SetActive(false);
            _ExitGO.SetActive(false);
        }
    }
}
