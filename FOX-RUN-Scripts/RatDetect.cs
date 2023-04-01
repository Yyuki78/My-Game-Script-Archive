using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatDetect : MonoBehaviour
{
    public bool RatPass = false;
    void OnTriggerStay(Collider other)
    {
        //キツネがネズミの上を通った場合にTrueを返す
        if (other.gameObject.tag == "Enemy")
        {
            RatPass = true;
            Debug.Log("ネズミを取得しました");
        }
    }
}
