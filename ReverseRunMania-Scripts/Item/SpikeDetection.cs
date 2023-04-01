using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDetection : MonoBehaviour
{
    private PlayerInformation _info;

    void Awake()
    {
        _info = GetComponentInParent<PlayerInformation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            AudioManager.instance.SE(22);
            other.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.layer = 12;
            other.gameObject.GetComponentInParent<EnemyMove>().ChangeSpeed(0);
            other.gameObject.GetComponentInParent<EnemyMove>().HitItem(0.25f);
            _info.isSpike = false;
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _info.isSpike = true;
    }
}
