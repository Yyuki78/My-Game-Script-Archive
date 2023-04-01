using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustSpeed : MonoBehaviour
{
    private EnemyMove _move;

    void Awake()
    {
        _move = GetComponentInParent<EnemyMove>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            _move.ChangeSpeed(other.gameObject.GetComponentInParent<EnemyMove>().speed);
        }
    }
}
