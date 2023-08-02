using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleHitSphere : MonoBehaviour
{
    [SerializeField] int type = 0;

    private MagicCircleLineRenderer _renderer;

    void Start()
    {
        _renderer = GetComponentInParent<MagicCircleLineRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wand")
        {
            if(other.GetComponentInParent<GrabWeapon>().isGrabRightHand)
                _renderer.HitSphere(this.gameObject, type, OVRInput.Controller.RTouch);
            else
                _renderer.HitSphere(this.gameObject, type, OVRInput.Controller.LTouch);
            gameObject.SetActive(false);
        }
    }
}
