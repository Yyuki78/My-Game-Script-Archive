using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSphere : MonoBehaviour
{
    [SerializeField] int sphereNum;
    [SerializeField] bool isOrange;
    public int Number => sphereNum;
    public bool Color => isOrange;
    private int type = 0;
    
    private Transform _transform;
    private AttackedCharaMove _move;

    private bool canHit = false;
    private Collider _collider;
    private MeshRenderer _renderer;
    private ParticleSystem[] _particles;
    private AudioSource _audio;

    [SerializeField] AudioClip[] clip = new AudioClip[10];

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _move = GetComponentInParent<AttackedCharaMove>();

        _collider = GetComponent<Collider>();
        _renderer = GetComponent<MeshRenderer>();
        _particles = GetComponentsInChildren<ParticleSystem>();
        _audio = GetComponent<AudioSource>();

        _collider.enabled = false;
        _renderer.enabled = false;
        _particles[0].Stop();

        if (isOrange)
            type = 0;
        else
            type = 1;
    }

    public void Init()
    {
        canHit = true;
        _collider.enabled = true;
        _renderer.enabled = true;
        _particles[0].Play();
        if (sphereNum == 100)
            _particles[1].Play();
    }

    public void Hit()
    {
        canHit = false;
        _collider.enabled = false;
        _renderer.enabled = false;
        _particles[0].Stop();
        if (sphereNum == 100)
            _particles[0].Stop();
        else
            _particles[1].Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canHit) return;
        if (other.tag == "Sword" && isOrange)
        {
            if (other.GetComponentInParent<GrabWeapon>().isGrabRightHand)
                _move.hitSphere(sphereNum, type, _transform.position, OVRInput.Controller.RTouch);
            else
                _move.hitSphere(sphereNum, type, _transform.position, OVRInput.Controller.LTouch);
            Hit();
            HitSound(true);
        }
        if (other.tag == "SwordBlue" && !isOrange)
        {
            if (other.GetComponentInParent<GrabWeapon>().isGrabRightHand)
                _move.hitSphere(sphereNum, type, _transform.position, OVRInput.Controller.RTouch);
            else
                _move.hitSphere(sphereNum, type, _transform.position, OVRInput.Controller.LTouch);
            Hit();
            HitSound(true);
        }
        if (other.tag == "Shield" && !isOrange && sphereNum == 100)
        {
            float speed = other.GetComponentInParent<WeaponSpeed>().GetHighestSpeed();
            if (other.GetComponentInParent<GrabWeapon>().isGrabRightHand)
                _move.ShieldBashCylinder(2, speed, OVRInput.Controller.RTouch);
            else
                _move.ShieldBashCylinder(2, speed, OVRInput.Controller.LTouch);
            Hit();
            _audio.PlayOneShot(_audio.clip);
        }
        if(other.tag == "Mace" && isOrange)
        {
            float speed = other.GetComponentInParent<WeaponSpeed>().GetHighestSpeed();
            if (GameObject.FindGameObjectWithTag("Haft").GetComponentInParent<GrabWeapon>().isGrabRightHand)
                _move.hitSphere(sphereNum, type, _transform.position, OVRInput.Controller.RTouch, speed);
            else
                _move.hitSphere(sphereNum, type, _transform.position, OVRInput.Controller.LTouch, speed);
            Hit();
            HitSound(false);
        }
    }

    private void HitSound(bool type)
    {
        int ran;
        if (type)
            ran = Random.Range(0, 10);
        else
            ran = Random.Range(9, 14);
        _audio.pitch = 1.0f + Random.Range(-0.1f, 0.1f);
        _audio.PlayOneShot(clip[ran]);
    }
}
