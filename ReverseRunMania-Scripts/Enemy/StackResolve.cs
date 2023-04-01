using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackResolve : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            if (other.gameObject.transform.position.z <= transform.position.z)
            {
                Destroy(transform.parent.gameObject);
            }
        }
        if (other.CompareTag("BackMostWall"))
        {
            StartCoroutine(destroy());
        }
    }

    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(transform.parent.gameObject);
    }
}
