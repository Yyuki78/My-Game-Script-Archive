using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianAttack : MonoBehaviour, IAttack
{
    public bool isGameMode = false;//falseが杖一本,trueが杖二本

    public bool isMove;
    private bool canAttack = false;
    private bool isFinish = false;
    private int FinishCircleNum = 0;
    private bool isBigVibrate = false;

    private float charaY = 0f;
    [SerializeField] float moveSpeed = 0.625f;
    [SerializeField] float rotateSpeed = 1f;

    private GameObject MagicCircle;
    private ParticleSystem[] rotateCircleParticle;
    private GameObject MagicFireGuide;
    private GameObject Meteor;

    private AttackManager _manager;
    private AttackAnimation _anim;
    private Transform _playerTrans;
    public Transform _attackedPieceTrans;
    public Transform _attackedPieceRotateTrans;
    private OVRScreenFade _fade;
    private AttackGauge _attackGauge;

    private void Start()
    {
        Invoke("Set", 0.1f);
    }

    private void Set()
    {
        _manager = GetComponent<AttackManager>();
        _anim = GetComponentInChildren<AttackAnimation>();
        _anim.isMagicMode = true;
        _playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _attackedPieceTrans = GameObject.FindGameObjectWithTag("AttackedPiece").GetComponent<Transform>();
        _attackedPieceRotateTrans = _attackedPieceTrans.GetChild(0).gameObject.GetComponent<Transform>();
        charaY = _attackedPieceTrans.position.y;
        _fade = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<OVRScreenFade>();
        _attackGauge = _anim.attackGauge;
    }

    //ゲームスタート
    public void SetMagicCircle(GameObject circle, GameObject obj, GameObject guide, GameObject meteor, bool mode = false)
    {
        MagicCircle = circle;
        rotateCircleParticle = obj.GetComponentsInChildren<ParticleSystem>();
        rotateCircleParticle[0].gameObject.SetActive(false);
        rotateCircleParticle[2].gameObject.SetActive(false);
        MagicFireGuide = guide;
        MagicFireGuide.SetActive(false);
        Meteor = meteor;
        StartCoroutine(waitMove());
        isGameMode = mode;
    }

    //プレイヤーの近くまで移動
    private IEnumerator waitMove()
    {
        yield return new WaitForSeconds(12f);
        _anim.MoveAnimation(true);
        isMove = true;
    }
    
    private void Update()
    {
        if (canAttack)
        {
            // X(右はA) Button押下又はY(右はB) Button押下
            if (OVRInput.GetDown(OVRInput.Button.One)|| OVRInput.GetDown(OVRInput.Button.Two))
            {
                canAttack = false;
                MagicFireGuide.SetActive(false);
                Instantiate(Meteor, _attackedPieceTrans.position, Quaternion.identity, transform);
                StartCoroutine(HitMeteor());
            }
        }

        if (!isMove) return;
        Vector3 targetPos = _playerTrans.position;
        targetPos.y = charaY;

        _attackedPieceTrans.position = Vector3.MoveTowards(_attackedPieceTrans.position, targetPos, moveSpeed * Time.deltaTime);
        Vector3 relativePos = targetPos - _attackedPieceRotateTrans.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        _attackedPieceRotateTrans.rotation = Quaternion.Slerp(_attackedPieceRotateTrans.rotation, rotation, rotateSpeed);
        if (Vector3.Distance(targetPos, _attackedPieceTrans.position) < 1.25f)
        {
            isMove = false;
            _anim.MoveAnimation(false);
            StartCoroutine(ActiveGuide());
        }
    }

    //攻撃誘導オブジェクトの表示
    private IEnumerator ActiveGuide()
    {
        yield return new WaitForSeconds(3.5f);
        canAttack = true;
        MagicFireGuide.SetActive(true);
        yield break;
    }

    //メテオの発射
    private IEnumerator HitMeteor()
    {
        AudioManager.Instance.StopSE(20);
        AudioManager.Instance.SE(8);
        var wait = new WaitForSeconds(1f);
        MagicCircle.SetActive(false);
        yield return wait;
        if (isFinish) yield break;
        isFinish = true;
        rotateCircleParticle[0].gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        _anim.KnockBack();
        StartCoroutine(Vibrate(0.5f, 1.0f, 0.6f));
        yield return wait;
        if (FinishCircleNum <= (isGameMode ? 7 : 5))
            _attackGauge.gaugeValue = FinishCircleNum * (isGameMode ? 14.2857143f : 20f);
        else
            _attackGauge.gaugeValue = 100 + (Mathf.Min((isGameMode ? 14 : 10), FinishCircleNum) - (isGameMode ? 7 : 5)) * (isGameMode ? 7.14285714f : 10f);
        _manager.ShowResult();
        yield break;
    }

    //魔法陣が一つ完成した
    public void FinishOneMagicCircle(int num)
    {
        if (!rotateCircleParticle[0].gameObject.activeSelf)
            rotateCircleParticle[0].gameObject.SetActive(true);

        var main = rotateCircleParticle[0].main;
        main.startSize = 1f + Mathf.Min(num, (isGameMode ? 12 : 9)) * (isGameMode ? 0.15f : 0.2f);
        main = rotateCircleParticle[1].main;
        main.startSize = 1f / 3f + Mathf.Min(num, (isGameMode ? 12 : 9)) * (isGameMode ? 0.075f : 0.1f);
        var sizeModule = rotateCircleParticle[1].sizeOverLifetime;
        sizeModule.yMultiplier = 0.1f + Mathf.Min(num, (isGameMode ? 12 : 9)) * (isGameMode ? 0.075f : 0.1f);
        if (num == (isGameMode ? 7 : 5))
            rotateCircleParticle[2].gameObject.SetActive(true);
        if (rotateCircleParticle[2].gameObject.activeSelf)
        {
            var emissionModule = rotateCircleParticle[2].emission;
            ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emissionModule.burstCount];
            emissionModule.GetBursts(bursts);
            bursts[0].count = 25 + Mathf.Min(num, (isGameMode ? 13 : 10)) * (isGameMode ? 6 : 8);
            emissionModule.SetBursts(bursts);
        }

        AudioManager.Instance.SE(7);
        AudioManager.Instance.MagicCircleSound(num);

        FinishCircleNum = num;
    }

    //当たった
    public void Hit(int type, Vector3 pos, float speed = 0f, OVRInput.Controller controller = OVRInput.Controller.Active)
    {
        if (type == 0)
            StartCoroutine(Vibrate(0.1f, 0.3f, 0.2f, false, controller));
        else
            StartCoroutine(Vibrate(0.1f, 0.5f, 0.3f, false, controller));
    }

    //敵が攻撃を行った(強制終了)
    public void HitEnemyAttack(int type = 0, OVRInput.Controller controller = OVRInput.Controller.Active)
    {
        if (isFinish) return;
        isFinish = true;
        canAttack = false;
        MagicFireGuide.SetActive(false);
        MagicCircle.SetActive(false);
        StartCoroutine(Vibrate(0.35f, 1.0f, 0.6f, true));
        StartCoroutine(WaitBigVibrate());
        _anim.isAttacking = false;
        _fade.fadeColor = new Color(1, 0, 0);
        _fade.fadeTime = 0f;
        _fade.FadeHitEffect();
        AudioManager.Instance.StopSE(20);
        AudioManager.Instance.SE(5);
    }

    //大きな振動は中断されないようにする
    WaitForSeconds wait2 = new WaitForSeconds(0.3f);
    private IEnumerator WaitBigVibrate()
    {
        yield return null;
        isBigVibrate = true;
        yield return wait2;
        isBigVibrate = false;
        yield break;
    }

    //コントローラーを振動させる
    private IEnumerator Vibrate(float duration = 0.1f, float frequency = 0.1f, float amplitude = 0.1f, bool type = false, OVRInput.Controller controller = OVRInput.Controller.Active)
    {
        if (!isBigVibrate)
            OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(duration);
        if (type)
        {
            _fade.fadeTime = 2f;
            _fade.FinishHitEffect();
        }
        if (!isBigVibrate)
            OVRInput.SetControllerVibration(0, 0, controller);
        if (!type) yield break;
        if (FinishCircleNum <= (isGameMode ? 7 : 5))
            _attackGauge.gaugeValue = FinishCircleNum * (isGameMode ? 7.14285714f : 10f);
        else
            _attackGauge.gaugeValue = 50 + (Mathf.Min((isGameMode ? 14 : 10), FinishCircleNum) - (isGameMode ? 7 : 5)) * (isGameMode ? 3.57142857f : 5f);
        yield return new WaitForSeconds(duration);
        if (type)
            _fade.fadeColor = new Color(0, 0, 0);
        _manager.ShowResult();
        yield break;
    }
}
