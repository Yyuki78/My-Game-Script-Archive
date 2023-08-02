using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaulterAttack : MonoBehaviour,IAttack
{
    public bool isGameMode = false;//falseÇ™åïìÒÇ¬,trueÇ™ëoênåï

    public bool isStart = false;
    public bool isMove;
    public bool isRotate = true;
    public bool[] isCombo = new bool[2];
    public bool[] isTrail = new bool[2];
    public bool isGaugeMax;
    public bool isPressSwitch = false;//ëoênåïóp

    private float KnockBackDamage = 0f;
    private int KnockBackMount = 12;
    private bool isBigVibrate = false;

    private float damage = 0f;
    private int[] comboNum = new int[2];
    private bool[] reachComboMax = new bool[2];
    private Coroutine[] playCoroutine = new Coroutine[2];
    private GameObject[] swordTrail = new GameObject[2];
    private ParticleSystem DoubleTrailParticle;//ëoênåïóp

    private bool once = true;
    private float charaY = 0f;
    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float rotateSpeed = 1f;

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
        swordTrail = GameObject.FindGameObjectsWithTag("WeaponTrail");
        swordTrail[0].SetActive(false);
        swordTrail[1].SetActive(false);
        _anim = GetComponentInChildren<AttackAnimation>();
        _playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _attackedPieceTrans = GameObject.FindGameObjectWithTag("AttackedPiece").GetComponent<Transform>();
        _attackedPieceRotateTrans = _attackedPieceTrans.GetChild(0).gameObject.GetComponent<Transform>();
        charaY = _attackedPieceTrans.position.y;
        _attackGauge = _anim.attackGauge;
        _scoreTextObjectPool = GetComponentInChildren<ScoreTextObjectPool>();
        _fade = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<OVRScreenFade>();
    }

    public void SetModeDecide(bool mode)
    {
        isGameMode = mode;
        if (mode)
        {
            swordTrail = new GameObject[2];
            var g = GameObject.FindGameObjectWithTag("DoubleTrailParticle");
            swordTrail[0] = g.transform.parent.GetChild(1).GetChild(2).gameObject;
            swordTrail[1] = g.transform.parent.GetChild(2).GetChild(2).gameObject;
            DoubleTrailParticle = g.GetComponent<ParticleSystem>();
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

    //ìñÇΩÇ¡ÇΩ
    public void Hit(int type, Vector3 pos, float speed = 0f, OVRInput.Controller controller = OVRInput.Controller.Active)
    {
        int num = 0;
        if (type == 0)
            num = 0;
        else if (type == 3)
            num = 1;
        else
            return;

        StartCoroutine(Vibrate(0.1f, 0.5f, 0.3f, 0, controller));

        if (isCombo[num] && comboNum[num] < 10)
            comboNum[num]++;
        if (isCombo[num] && comboNum[num] == 10 && !reachComboMax[num])
        {
            reachComboMax[num] = true;
            AudioManager.Instance.SE(6);
        }

        if (!_attackGauge.isGaugeMax)
        {
            if (isCombo[num])
                damage = 1.5f + (comboNum[num] * 0.25f);
            else
                damage = 1.5f;
        }
        else
        {
            if (isCombo[num])
                damage = 0.5f + (comboNum[num] * 0.125f);
            else
                damage = 0.5f;
        }
        if (isGameMode)
        {
            if (DoubleTrailParticle.isPlaying && !isPressSwitch)
                damage *= 1.5f;
            if (isPressSwitch)
            {
                isPressSwitch = false;
                damage *= 0.5f;
            }
        }
        
        _scoreTextObjectPool.Active(pos, comboNum[num], num);
        KnockBackDamage += damage;
        if (KnockBackDamage > KnockBackMount)
        {
            _anim.KnockBack(false);
            KnockBackDamage = 0f;
            KnockBackMount *= 2;
        }
        _attackGauge.ChangeGauge(damage);
        if (playCoroutine[num] != null)
        {
            StopCoroutine(playCoroutine[num]);
            playCoroutine[num] = null;
        }
        playCoroutine[num] = StartCoroutine(chainTimer(num));
    }

    WaitForSeconds wait = new WaitForSeconds(3f);
    private IEnumerator chainTimer(int num)
    {
        swordTrail[num].SetActive(true);
        isCombo[num] = true;
        if (isGameMode && swordTrail[0].activeSelf && swordTrail[1].activeSelf)
            DoubleTrailParticle.Play();
        yield return wait;
        swordTrail[num].SetActive(false);
        isCombo[num] = false;
        comboNum[num] = 0;
        reachComboMax[num] = false;
        if (isGameMode)
            DoubleTrailParticle.Stop();
        yield break;
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
            case 2://ëäéËÇÃåïÇ∆é©ï™ÇÃèÇÇ™Ç‘Ç¬Ç©Ç¡ÇΩ(ìÀåÇï∫Ç»ÇÃÇ≈Ç†ÇËìæÇ»Ç¢)
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
