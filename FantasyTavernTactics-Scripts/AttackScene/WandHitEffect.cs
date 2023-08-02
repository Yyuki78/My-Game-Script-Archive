using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandHitEffect : MonoBehaviour
{
    private ParticleSystem[] _particle = new ParticleSystem[4];
    void Start()
    {
        _particle = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MagicSphere")
        {
            for(int i = 0; i < _particle.Length; i++)
                _particle[i].Play();
        }
    }
}
