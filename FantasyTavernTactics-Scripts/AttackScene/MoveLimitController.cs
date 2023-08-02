using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLimitController : MonoBehaviour
{
    private MeshRenderer _renderer;

    void Start()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
        _renderer.enabled = false;
    }

    public void ChangeMoveLimitCube(int num)
    {
        if (num == 0)
        {
            if (!_renderer.enabled)
                _renderer.enabled = true;
        }
        else
        {
            if (_renderer.enabled)
                _renderer.enabled = false;
        }
    }
}
