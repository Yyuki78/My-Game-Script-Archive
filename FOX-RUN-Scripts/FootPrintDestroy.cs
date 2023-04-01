using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrintDestroy : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Disappearing());
    }

    IEnumerator Disappearing()
    {
        int step = 60;
        for (int i = 0; i < step; i++)
        {
            yield return new WaitForSeconds(1f);
            GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1 - 1.0f * i / step);
            yield return null;
        }
        Destroy(gameObject);
    }
}