using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongTask : MonoBehaviour
{
    [SerializeField] int taskType = 0;//0�͂���ȊO,1�͒n���V,2�͔z�B�󂯎��,3�͉~���^��

    [SerializeField] GameObject[] TransparentObj;//�A�N�e�B�u���������
    [SerializeField] GameObject[] SetOutlineObj;//�A�E�g���C����t�������

    [SerializeField] int LoopNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Prepare();
    }

    public void Prepare()
    {
        switch (taskType)
        {
            case 0:
                for (int i = 0; i < LoopNum; i++)
                {
                    TransparentObj[i].SetActive(true);
                    SetOutlineObj[i].AddComponent<Outline>();
                }
                break;
            case 1:
                SetOutlineObj[0].AddComponent<Outline>();
                break;
            case 2:
                int ran = Random.Range(0, 3);
                TransparentObj[0].SetActive(true);
                SetOutlineObj[ran].SetActive(true);
                SetOutlineObj[ran].AddComponent<Outline>();
                break;
            case 3:
                int ran2;
                for (int i = 0; i < LoopNum; i++)
                {
                    ran2 = Random.Range(0, 2);
                    TransparentObj[i * 2 + ran2].SetActive(true);
                    SetOutlineObj[i].AddComponent<Outline>();
                }
                break;
            default:
                Debug.Log("�^�X�N�Z�b�g�~�X�I");
                break;
        }
    }
}
