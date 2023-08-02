using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class CupGrabBehavior : MonoBehaviour
{
    [SerializeField] int CupNumber;

    private Vector3 initialLocalPosition;
    private Transform initialParent;

    private ThreeCupsGame _manager;
    private OVRGrabbableExtended grabbable;
    private AudioSource _audio;

    private void Awake()
    {
        _manager = GetComponentInParent<ThreeCupsGame>();
        grabbable = GetComponent<OVRGrabbableExtended>();
        _audio = GetComponent<AudioSource>();
        initialLocalPosition = transform.localPosition;
        transform.localEulerAngles = new Vector3(0, Random.Range(0, 360f), 0);
        initialParent = transform.parent;
    }

    private void OnEnable()
    {
        // listen for grabs
        grabbable.OnGrabBegin.AddListener(OnGrabbed);
        grabbable.OnGrabEnd.AddListener(OnReleased);
    }

    private void OnDisable()
    {
        // stop listening for grabs
        grabbable.OnGrabBegin.RemoveListener(OnGrabbed);
        grabbable.OnGrabEnd.RemoveListener(OnReleased);

        transform.SetParent(initialParent);
        transform.localPosition = initialLocalPosition;
        transform.localEulerAngles = new Vector3(0, Random.Range(0, 360f), 0);
    }

    private void OnReleased()
    {

    }

    private void OnGrabbed()
    {
        _manager.ShowResult(CupNumber);
        _audio.volume = 0.35f + Random.Range(-0.1f, 0.1f);
        _audio.pitch = 1.0f + Random.Range(-0.1f, 0.1f);
        _audio.PlayOneShot(_audio.clip);
    }

    public void Finish()
    {
        if (grabbable.isGrabbed)
            StartCoroutine(VanishCoroutine());
        else
            gameObject.SetActive(false);
    }

    private IEnumerator VanishCoroutine()
    {
        yield return new WaitForSeconds(2f);
        if (grabbable.isGrabbed)
            grabbable.grabbedBy.ForceRelease(grabbable);
        gameObject.SetActive(false);
    }
}
