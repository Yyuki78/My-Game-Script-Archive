using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PieceAnimation : MonoBehaviour
{
    private int BoardSize;
    private Vector3 pos;
    private Transform rotateTransform;
    private Hashtable moveHash;

    private PieceInfomation _info;
    private PieceGrabBehavior _grab;
    private Animator _ani;

    private PieceAnimation _enemyAnime;
    private int DamageDealt = 0;
    
    private DamageTextObjectPool _damageText;

    private bool isLockRotate = false;
    private bool isFinish = false;

    private AudioSource _audio;
    [SerializeField] AudioClip[] clip = new AudioClip[10];

    private void Awake()
    {
        _info = GetComponentInParent<PieceInfomation>();
        if (_info.Role != 5)
            _grab = GetComponentInParent<PieceGrabBehavior>();
        _ani = GetComponent<Animator>();
        _damageText = GameObject.FindGameObjectWithTag("DamageTextObjectPool").GetComponent<DamageTextObjectPool>();
        rotateTransform = transform.parent.gameObject.GetComponent<Transform>();
        _audio = GetComponentInParent<AudioSource>();
    }

    private void Start()
    {
        BoardSize = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>().BoardSize;
        if (_info.Role != 5)
            StartCoroutine(IdleAnimation());
    }

    private IEnumerator IdleAnimation()
    {
        int ran;
        float wait;
        wait = Random.Range(6f, 10f);
        yield return new WaitForSeconds(wait);
        while (true)
        {
            if (_info.isDie) yield break;

            ran = Random.Range(0, 10);
            if (ran < 2)
            {
                _ani.SetInteger("IdleActionNumber", ran);
                _ani.SetTrigger("IdleAction");
            }

            wait = Random.Range(8f, 12f);
            yield return new WaitForSeconds(wait);
        }
    }

    public void Move(Vector2 newPos, bool isAI)
    {
        _info.CurrentPosition = newPos;
        switch (BoardSize)
        {
            case 5:
                pos = new Vector3(4.579f + (newPos.y - 1) * 0.2f, 1.432f, 4.097f + (newPos.x - 1) * 0.2f);
                break;
            case 6:
                pos = new Vector3(4.559f + (newPos.y - 1) * 0.168f, 1.432f, 4.077f + (newPos.x - 1) * 0.168f);
                break;
            case 7:
                pos = new Vector3(4.55f + (newPos.y - 1) * 0.143f, 1.432f, 4.067f + (newPos.x - 1) * 0.143f);
                break;
            default:
                Debug.Log("ボードサイズ設定ミス");
                break;
        }
        if (isAI)
        {
            _info.isAIMoving = true;
            _info.gameObject.transform.DOMove(pos, 1.4f).SetEase(Ease.InOutQuad)
                .OnComplete(() => { _info.isAIMoving = false; });
            int ran = Random.Range(0, 5);
            _audio.volume = 0.2f + Random.Range(-0.05f, 0.05f);
            _audio.pitch = 1.0f;
            _audio.PlayOneShot(clip[ran]);
        }
        else
        {
            _info.gameObject.transform.position = pos;
            _audio.volume = 0.3f + Random.Range(-0.1f, 0.1f);
            _audio.pitch = 1.0f + Random.Range(-0.1f, 0.1f);
            _audio.PlayOneShot(clip[5]);
        }
        if (_info.Role != 5)
            _grab.UpdateReturnPosition(pos);
    }

    public void Attacking(int AttrAdv, int damage, int rotate, PieceAnimation ani)
    {
        _audio.pitch = 1.0f;
        _audio.volume = 0.5f;
        _audio.PlayOneShot(clip[6]);
        PieceInfomation _enemyInfo = ani.gameObject.GetComponentInParent<PieceInfomation>();
        _enemyAnime = ani;
        _enemyInfo.isCanAttacked = false;
        DamageDealt = damage;

        if (_info.Role == 4)
        {
            _ani.SetInteger("AttackDamage", damage);
        }
        else
        {
            switch (AttrAdv)
            {
                case 0:
                    _ani.SetInteger("AttackDamage", 10);
                    break;
                case 1:
                    _ani.SetInteger("AttackDamage", 60);
                    break;
                case 2:
                    _ani.SetInteger("AttackDamage", 100);
                    break;
                default:
                    Debug.Log("属性相性の設定ミス");
                    break;
            }
        }
        int ran = Random.Range(0, 3);
        _ani.SetInteger("AttackVariation", ran);
        _ani.SetTrigger("Attack");

        //攻撃側回転0=上,1=右上,2=右,3=右下,4=下,5=左下,6=左,7=左上
        int plusNum = 0;
        if (_info.Side)
            plusNum = 0;
        else
            plusNum = 180;
        rotateTransform.localEulerAngles = new Vector3(0, 45 * rotate + plusNum, 0);
        StartCoroutine(Looking());
        //Debug.Log("攻撃しました" + damage);
    }

    public void FaceTarget(int rotate)
    {
        if (!_info.isCanAttacked) return;
        if (_info.Role == 5) return;
        int plusNum = 0;
        if (_info.Side)
            plusNum = 0;
        else
            plusNum = 180;
        rotateTransform.localEulerAngles = new Vector3(0, 180 + 45 * rotate + plusNum, 0);
        StartCoroutine(Looking());
    }

    WaitForSeconds wait = new WaitForSeconds(1f);
    private IEnumerator Looking()
    {
        isLockRotate = true;
        yield return wait;
        if (!isFinish)
            isLockRotate = false;
    }

    public void Attacked(int damage)
    {
        _audio.volume = 0.5f + Random.Range(-0.05f, 0.05f);
        _audio.pitch = 1.0f + Random.Range(-0.05f, 0.05f);
        if (damage <= 30)
            _audio.PlayOneShot(clip[7]);
        else if (damage <= 60)
            _audio.PlayOneShot(clip[8]);
        else
            _audio.PlayOneShot(clip[9]);
        _ani.SetTrigger("Defence");
        _ani.SetInteger("DefenceDamage", damage);
        if (_info.HP <= 0)
        {
            _ani.SetBool("Die", true);
            if (_info.Role == 4)
                AudioManager.Instance.SE(26);
            if (_info.Role != 5)
                _grab.PieceDie();
            else
                GetComponentInParent<MysteryBoxManager>().DestroyMysteryBox(true);
        }
        _damageText.Active(_info.CurrentPosition, damage);
        _info.Attacked(damage);
        _info.isCanAttacked = true;
        if (_info.Role == 5)
            GetComponentInParent<MysteryBoxManager>().AttackedBox();
        //Debug.Log("攻撃されました" + damage);
    }

    //ゲーム終了時に回転リセットを呼ばないために使用される
    public void LockRotation()
    {
        isFinish = true;
        isLockRotate = true;
    }

    //key
    public void Hit()
    {
        _enemyAnime.Attacked(DamageDealt);
    }

    //key
    public void ResetRotation()
    {
        if (!isLockRotate)
            rotateTransform.localEulerAngles = Vector3.zero;
    }

    //key
    public void Die()
    {
        if (_info.Role != 4)
            _info.gameObject.SetActive(false);
        else
            GetComponentInParent<TurnManager>().GameFinish();
    }
}
