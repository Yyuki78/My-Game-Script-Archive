using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjectSize : MonoBehaviour
{
    private Vector3 size;

    private void Start()
    {
        size = transform.localScale;
    }

    private void Update()
    {
        //transform.localScale = size;
    }

    public void Resize(Vector3 Nsize)
    {
        Debug.Log("サイズを" + Nsize + "に変更します");
        size = Nsize;
    }

    private IEnumerator setSize(Vector3 size)
    {
        yield return new WaitForSeconds(0.05f);
        this.gameObject.transform.localScale = size;
        yield break;
    }
}
