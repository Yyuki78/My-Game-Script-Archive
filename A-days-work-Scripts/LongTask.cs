using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongTask : MonoBehaviour
{
    [SerializeField] int taskType = 0;//0はそれ以外,1は地球儀,2は配達受け取り,3は円柱運び

    [SerializeField] GameObject[] TransparentObj;//アクティブ化するもの
    [SerializeField] GameObject[] SetOutlineObj;//アウトラインを付けるもの

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
                Debug.Log("タスクセットミス！");
                break;
        }
    }
}
