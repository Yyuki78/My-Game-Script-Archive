using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetect : MonoBehaviour
{
    public bool ObstaclePass = false;
    void OnCollisionEnter(Collision other)
    {
        //キツネが農具とぶつかった場合にTrueを返す
        if (other.gameObject.tag == "Enemy")
        {
            ObstaclePass = true;
            Debug.Log("農具と接触しました");
        }
    }
}
