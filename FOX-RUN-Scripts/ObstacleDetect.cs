using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetect : MonoBehaviour
{
    public bool ObstaclePass = false;
    void OnCollisionEnter(Collision other)
    {
        //ƒLƒcƒl‚ª”_‹ï‚Æ‚Ô‚Â‚©‚Á‚½ê‡‚ÉTrue‚ğ•Ô‚·
        if (other.gameObject.tag == "Enemy")
        {
            ObstaclePass = true;
            Debug.Log("”_‹ï‚ÆÚG‚µ‚Ü‚µ‚½");
        }
    }
}
