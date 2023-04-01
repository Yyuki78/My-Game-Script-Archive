using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{

    public GameObject mainCamera;      //���C���J�����i�[�p
    public GameObject subCamera1;       //�T�u�J����1�i�[�p 
    public GameObject subCamera2;       //�T�u�J����2�i�[�p 

    //�J�����؂�ւ�����x�����s��Ȃ����߂�bool
    private bool once = false; //���Վ��_���ǂ���
    private bool once2 = false; //���Վ��_���ǂ���

    GameObject Fox; //Fox���̂��̂�����ϐ�
    EnemyMove _move; //EnemyMove������ϐ�

    //�Ăяo�����Ɏ��s�����֐�
    void Start()
    {
        //���C���J�����ƃT�u�J���������ꂼ��擾
        mainCamera = GameObject.Find("CM vcam1");
        subCamera1 = GameObject.Find("CM vcam2 back");
        subCamera2 = GameObject.Find("CM vcam3 fukan");

        //�T�u�J�������A�N�e�B�u�ɂ���
        subCamera1.SetActive(false);
        subCamera2.SetActive(false);

        Fox = GameObject.Find("Fox"); //Fox���I�u�W�F�N�g�̖��O����擾���ĕϐ��Ɋi�[����
        _move = Fox.GetComponent<EnemyMove>(); //Fox�̒��ɂ���EnemyMove���擾���ĕϐ��Ɋi�[����
    }


    //�P�ʎ��Ԃ��ƂɎ��s�����֐�
    void Update()
    {
        if (_move.hukan == true)
        {
            StartCoroutine("ChangeHukan");
            Debug.Log("���Վ��_");
        }

        if (_move.hukan == false && once2 == false)
        {
            StopCoroutine("ChangeHukan");
            //���C���J�������A�N�e�B�u�ɐݒ�
            subCamera2.SetActive(false);
            subCamera1.SetActive(false);
            mainCamera.SetActive(true);
            once = true;
            once2 = true;
        }

        //N�L�[��������Ă���ԁA�T�u�J�������A�N�e�B�u�ɂ���
        if (Input.GetKeyDown(KeyCode.N))
        {
            //�T�u�J�������A�N�e�B�u�ɐݒ�
            mainCamera.SetActive(false);
            subCamera1.SetActive(true);
            subCamera2.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.N))
        {
            //���C���J�������A�N�e�B�u�ɐݒ�
            subCamera2.SetActive(false);
            subCamera1.SetActive(false);
            mainCamera.SetActive(true);
        }

        /*
        if (Input.GetKeyDown(KeyCode.M))
        {
            mainCamera.SetActive(false);
            subCamera1.SetActive(false);
            subCamera2.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            //���C���J�������A�N�e�B�u�ɐݒ�
            subCamera2.SetActive(false);
            subCamera1.SetActive(false);
            mainCamera.SetActive(true);
        }
        */
    }

    IEnumerator ChangeHukan()
    {
        once2 = false;
        if (_move.hukan == false) yield break;
        if (once == false) yield break;
        yield return new WaitForSeconds(1);
        if (_move.hukan == false) yield break;
        //���Վ��_�Ɉڍs���܂�
        mainCamera.SetActive(false);
        subCamera1.SetActive(false);
        subCamera2.SetActive(true);
        once = false;
    }
}