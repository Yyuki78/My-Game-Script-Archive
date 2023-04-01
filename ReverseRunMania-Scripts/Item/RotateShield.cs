using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateShield : MonoBehaviour
{
    [SerializeField] float rotateValue = 2f;
    void Update()
    {
        transform.Rotate(0, rotateValue, 0);
    }
}
