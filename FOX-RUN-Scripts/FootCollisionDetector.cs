using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootCollisionDetector : MonoBehaviour
{
    public bool FootGaugeCol = false;
    void OnTriggerStay(Collider other)
    {
        //�{�[�������Ղ̏��ʂ������̔�����s���A����Ă���ꍇ�ɂ̓Q�[�W�̏㏸���~�߂�
        if (other.gameObject.tag == "Foot")
        {
            Debug.Log("���Ղ̏�ł�");
            FootGaugeCol = true;
        }
        else
        {
            FootGaugeCol = false;
        }
    }
}
