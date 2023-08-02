using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderAttack : MonoBehaviour,IAttack
{
    public int isGameMode = 0;//f0Ç™èeÇ∆åï,1Ç™èeìÒÇ¬,2Ç™èeÇ∆éûé~Çﬂéûåv

    public bool isStart = false;
    public bool isMove;
    public bool isRotate = true;
    public bool isCombo;
    public bool isTrail;
    public bool isGaugeMax;

    private float KnockBackDamage = 0f;
    private int KnockBackMount = 12;
    private bool isBigVibrate = false;

    private float damage = 0f;
    private int comboNum = 0;
    private bool reachComboMax = false;
    private Coroutine playCoroutine;
    private GameObject swordTrail;

    private float lastAttackDamage = 0f;

    private bool once = true;
    private float charaY = 0f;
    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float rotateSpeed = 1f;
    private float moveDistance = 2.25f;

    private HourGlass _hourGlass;
    private AttackedCharaMove _move;
    private AttackAnimation _anim;
    private Transform _playerTrans;
    public Transform _attackedPieceTrans;
    public Transform _attackedPieceRotateTrans;
    private AttackGauge _attackGauge;
    private ScoreTextObjectPool _scoreTextObjectPool;
    private OVRScreenFade _fade;

    private void Start()
    {
        Invoke("Set", 0.1f);
    }

    private void Set()
    {
        swordTrail = GameObject.FindGameObjectWithTag("WeaponTrail");
        swordTrail.SetActive(false);
        _move = GetComponentInChildren<AttackedCharaMove>();
        _anim = GetComponentInChildren<AttackAnimation>();
        _playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _attackedPieceTrans = GameObject.FindGameObjectWithTag("AttackedPiece").GetComponent<Transform>();
        _attackedPieceRotateTrans = _attackedPieceTrans.GetChild(0).gameObject.GetComponent<Transform>();
        charaY = _attackedPieceTrans.position.y;
        _attackGauge = _anim.attackGauge;
        _scoreTextObjectPool = GetComponentInChildren<ScoreTextObjectPool>();
        _fade = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<OVRScreenFade>();
    }

    public void SetModeDecide(int mode)
    {
        isGameMode = mode;
        if (mode == 1)
        {
            moveDistance = 1.5f;
            moveSpeed = 0.5f;
        }
        else if (mode == 2)
        {
            _hourGlass = GameObject.FindGameObjectWithTag("HourGlass").GetComponent<HourGlass>();
        }
    }

    private void Update()
    {
        if (!isStart) return;
        Vector3 targetPos = _playerTrans.position;
        targetPos.y = charaY;

        Vector3 diff = targetPos - _attackedPieceRotateTrans.position;
        Vector3 axis = Vector3.Cross(_attackedPieceRotateTrans.forward, diff);
        float angleDiff = Vector3.Angle(diff, -_attackedPieceRotateTrans.forward) * (axis.y < 0 ? -1 : 1);

        if (angleDiff < 0f && angleDiff > -120f && isRotate && !isMove && !_anim.AttackState)
            StartCoroutine(WaitRotate(false));
        if (angleDiff > 0f && angleDiff < 120f && isRotate && !isMove && !_anim.AttackState)
            StartCoroutine(WaitRotate(true));

        if (Vector3.Distance(targetPos, _attackedPieceTrans.position) > moveDistance)
            MoveChara();

        if (isMove)
        {
            _attackedPieceTrans.position = Vector3.MoveTowards(_attackedPieceTrans.position, targetPos, moveSpeed * Time.deltaTime);
            Vector3 relativePos = targetPos - _attackedPieceRotateTrans.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            _attackedPieceRotateTrans.rotation = Quaternion.Slerp(_attackedPieceRotateTrans.rotation, rotation, rotateSpeed);
            if (Vector3.Distance(targetPos, _attackedPieceTrans.position) < 1.5f)
            {
                once = true;
                isMove = false;
                _anim.MoveAnimation(false);
            }
        }
    }

    //ÉLÉÉÉâÇÃâÒì]ë“Çø
    private IEnumerator WaitRotate(bool type)
    {
        isRotate = false;
        _anim.RotateAnimation(type);
        yield return new WaitForSeconds(3f);
        isRotate = true;
        yield break;
    }

    //ÉvÉåÉCÉÑÅ[ÇÃãﬂÇ≠Ç‹Ç≈à⁄ìÆ
    private void MoveChara()
    {
        if (!once) return;
        once = false;
        isMove = true;
        _anim.MoveAnimation(true);
    }

    //ìñÇΩÇ¡ÇΩ
    public void Hit(int type, Vector3 pos, float speed = 0f, OVRInput.Controller controller = OVRInput.Controller.Active)
    {
        if (type != 0) return;
        StartCoroutine(Vibrate(0.1f, 0.5f, 0.3f, 0, controller));

        if (isCombo && comboNum < 10)
            comboNum++;
        if (isCombo && comboNum == 10 && !reachComboMax)
        {
            reachComboMax = true;
            AudioManager.Instance.SE(6);
        }

        if (!_attackGauge.isGaugeMax)
        {
            if (isCombo)
                damage = 1f + (comboNum * 0.175f);
            else
                damage = 1f;
        }
        else
        {
            if (isCombo)
                damage = 0.35f + (comboNum * 0.1f);
            else
                damage = 0.35f;
        }
        _scoreTextObjectPool.Active(pos, comboNum);
        KnockBackDamage += damage;
        if (KnockBackDamage > KnockBackMount)
        {
            _anim.KnockBack(false);
            KnockBackDamage = 0f;
            KnockBackMount *= 2;
        }
        _attackGauge.ChangeGauge(damage);
        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }
        playCoroutine = StartCoroutine(chainTimer());
    }

    WaitForSeconds wait = new WaitForSeconds(3f);
    private IEnumerator chainTimer()
    {
        if (isGameMode != 1)
            swordTrail.SetActive(true);
        isCombo = true;
        yield return wait;
        if (isGameMode != 1)
            swordTrail.SetActive(false);
        isCombo = false;
        comboNum = 0;
        reachComboMax = false;
        yield break;
    }

    //èeÇÃHit
    public void HitBullet(int gunComboNum, OVRInput.Controller controller = OVRInput.Controller.Active)
    {
        _move.hitSphere(0, 1, Vector3.zero, OVRInput.Controller.Active);
        StartCoroutine(Vibrate(0.2f, 1.0f, 0.6f, 0, controller));

        if (!_attackGauge.isGaugeMax)
            damage = 3f + (gunComboNum * 0.4f);
        else
            damage = 1f + (gunComboNum * 0.2f);

        if (isGameMode == 2 && !_attackGauge.isGaugeMax)
        {
            if (!_attackGauge.isGaugeMax)
                damage -= 1f;
            _hourGlass.PlusAccumulatedDamage(damage);
        }

        KnockBackDamage += damage;
        if (KnockBackDamage > KnockBackMount)
        {
            _anim.KnockBack(false);
            KnockBackDamage = 0f;
            KnockBackMount *= 2;
        }
        _attackGauge.ChangeGauge(damage);
    }

    //óºéËèeÉÇÅ[ÉhÇÃHit
    public void DoubleGunHit(int comboNum, bool isRed, OVRInput.Controller controller = OVRInput.Controller.Active)
    {
        if (isRed)
        {
            _move.hitSphere(0, 2, Vector3.zero, OVRInput.Controller.Active);
        }
        else
        {
            _move.hitSphere(0, 1, Vector3.zero, OVRInput.Controller.Active);
        }
        if (!_attackGauge.isGaugeMax)
            damage = 2f + (comboNum * 0.3f);
        else
            damage = 0.75f + (comboNum * 0.15f);
        StartCoroutine(Vibrate(0.2f, 1.0f, 0.6f, 0, controller));
        KnockBackDamage += damage;
        if (KnockBackDamage > KnockBackMount)
        {
            _anim.KnockBack(false);
            KnockBackDamage = 0f;
            KnockBackMount *= 2;
        }
        _attackGauge.ChangeGauge(damage);
    }

    //óºéËèeÉÇÅ[ÉhÇÃç≈å„ÇÃãOê’çUåÇÇ™ìñÇΩÇ¡ÇΩ
    public void HitFinalAttack()
    {
        lastAttackDamage += 1.5f;
    }

    public void LastAttackShow()
    {
        _attackGauge.ChangeGauge(lastAttackDamage);
    }

    //ìGÇÃçUåÇÇ…ìñÇΩÇ¡ÇΩÇ©íeÇ¢ÇΩ
    public void HitEnemyAttack(int type = 0, OVRInput.Controller controller = OVRInput.Controller.Active)
    {
        switch (type)
        {
            case 1:
                if (!_anim.isAttacking || !_anim.CanAttack)
                {
                    StartCoroutine(Vibrate(0.1f, 0.1f, 0.1f, 0, controller));
                    break;
                }
                //çUåÇÇíeÇ¢ÇΩ
                AudioManager.Instance.SE(23);
                _anim.CanAttack = false;
                StartCoroutine(Vibrate(0.5f, 1.0f, 0.6f, 0, controller));
                StartCoroutine(WaitBigVibrate());
                _anim.isAttacking = false;
                KnockBackDamage += KnockBackMount / 2f;
                if (KnockBackDamage > KnockBackMount)
                {
                    _anim.KnockBack(false);
                    KnockBackDamage = 0f;
                    KnockBackMount *= 2;
                }
                break;
            case 0://ëÃÇ…ìñÇΩÇ¡ÇΩ
            case 2://ëäéËÇÃåïÇ∆é©ï™ÇÃèÇÇ™Ç‘Ç¬Ç©Ç¡ÇΩ(éwäˆäØÇ»ÇÃÇ≈Ç†ÇËìæÇ»Ç¢)
                if (!_anim.isAttacking || !_anim.CanAttack)
                {
                    StartCoroutine(Vibrate());
                    break;
                }
                _anim.CanAttack = false;
                StartCoroutine(Vibrate(0.5f, 1.0f, 0.6f, 1));
                StartCoroutine(WaitBigVibrate());
                _anim.isAttacking = false;
                _fade.fadeColor = new Color(1, 0, 0);
                _fade.fadeTime = 0f;
                _fade.FadeHitEffect();
                _attackGauge.ChangeGauge(-20f);
                AudioManager.Instance.SE(5);
                break;
            case 3:
                //ëäéËÇÃèÇÇ…Ç‘Ç¬Ç©Ç¡ÇΩ
                KnockBackDamage -= (damage / 2);
                if (KnockBackDamage < 0)
                    KnockBackDamage = 0;
                break;
            case 4://ëäéËÇÃìÀåÇï∫ÇÃçUåÇÇãÚÇÁÇ¡ÇΩ
                if (!_anim.isAttacking)
                {
                    StartCoroutine(Vibrate());
                    break;
                }
                StartCoroutine(Vibrate(0.2f, 0.5f, 0.3f, 2));
                _fade.fadeColor = new Color(1, 0, 0);
                _fade.fadeTime = 0f;
                _fade.FadeHitEffect();
                _attackGauge.ChangeGauge(-5f);
                AudioManager.Instance.SE(5);
                break;
            default:
                StartCoroutine(Vibrate());
                break;
        }
    }

    //ëÂÇ´Ç»êUìÆÇÕíÜífÇ≥ÇÍÇ»Ç¢ÇÊÇ§Ç…Ç∑ÇÈ
    WaitForSeconds wait2 = new WaitForSeconds(0.45f);
    private IEnumerator WaitBigVibrate()
    {
        yield return null;
        isBigVibrate = true;
        yield return wait2;
        isBigVibrate = false;
        yield break;
    }

    //ÉRÉìÉgÉçÅ[ÉâÅ[ÇêUìÆÇ≥ÇπÇÈ
    private IEnumerator Vibrate(float duration = 0.1f, float frequency = 0.1f, float amplitude = 0.1f, int type = 0, OVRInput.Controller controller = OVRInput.Controller.Active)
    {
        if (!isBigVibrate)
            OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(duration);
        if (type != 0)
        {
            if (type == 1)
            {
                _fade.fadeTime = 2f;
                _fade.FinishHitEffect();
            }
            else
            {
                _fade.fadeTime = 1f;
                _fade.FinishHitEffect();
            }
        }
        if (!isBigVibrate)
            OVRInput.SetControllerVibration(0, 0, controller);
        yield return new WaitForSeconds(duration);
        if (type != 0)
            _fade.fadeColor = new Color(0, 0, 0);
        yield break;
    }
}
