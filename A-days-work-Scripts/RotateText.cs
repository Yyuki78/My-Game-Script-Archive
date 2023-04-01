using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateText : MonoBehaviour
{
    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        _transform.Rotate(0, 0.2f, 0);
    }
}
