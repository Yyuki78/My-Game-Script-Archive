using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static float time;

    [SerializeField] GameObject gameClear;//ÉQÅ[ÉÄÉNÉäÉAâÊñ 
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameClear.activeSelf == false)
        {
            time += Time.deltaTime;
        }
    }
}
