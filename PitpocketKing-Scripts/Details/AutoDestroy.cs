using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float time = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", time);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
