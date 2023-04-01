using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthShatter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.layer = 12;
            other.gameObject.GetComponentInParent<EnemyMove>().ChangeSpeed(0);
            other.gameObject.GetComponentInParent<EnemyMove>().HitItem(0.15f);
        }
    }
}
