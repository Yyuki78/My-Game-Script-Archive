using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    //当たると軌道を表示する(オブジェクトプール)
    //外さずに連続で当てるとコンボになる
    //撃つ時に銃口からパーティクルを出す
    public bool BattleMode = false;
    [SerializeField] bool isGunRed;

    private bool isFirstGrab = true;
    private bool canShoot = true;
    private bool isGunCombo;
    private int gunComboNum = 0;
    [SerializeField] GameObject GunPointer;
    private OVRInput.Controller controller;

    private Transform ShootPosition;
    [SerializeField] LayerMask hitLayer = 0;
    private ParticleSystem _particle;

    private int TrajectoryNum = 0;
    private LineRenderer[] _lineRenderer = new LineRenderer[60];
    private bool[] canShowLine = new bool[5];
    [SerializeField] Gradient[] LineGradient = new Gradient[6];
    private Coroutine playCoroutine;
    [SerializeField] Material FinishTrajectoryMat;

    private AttackManager _manager;
    private CommanderAttack _attack;
    private OVRGrabbableExtended grabbable;
    private Rigidbody _rigid;

    [SerializeField] AudioClip[] clip = new AudioClip[2];
    private AudioSource _audio;

    private void Awake()
    {
        ShootPosition = transform.GetChild(0);
        _manager = GetComponentInParent<AttackManager>();
        grabbable = GetComponent<OVRGrabbableExtended>();
        _rigid = GetComponent<Rigidbody>();
        _particle = GetComponentInChildren<ParticleSystem>();
        _lineRenderer = GetComponentsInChildren<LineRenderer>();
        _audio = GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        _rigid.isKinematic = true;
        for(int i = 0; i < _lineRenderer.Length; i++)
        {
            _lineRenderer[i].positionCount = 1;
            _lineRenderer[i].SetPosition(0, ShootPosition.position);
        }
        for (int i = 0; i < canShowLine.Length; i++)
            canShowLine[i] = true;
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
            controller = OVRInput.Controller.RTouch;
        else
            controller = OVRInput.Controller.LTouch;
        _rigid.isKinematic = false;
        if (isFirstGrab)
        {
            isFirstGrab = false;
            _manager.StartGame();
            _attack = GetComponentInParent<CommanderAttack>();
            GunPointer.SetActive(true);
        }
        _audio.pitch = 1.0f + Random.Range(-0.1f, 0.1f);
        _audio.PlayOneShot(_audio.clip);
    }

    public void FinishAttack()
    {
        StartCoroutine(Finish());
    }

    //ラグなのか処理が重いのか、武器を手放せないことがあるので複数回呼ぶ
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

    void Update()
    {
        if (!grabbable.isGrabbed) return;

        if (OVRInput.GetDown(OVRInput.Button.One, controller) || OVRInput.GetDown(OVRInput.Button.Two, controller) ||
            OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, controller) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller)||
            OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger, controller) || OVRInput.GetDown(OVRInput.Button.Any, controller))
        {
            //GripとMenu以外の何かしらのボタンが押された
            if (canShoot)
            {
                canShoot = false;
                _audio.pitch = 1.0f + Random.Range(-0.1f, 0.1f);
                _audio.volume = 0.25f;
                _audio.PlayOneShot(clip[0]);
                StartCoroutine(ReloadWait());
                _particle.Play();
                if (Physics.Raycast(ShootPosition.position, ShootPosition.forward, out var hit, 30, hitLayer, QueryTriggerInteraction.UseGlobal))
                {
                    _audio.volume = 0.35f + Random.Range(-0.05f, 0.05f);
                    _audio.PlayOneShot(clip[1]);
                    if (isGunCombo && gunComboNum < 5)
                        gunComboNum++;
                    if (!isGunCombo)
                        isGunCombo = true;
                    if (!BattleMode)
                    {
                        TrajectoryNum = 0;
                        while (!canShowLine[TrajectoryNum])
                            TrajectoryNum++;
                        StartCoroutine(ShowTrajectory(false, TrajectoryNum, gunComboNum, hit.point));
                        _attack.HitBullet(gunComboNum, controller);
                    }
                    else
                    {
                        if (TrajectoryNum < _lineRenderer.Length)
                        {
                            StartCoroutine(ShowTrajectory(true, TrajectoryNum, gunComboNum, hit.point));
                            TrajectoryNum++;
                        }
                        _attack.DoubleGunHit(gunComboNum, isGunRed, controller);
                    }
                    if (playCoroutine != null)
                    {
                        StopCoroutine(playCoroutine);
                        playCoroutine = null;
                    }
                    playCoroutine = StartCoroutine(chainTimer());
                }
            }
        }
    }

    WaitForSeconds wait = new WaitForSeconds(0.25f);
    private IEnumerator ReloadWait()
    {
        yield return wait;
        canShoot = true;
        yield break;
    }

    WaitForSeconds wait2 = new WaitForSeconds(1.25f);
    private IEnumerator ShowTrajectory(bool mode, int num, int comboNum, Vector3 pos)
    {
        _lineRenderer[num].colorGradient = LineGradient[gunComboNum];
        if (!mode)
        {
            canShowLine[num] = false;
            _lineRenderer[num].positionCount = 2;
            _lineRenderer[num].SetPosition(0, ShootPosition.position);
            _lineRenderer[num].SetPosition(1, pos);
            yield return wait2;
            _lineRenderer[num].positionCount = 1;
            canShowLine[num] = true;
        }
        else
        {
            if (_lineRenderer[0].transform.parent.gameObject.transform.parent != null)
                _lineRenderer[0].transform.parent.gameObject.transform.parent = null;
            _lineRenderer[num].gameObject.GetComponent<TrajectoryDetection>().SetLine(ShootPosition.position, pos);
        }
        yield break;
    }

    WaitForSeconds wait3 = new WaitForSeconds(5f);
    private IEnumerator chainTimer()
    {
        isGunCombo = true;
        yield return wait3;
        isGunCombo = false;
        gunComboNum = 0;
        yield break;
    }

    //最後に全ての軌跡で攻撃する(Mode2のみ)
    public void FinishTrajectoryAttack()
    {
        StartCoroutine(finishTrajectoryAttack());
    }

    private IEnumerator finishTrajectoryAttack()
    {
        for (int i = 0; i < _lineRenderer.Length; i++)
        {
            _lineRenderer[i].GetComponent<TrajectoryDetection>().FinishAttack(FinishTrajectoryMat);
            yield return null;
        }
        if(isGunRed)
            _attack.LastAttackShow();
        yield break;
    }
}
