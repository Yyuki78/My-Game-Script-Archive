using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundLight : MonoBehaviour
{
    public bool night = false;
    private void Update()
    {
        // Y軸に対して、1秒間に-6度回転させる
        if (transform.eulerAngles.x >= 200)
        {
            transform.Rotate(new Vector3(0, -18, 0) * Time.deltaTime);
        }
        else
        {
            transform.Rotate(new Vector3(0, -6, 0) * Time.deltaTime);
        }

        //昼か夜かの判定（ゲージテキストの色を変える）
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