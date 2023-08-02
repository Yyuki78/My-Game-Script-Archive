using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLimitDetection : MonoBehaviour
{
    private MoveLimitController _controller;

    private void Start()
    {
        _controller = GetComponentInParent<MoveLimitController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            _controller.ChangeMoveLimitCube(0);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            _controller.ChangeMoveLimitCube(0);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            _controller.ChangeMoveLimitCube(1);
    }
}
