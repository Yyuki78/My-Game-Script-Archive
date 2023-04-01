using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindParticle : MonoBehaviour
{
    private GameObject Player;
    private PlayerMove _move;
    private float speed;
    private float particleNum = 0;

    private ParticleSystem _particle;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        _move = Player.GetComponent<PlayerMove>();
        _particle = GetComponent<ParticleSystem>();
    }
    
    void Update()
    {
        var em = _particle.emission;
        if (Input.GetAxis("Vertical") > 0f)
            em.rateOverTime = 80f;
        else
            em.rateOverTime = 0f;
    }
}
