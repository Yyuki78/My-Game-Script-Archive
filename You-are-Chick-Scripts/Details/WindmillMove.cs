using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillMove : MonoBehaviour
{
    private Rigidbody2D _rigid;

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        AudioManager.instance.SE(10);
        _rigid.AddTorque(0.5f, ForceMode2D.Impulse);
    }
}
