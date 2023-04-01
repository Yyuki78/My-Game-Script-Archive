using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFix2 : MonoBehaviour
{
    GameObject Cam;
    Transform _transform;

    private void Start()
    {
        Cam = GameObject.Find("CM vcam1");
        _transform = Cam.GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        Debug.Log(_transform.localEulerAngles);
        gameObject.transform.rotation = Quaternion.Euler(-_transform.localEulerAngles.x, -_transform.localEulerAngles.y * 180, 0);
        gameObject.transform.localPosition = Vector3.zero;
    }
}
