using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : MonoBehaviour
{
    public bool isMagicMode = false;

    public bool CanAttack = true;
    public bool isAttacking = false;
    public bool AttackState { private set; get; } = false;

    //攻撃側に受け渡す用
    public GameObject[] EnemyWeapon;
    public AttackGauge attackGauge;

    private int role = 0;
    private bool firstMove = true;
    private bool isInterval = false;
    private Animator _ani;

    void Awake()
    {
        _ani = GetComponent<Animator>();
    }

    public void StartAnimation(int num)
    {
        role = num;
        if (num != 2)
        {
            _ani.SetTrigger("FirstAction");
            _ani.SetBool("isStart", true);
        }
        else
        {
            _ani.SetBool("isMagic", true);
            int ran = Random.Range(0, 2);
            _ani.SetInteger("IdleActionNumber", ran);
            _ani.SetBool("isStart", true);
        }
    }

    public void MoveAnimation(bool type)
    {
        if (AttackState) return;
        if (firstMove && !isMagicMode)
        {
            firstMove = false;
            return;
        }
            
        if (type)
        {
            _ani.SetBool("Move", true);
            _ani.SetTrigger("MoveAction");
        }
        else
            _ani.SetBool("Move", false);
    }

    public void RotateAnimation(bool type)
    {
        if (isAttacking || isInterval) return;
        if (type)
            _ani.SetInteger("AngleNumber", 0);
        else
            _ani.SetInteger("AngleNumber", 1);
        _ani.SetTrigger("RotateAction");
        StartCoroutine(RotateInterval());
    }

    WaitForSeconds wait= new WaitForSeconds(1f);
    private IEnumerator RotateInterval()
    {
        isInterval = true;
        yield return wait;
        isInterval = false;
    }

    public void KnockBack(bool type = false)
    {
        if (isAttacking)
            isAttacking = false;
        _ani.SetTrigger("KnockBack");
        if (!type)
        {
            int ran = Random.Range(0, 2);
            _ani.SetInteger("KnockBackVariation", ran);
        }
        else
        {
            _ani.SetInteger("KnockBackVariation", 2);
        }
    }

    public void Finish()
    {
        _ani.SetBool("isStart", false);
    }

    //一時停止・再開(HourGlassでのみ使用)
    public void TogglePlayback(bool type)
    {
        if (type)
        {
            _ani.speed = 0f;
        }
        else
        {
            _ani.speed = 1f;
        }
    }

    //様々な変数の設定 key
    public void ResetRotation()
    {
        CanAttack = true;
        AttackState = false;
        int ran = Random.Range(0, 5);
        _ani.SetInteger("IdleActionNumber", ran);
        if (_ani.GetBool("Blocking"))
            _ani.SetBool("Blocking", false);
        if (isAttacking)
            isAttacking = false;
    }

    //攻撃開始　key
    public void StartAttack()
    {
        AttackState = true;
        int ran = Random.Range(1, 6);
        _ani.SetInteger("AttackVariation", ran);
        StartCoroutine(attack());
        if(_ani.GetBool("Blocking"))
            _ani.SetBool("Blocking", false);
    }

    //突撃兵専用の攻撃開始　key
    public void StartAttack2()
    {
        AttackState = true;
        int ran = Random.Range(1, 13);
        _ani.SetInteger("AttackVariation", ran);
        StartCoroutine(attack2());
    }

    //何もしない(魔法時のみ意味有) key
    public void Hit()
    {
        if (!isMagicMode) return;
        GetComponentInParent<IAttack>().HitEnemyAttack();
    }

    //何もしない key
    public void Die()
    {

    }

    //防御中 key
    public void Blocking()
    {
        if (role == 1)
            _ani.SetBool("Blocking", true);
    }

    private IEnumerator attack()
    {
        yield return new WaitForSeconds(0.75f);
        isAttacking = true;
        yield return new WaitForSeconds(1.75f);
        isAttacking = false;
        yield break;
    }

    private IEnumerator attack2()
    {
        yield return new WaitForSeconds(0.25f);
        isAttacking = true;
        yield break;
    }
}
