using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerDetection : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Destroy(other.gameObject);
        }
    }
}
