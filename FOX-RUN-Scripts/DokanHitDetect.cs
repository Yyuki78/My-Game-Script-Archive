using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DokanHitDetect : MonoBehaviour
{
    public bool DokanEnemyPass = false;
    void OnTriggerStay(Collider other)
    {
        //�L�c�l���ʂ������̔�����s���A�ǂ����True�̏ꍇ�ɃQ�[�W�㏸������B
        if (other.gameObject.tag == "Enemy")
        {
            DokanEnemyPass = true;
            Debug.Log("�y�ǂɃL�c�l������܂����B");
        }
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("�y�ǂ�Player!?");
        }
    }
}
