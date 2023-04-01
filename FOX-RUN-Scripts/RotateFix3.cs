using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFix3 : MonoBehaviour
{
    GameObject Cam;
    Transform _transform;

    private void Start()
    {
        Cam = GameObject.Find("CM vcam1");
        _transform = Cam.GetComponent<Transform>();
    }

    void Update()
    {
        gameObject.transform.localEulerAngles = new Vector3(-_transform.rotation.x, -_transform.rotation.y, -_transform.rotation.z);
        gameObject.transform.localPosition = new Vector3(-_transform.position.x, -_transform.position.y, -_transform.position.z);
    }
}
