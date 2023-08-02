using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPiece : MonoBehaviour
{
    public bool isGrabbed { private set; get; } = false;
    private bool isFirstGrab = false;

    [SerializeField] float moveSpeed = 5f;
    private Vector3 initialLocalPosition;
    private Vector3 initialLocalRotation;
    private Transform initialParent;

    private OVRGrabbableExtended grabbable;
    private AudioSource _audio;
    private TutorialManager _tutorialManager;

    [SerializeField] AudioClip clip;

    private void Awake()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation.eulerAngles;
        initialParent = transform.parent;
        grabbable = GetComponent<OVRGrabbableExtended>();
        _audio = GetComponent<AudioSource>();
        _tutorialManager = GetComponentInParent<TutorialManager>();
    }

    private void Update()
    {
        if (!isFirstGrab || isGrabbed) return;
        if (Vector3.Distance(transform.position, initialLocalPosition) > 0.005f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialLocalPosition, moveSpeed * Time.deltaTime);
            transform.localEulerAngles = initialLocalRotation;
        }
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
        isGrabbed = false;
        transform.SetParent(initialParent);
        transform.localEulerAngles = initialLocalRotation;
    }

    private void OnGrabbed()
    {
        if(_tutorialManager._currentState != TutorialManager.TutorialState.GrabPiece)
        {
            grabbable.grabbedBy.ForceRelease(grabbable);
            OnReleased();
        }
        else
        {
            _audio.PlayOneShot(_audio.clip);
            isGrabbed = true;
            isFirstGrab = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PiecePlace" && _tutorialManager._currentState == TutorialManager.TutorialState.GrabPiece)
        {
            _audio.PlayOneShot(clip);
            _tutorialManager.SetState(TutorialManager.TutorialState.GoStageSelect);
            initialLocalPosition = new Vector3(5.5137f, 1.3931f, 4.7142f);
            initialLocalRotation = new Vector3(0, 180f, 0);
            grabbable.grabbedBy.ForceRelease(grabbable);
            OnReleased();
            Destroy(grabbable);
        }
    }
}
