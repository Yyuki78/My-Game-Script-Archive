using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatDetect : MonoBehaviour
{
    public bool RatPass = false;
    void OnTriggerStay(Collider other)
    {
        //�L�c�l���l�Y�~�̏��ʂ����ꍇ��True��Ԃ�
        if (other.gameObject.tag == "Enemy")
        {
            RatPass = true;
            Debug.Log("�l�Y�~���擾���܂���");
        }
    }
}
