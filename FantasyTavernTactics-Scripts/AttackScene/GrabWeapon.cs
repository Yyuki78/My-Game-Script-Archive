using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabWeapon : MonoBehaviour
{
    public bool isGrabRightHand { private set; get; }
    [SerializeField] bool isMace = false;

    private bool isFirstGrab = true;
    [SerializeField] GameObject SwordPointer;
    [SerializeField] GameObject SwordPointer2;

    private AttackManager _manager;
    private OVRGrabbableExtended grabbable;
    private Rigidbody _rigid;
    private AudioSource _audio;

    private void Awake()
    {
        _manager = GetComponentInParent<AttackManager>();
        grabbable = GetComponent<OVRGrabbableExtended>();
        _rigid = GetComponent<Rigidbody>();
        _audio = GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        _rigid.isKinematic = true;
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
        _rigid.isKinematic = true;
    }

    private void OnGrabbed()
    {
        if (grabbable.grabbedBy.isRightHand)
            isGrabRightHand = true;
        else
            isGrabRightHand = false;
        if (!isMace)
            _rigid.isKinematic = false;
        if (isFirstGrab)
        {
            isFirstGrab = false;
            if (SwordPointer != null)
                SwordPointer.SetActive(true);
            if (SwordPointer2 != null)
                SwordPointer2.SetActive(true);
            _manager.StartGame();
        }
        _audio.pitch = 1.0f + Random.Range(-0.1f, 0.1f);
        _audio.PlayOneShot(_audio.clip);
    }

    public void FinishAttack()
    {
        StartCoroutine(Finish());
    }

    //ƒ‰ƒO‚È‚Ì‚©ˆ—‚ªd‚¢‚Ì‚©A•Ší‚ğè•ú‚¹‚È‚¢‚±‚Æ‚ª‚ ‚é‚Ì‚Å•¡”‰ñŒÄ‚Ô
    private IEnumerator Finish()
    {
        if (grabbable.isGrabbed)
        {
            grabbable.grabbedBy.ForceRelease(grabbable);
            OnReleased();
        }
        Destroy(gameObject);
        yield break;
    }
}
