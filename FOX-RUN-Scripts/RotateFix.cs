using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFix : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
