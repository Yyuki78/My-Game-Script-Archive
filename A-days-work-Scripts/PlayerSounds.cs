using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private CharacterController _controller;
    private AudioSource _audio;
    [SerializeField] AudioClip[] _clip;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_controller.velocity.magnitude == 0)
        {
            _audio.Pause();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (_controller.velocity.magnitude != 0)
        {
            if (collision.gameObject.tag == "Stage")
            {
                _audio.clip = _clip[0];
            }
            if (collision.gameObject.tag == "Sand")
            {
                _audio.clip = _clip[1];
            }
            _audio.Play();
        }
    }
}
