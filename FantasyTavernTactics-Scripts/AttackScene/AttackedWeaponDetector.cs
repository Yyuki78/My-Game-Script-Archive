using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedWeaponDetector : MonoBehaviour
{
    [SerializeField] bool isShield = false;//武器か盾か
    [SerializeField] GameObject hitParticle;
    [SerializeField] AudioClip shieldClip;
    private AttackedCharaMove _move;
    private AudioSource _audio;

    private void Awake()
    {
        _move = GetComponentInParent<AttackedCharaMove>();
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _audio.pitch = 1.0f + Random.Range(-0.1f, 0.1f);
        if (!isShield)
        {
            if (other.tag == "Sword" || other.tag == "SwordBlue")
            {
                //剣用パーティクル
                hitParticle.SetActive(true);
                StartCoroutine(hideParticle());
                if (other.GetComponentInParent<GrabWeapon>().isGrabRightHand)
                    _move.HitAttack(1, OVRInput.Controller.RTouch);
                else
                    _move.HitAttack(1, OVRInput.Controller.LTouch);
                _audio.PlayOneShot(_audio.clip);
            }
            else if (other.tag == "Shield")
            {
                hitParticle.SetActive(true);
                StartCoroutine(hideParticle());
                if (other.GetComponentInParent<GrabWeapon>().isGrabRightHand)
                    _move.HitAttack(3, OVRInput.Controller.RTouch);
                else
                    _move.HitAttack(3, OVRInput.Controller.LTouch);
                _audio.PlayOneShot(shieldClip);
            }
            else if (other.tag == "Player")
            {
                _move.HitAttack();
            }
        }
        else
        {
            if (other.tag == "Sword" || other.tag == "SwordBlue")
            {
                //剣用パーティクル
                hitParticle.SetActive(true);
                StartCoroutine(hideParticle());
                if (other.GetComponentInParent<GrabWeapon>().isGrabRightHand)
                    _move.HitAttack(2, OVRInput.Controller.RTouch);
                else
                    _move.HitAttack(2, OVRInput.Controller.LTouch);
                _audio.PlayOneShot(_audio.clip);
            }
        }
    }

    private IEnumerator hideParticle()
    {
        yield return new WaitForSeconds(0.3f);
        hitParticle.SetActive(false);
    }
}
