using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetect : MonoBehaviour
{
    public bool ObstaclePass = false;
    void OnCollisionEnter(Collision other)
    {
        //�L�c�l���_��ƂԂ������ꍇ��True��Ԃ�
        if (other.gameObject.tag == "Enemy")
        {
            ObstaclePass = true;
            Debug.Log("�_��ƐڐG���܂���");
        }
    }
}
