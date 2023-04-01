using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            other.gameObject.GetComponent<CoinMove>().isMagnet = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            other.gameObject.GetComponent<CoinMove>().isMagnet = true;
        }
    }
}
