using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkDetection : MonoBehaviour
{
    [SerializeField] InkEffect _ink;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _ink.InkSplash();
        }
    }
}
