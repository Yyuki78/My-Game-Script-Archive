using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxGauge : MonoBehaviour
{
    //�͈͊O�ɏo���Ƃ��ɖ����Q�[�W�����炷
    private EnemyMove _move;
    public bool reduse = false;

    public void OnTriggerEnter(Collider other)
    {
        reduse = false;
    }

    public void OnTriggerExit(Collider other)
    {
        reduse = true;
    }
}
