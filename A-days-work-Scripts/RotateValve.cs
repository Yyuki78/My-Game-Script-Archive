using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateValve : MonoBehaviour
{
    private TaskManager _manager;
    private AudioSource _audio;

    private bool once = true;
    private bool once2 = true;

    private void Start()
    {
        _manager = GetComponentInParent<TaskManager>();
        _audio = GetComponent<AudioSource>();
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        if (!once) return;
        if (transform.eulerAngles.y > 90 || transform.eulerAngles.y < -90)
        {
            var outline = GetComponent<Outline>();
            if (outline != null)
            {
                if (once)
                {
                    once = false;

                    _manager.FinishTask(0);
                    _manager.FinishTaskNumber(8);

                    Destroy(GetComponent<Outline>());
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && once2)
        {
            once2 = false;
            _audio.Play();
        }
    }
}
