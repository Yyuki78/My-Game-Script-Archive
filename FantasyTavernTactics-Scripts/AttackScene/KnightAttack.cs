using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttack : MonoBehaviour,IAttack
{
    public bool isGameMode = false;

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

    private bool once = true;
    private float charaY = 0f;
    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float rotateSpeed = 1f;

    private float time = 0f;
    private float elapsedShieldTime = 3f;
    private bool isShieldBash = true;
    private bool isShieldBashEffecting = false;

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

    public void Start(bool mode)
    {
        isGameMode = mode;
        if (isGameMode)
        {
            var g = GameObject.FindGameObjectsWithTag("Mace");
            for(int i = 0; i < g.Length; i++)
                g[i].GetComponentInParent<WeaponSpeed>().GameStart();
        }
        GameObject.FindGameObjectWithTag("Shield").GetComponentInParent<WeaponSpeed>().GameStart();
        isStart = true;
    }

    private void Update()
    {
        if (!isStart) return;
        time += Time.deltaTime;
        if (time < 8f && time > 6f && isShieldBash)
        {
            isShieldBash = false;
            StartCoroutine(ShieldBashAction());
        }
        if (time < 16f && time > 14f && isShieldBash)
        {
            isShieldBash = false;
            StartCoroutine(ShieldBashAction());
        }
        if (time < 24f && time > 22f && isShieldBash)
        {
            isShieldBash = false;
            StartCoroutine(ShieldBashAction());
        }

        if (isShieldBashEffecting)
        {
            elapsedShieldTime -= Time.deltaTime;
        }

        Vector3 targetPos = _playerTrans.position;
        targetPos.y = charaY;

        Vector3 diff = targetPos - _attackedPieceRotateTrans.position;
        Vector3 axis = Vector3.Cross(_attackedPieceRotateTrans.forward, diff);
        float angleDiff = Vector3.Angle(diff, -_attackedPieceRotateTrans.forward) * (axis.y < 0 ? -1 : 1);

        if (angleDiff < 0f && angleDiff > -120f && isRotate && !isMove && !_anim.AttackState)
            StartCoroutine(WaitRotate(false));
        if (angleDiff > 0f && angleDiff < 120f && isRotate && !isMove && !_anim.AttackState)
            StartCoroutine(WaitRotate(true));

        if (Vector3.Distance(targetPos, _attackedPieceTrans.position) > 2.25f)
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

    //ÉVÅ[ÉãÉhÉoÉbÉVÉÖ
    private IEnumerator ShieldBashAction()
    {
        float ran = Random.Range(0, 4f);
        yield return new WaitForSeconds(ran);
        yield return null;
        isShieldBashEffecting = true;
        elapsedShieldTime = 3f;
        _move.ShieldBashCylinder(0);
        yield return new WaitForSeconds(3f);
        _move.ShieldBashCylinder(1);
        isShieldBash = true;
        isShieldBashEffecting = false;
        elapsedShieldTime = 3f;
        yield break;
    }

    //ìñÇΩÇ¡ÇΩ
    public void Hit(int type, Vector3 pos, float speed = 0f, OVRInput.Controller controller = OVRInput.Controller.Active)
    {
        if (type == 0)
        {
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
                    damage = (isGameMode ? 0.25f : 1f) + (comboNum * (isGameMode ? 0.05f : 0.125f)) + Mathf.Min(speed, 15f) * 0.15f;
                else
                    damage = (isGameMode ? 0.25f : 1f) + Mathf.Min(speed, 15f) * 0.15f;
            }
            else
            {
                if (isCombo)
                    damage = (isGameMode ? 0f : 0.25f) + (comboNum * (isGameMode ? 0.025f : 0.075f)) + Mathf.Min(speed, 15f) * 0.1f;
                else
                    damage = (isGameMode ? 0f : 0.25f) + Mathf.Min(speed, 15f) * 0.1f;
            }
            //Debug.Log(damage + "  mace  " + speed);
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
        }else if (type == 1)
        {
            StartCoroutine(Vibrate(0.5f, 0.5f, 0.3f, 0, controller));
            StartCoroutine(WaitBigVibrate());
            damage = 5 + elapsedShieldTime * 3f + Mathf.Min(speed, 9f) * 1f;
            KnockBackDamage += damage * 2f;
            _attackGauge.ChangeGauge(damage);
            //Debug.Log(damage + "   " + speed);
        }
    }

    WaitForSeconds wait = new WaitForSeconds(3f);
    private IEnumerator chainTimer()
    {
        swordTrail.SetActive(true);
        isCombo = true;
        yield return wait;
        swordTrail.SetActive(false);
        isCombo = false;
        comboNum = 0;
        reachComboMax = false;
        yield break;
    }

    //ìGÇÃçUåÇÇ…ìñÇΩÇ¡ÇΩÇ©íeÇ¢ÇΩ
    public void HitEnemyAttack(int type = 0, OVRInput.Controller controller = OVRInput.Controller.Active)
    {
        switch (type)
        {
            case 1://çUåÇÇíeÇ¢ÇΩ
                if (!_anim.isAttacking || !_anim.CanAttack)
                {
                    StartCoroutine(Vibrate(0.1f, 0.1f, 0.1f, 0, controller));
                    break;
                }
                AudioManager.Instance.SE(23);
                _anim.CanAttack = false;
                StartCoroutine(Vibrate(0.5f, 1.0f, 0.6f, 0, controller));
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
            case 2://ëäéËÇÃåïÇ∆é©ï™ÇÃèÇÇ™Ç‘Ç¬Ç©Ç¡ÇΩ
                if (!_anim.isAttacking || !_anim.CanAttack)
                {
                    StartCoroutine(Vibrate(0.1f, 0.1f, 0.1f, 0, controller));
                    break;
                }
                AudioManager.Instance.SE(23);
                _anim.CanAttack = false;
                StartCoroutine(Vibrate(0.5f, 1.0f, 0.6f, 1, controller));
                StartCoroutine(WaitBigVibrate());
                _anim.isAttacking = false;
                _anim.KnockBack(true);
                KnockBackDamage = 0f;
                KnockBackMount *= 2;
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
