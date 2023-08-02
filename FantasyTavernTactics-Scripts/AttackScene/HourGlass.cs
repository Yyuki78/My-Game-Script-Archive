using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourGlass : MonoBehaviour
{
    [SerializeField] float AccumulatedDamage = 0f;

    [SerializeField] AudioClip[] clip = new AudioClip[4];

    private bool isStopTime = false;
    private bool isGaugeSound = true;

    private GameObject[] ParticleObj = new GameObject[3];

    private OVRGrabbableExtended grabbable;
    private OVRInput.Controller controller;
    private AudioSource _audio;
    private AttackManager _manager;

    private AttackAnimation _ani;

    private void Awake()
    {
        ParticleObj[0] = transform.GetChild(1).gameObject;
        ParticleObj[1] = transform.GetChild(2).gameObject;
        ParticleObj[2] = transform.GetChild(3).gameObject;
        grabbable = GetComponent<OVRGrabbableExtended>();
        _audio = GetComponentInChildren<AudioSource>();
        _manager = GetComponentInParent<AttackManager>();
    }

    void Start()
    {
        _ani = GameObject.FindGameObjectWithTag("AttackedPiece").GetComponentInChildren<AttackAnimation>();
        ParticleObj[0].SetActive(false);
        ParticleObj[1].SetActive(false);
        ParticleObj[2].SetActive(false);
    }

    void Update()
    {
        if (!grabbable.isGrabbed) return;
        if (OVRInput.GetDown(OVRInput.Button.One, controller) || OVRInput.GetDown(OVRInput.Button.Two, controller) ||
            OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, controller) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller) ||
            OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger, controller))
        {
            //Grip‚ÆMenuˆÈŠO‚Ì‰½‚©‚µ‚ç‚Ìƒ{ƒ^ƒ“‚ª‰Ÿ‚³‚ê‚½
            if (AccumulatedDamage < 30f)
            {
                //‰¹
                _audio.PlayOneShot(clip[1]);
                return;
            }
            else
            {
                StartCoroutine(StopTime(Mathf.Min(AccumulatedDamage / 10f, 5f)));
            }
        }
    }

    WaitForSeconds wait = new WaitForSeconds(0.5f);
    private IEnumerator StopTime(float time)
    {
        ParticleObj[1].SetActive(true);
        ParticleObj[2].SetActive(false);
        _audio.PlayOneShot(clip[2]);
        AccumulatedDamage = 0f;
        isStopTime = true;
        _manager.isPauseTimer = true;
        _ani.TogglePlayback(true);
        AudioManager.Instance.TogglePlaybackBGM(false);
        isGaugeSound = true;
        yield return wait;
        yield return wait;
        AudioManager.Instance.SE(21);
        yield return new WaitForSeconds(time - 2f);
        AudioManager.Instance.SE(22);
        yield return wait;
        AudioManager.Instance.StopSE(21);
        yield return wait;
        isStopTime = false;
        _manager.isPauseTimer = false;
        _ani.TogglePlayback(false);
        AudioManager.Instance.TogglePlaybackBGM(true);
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

    public void PlusAccumulatedDamage(float num)
    {
        if (isStopTime) num /= 2;
        AccumulatedDamage += num;
        if (AccumulatedDamage > 30f && AccumulatedDamage < 40f && isGaugeSound)
        {
            isGaugeSound = false;
            _audio.PlayOneShot(clip[3]);
            ParticleObj[2].SetActive(true);
        }
        if (AccumulatedDamage > 50f && !isGaugeSound)
        {
            isGaugeSound = true;
            _audio.PlayOneShot(clip[0]);
            ParticleObj[0].SetActive(true);
        }
    }
}
