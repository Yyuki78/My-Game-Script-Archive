using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleTask : MonoBehaviour
{
    [SerializeField] int taskType = 0;//0は水やり・ゴミ捨て,1は箱開け系,2はドラム缶,3は球運び

    [SerializeField] GameObject[] ChangeObjs;//アクティブ化orアウトラインを付けるもの

    [SerializeField] int LoopNum = 0;//水やりは2,ゴミ捨ては3

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
                int ran;
                for(int i = 0; i < LoopNum; i++)
                {
                    ran = Random.Range(0, 2);
                    if (LoopNum == 2)
                        ChangeObjs[i * 2 + ran + 4].SetActive(true);
                    ChangeObjs[i * 2 + ran].SetActive(true);
                    ChangeObjs[i * 2 + ran].AddComponent<Outline>();
                }
                if (LoopNum == 3)
                    ChangeObjs[6].SetActive(true);
                else if (LoopNum == 2)
                    ChangeObjs[8].AddComponent<Outline>();
                break;
            case 1:
                ChangeObjs[0].AddComponent<Outline>();
                break;
            case 2:
                ChangeObjs[0].SetActive(true);
                ChangeObjs[1].SetActive(true);
                ChangeObjs[2].AddComponent<Outline>();
                ChangeObjs[3].AddComponent<Outline>();
                break;
            case 3:
                var outline = ChangeObjs[0].AddComponent<Outline>();
                outline.OutlineColor = new Color(1, 130f / 255f, 0, 123f / 255f);

                var renderer = ChangeObjs[0].GetComponent<MeshRenderer>();
                Material[] materials = renderer.sharedMaterials;
                materials[1] = ChangeObjs[1].GetComponent<MeshRenderer>().material;
                renderer.sharedMaterials = materials;

                int ran2 = Random.Range(0, 3);
                ChangeObjs[ran2 + 2].AddComponent<Outline>();
                break;
            default:
                Debug.Log("タスクセットミス！");
                break;
        }
    }
}
