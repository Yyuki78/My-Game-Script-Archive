using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject NPC;
    //NPC頭上に表示するObjの色　役割で変わる
    [SerializeField] Material[] colorMat;

    [SerializeField] Transform[] generatePoints;

    [SerializeField] GameObject[] Buttons;

    private AudioManager _audio;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 30; i++)
        {
            PopNPC();
        }
        _audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        _audio.BGM1();
    }

    private void PopNPC()
    {
        int ran = Random.Range(0, generatePoints.Length);
        var ins = Instantiate(NPC, generatePoints[ran].position, Quaternion.identity, transform);
        NPCInformation info = ins.GetComponent<NPCInformation>();
        ins.GetComponent<NPCController>().isInfinite = true;
        int parcent = Random.Range(0, 100);
        if (parcent < 60)
        {
            info.role = 0;
        }
        else if (parcent < 65)
        {
            info.role = 1;
        }
        else if (parcent < 85)
        {
            info.role = 2;
        }
        else if (parcent < 95)
        {
            info.role = 3;
        }
        else
        {
            info.role = 4;
        }

        var rotateObj = ins.GetComponentInChildren<RotateObj>();
        rotateObj.gameObject.GetComponent<MeshRenderer>().material = colorMat[info.role];
    }
}
