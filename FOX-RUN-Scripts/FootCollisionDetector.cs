using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootCollisionDetector : MonoBehaviour
{
    public bool FootGaugeCol = false;
    void OnTriggerStay(Collider other)
    {
        //ボールが足跡の上を通ったかの判定を行い、取っている場合にはゲージの上昇を止める
        if (other.gameObject.tag == "Foot")
        {
            Debug.Log("足跡の上です");
            FootGaugeCol = true;
        }
        else
        {
            FootGaugeCol = false;
        }
    }
}
