using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwitcher : MonoBehaviour
{
    private bool isSwordMode = true;//trueは前が赤、falseは前が青
    private bool isCanChange = true;

    [SerializeField] GameObject SwordRed;
    [SerializeField] GameObject SwordBlue;

    [SerializeField] Vector3 SetPosition1;
    [SerializeField] Vector3 SetPosition2;
    [SerializeField] Vector3 SetRotation1;
    [SerializeField] Vector3 SetRotation2;

    [SerializeField] AudioClip clip;

    private OVRGrabbableExtended grabbable;
    private OVRInput.Controller controller;
    private AudioSource _audio;
    private AssaulterAttack _attack;

    private void Awake()
    {
        grabbable = GetComponent<OVRGrabbableExtended>();
        _audio = GetComponentInChildren<AudioSource>();
    }

    void Start()
    {
        SwordRed.transform.localPosition = SetPosition1;
        SwordRed.transform.localEulerAngles = SetRotation1;
        SwordBlue.transform.localPosition = SetPosition2;
        SwordBlue.transform.localEulerAngles = SetRotation2;
    }

    void Update()
    {
        if (!grabbable.isGrabbed) return;
        if (!isCanChange) return;

        if (OVRInput.GetDown(OVRInput.Button.One, controller) || OVRInput.GetDown(OVRInput.Button.Two, controller) ||
            OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, controller) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger, controller))
        {
            //GripとMenu以外の何かしらのボタンが押された
            _audio.PlayOneShot(clip);
            if (isSwordMode)
            {
                SwordRed.transform.localPosition = SetPosition1;
                SwordRed.transform.localEulerAngles = SetRotation1;
                SwordBlue.transform.localPosition = SetPosition2;
                SwordBlue.transform.localEulerAngles = SetRotation2;
            }
            else
            {
                SwordBlue.transform.localPosition = SetPosition1;
                SwordBlue.transform.localEulerAngles = SetRotation1;
                SwordRed.transform.localPosition = SetPosition2;
                SwordRed.transform.localEulerAngles = SetRotation2;
            }
            isSwordMode = !isSwordMode;
            isCanChange = false;
            StartCoroutine(SwitchOnce());
        }
    }

    WaitForSeconds wait = new WaitForSeconds(0.2f); 
    private IEnumerator SwitchOnce()
    {
        yield return wait;
        isCanChange = true;
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
    }

    private void OnReleased()
    {
        
    }

    private void OnGrabbed()
    {
        if (grabbable.grabbedBy.isRightHand)
            controller = OVRInput.Controller.RTouch;
        else
            controller = OVRInput.Controller.LTouch;
    }
}
