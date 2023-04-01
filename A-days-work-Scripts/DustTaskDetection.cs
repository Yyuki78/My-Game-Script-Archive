using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustTaskDetection : MonoBehaviour
{
    private WipeTask _wipe;
    private bool once = true;

    // Start is called before the first frame update
    void Start()
    {
        _wipe = GetComponentInParent<WipeTask>();
    }

    private void Update()
    {
        if (transform.position.y <= -1 && once)
        {
            once = false;
            _wipe.wipe();
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Stage" || collision.gameObject.tag == "Sand")
        {
            if (once)
            {
                once = false;
                _wipe.wipe();
                gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Stage" && once)
        {
            once = false;
            _wipe.wipe();
            gameObject.SetActive(false);
        }
    }
}
