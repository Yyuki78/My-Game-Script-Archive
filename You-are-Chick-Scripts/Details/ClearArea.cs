using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearArea : MonoBehaviour
{
    [SerializeField] GameObject StageController;
    private bool once = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && once)
        {
            once = false;
            StageController.GetComponent<IClear>().Clear();
        }
    }
}
