using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundLight : MonoBehaviour
{
    public bool night = false;
    private void Update()
    {
        // Y���ɑ΂��āA1�b�Ԃ�-6�x��]������
        if (transform.eulerAngles.x >= 200)
        {
            transform.Rotate(new Vector3(0, -18, 0) * Time.deltaTime);
        }
        else
        {
            transform.Rotate(new Vector3(0, -6, 0) * Time.deltaTime);
        }

        //�����邩�̔���i�Q�[�W�e�L�X�g�̐F��ς���j
        if (transform.eulerAngles.x >= 180)
        {
            night = true;
        }
        else
        {
            night = false;
        }
        
    }
}