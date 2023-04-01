using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵の状態管理スクリプト
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStatus : MobStatus
{
    private NavMeshAgent _agent;
    Vector3 _linkEndPos;

    private bool SitEnable = true;
    private bool once = false;

    private bool JumpT = false;
    private bool DropT = false;
    private bool onceJ = false;
    private float span = 2f;
    private float passTime = 0f;

    protected override void Start()
    {
        base.Start();

        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // NavMeshAgentのvelocityで移動速度のベクトルが取得できる
        _animator.SetFloat("MoveSpeed", _agent.velocity.magnitude);

        if (_agent.isOnOffMeshLink)                // 「飛び降り」または「ジャンプで飛び越え」へ遷移する
        {
            OffMeshLinkData linkData = _agent.currentOffMeshLinkData;
            _linkEndPos = linkData.endPos;

            if (linkData.linkType == OffMeshLinkType.LinkTypeDropDown)       // 「飛び降り」への遷移
            {
                DropT = true;
                passTime += Time.deltaTime;
                //_agent.speed = 1;

                Debug.Log("Drop Downします");
                return;
            }

            if (linkData.linkType == OffMeshLinkType.LinkTypeJumpAcross && onceJ == false)    // 「ジャンプで飛び越え」への遷移
            {
                JumpT = true;
                onceJ = true;
                //_agent.speed = 2;
                _animator.SetTrigger("Jump");

                Debug.Log("Jumpします");
                return;
            }
        }
        if (DropT == true)
        {
            DropDown();
        }
        if (JumpT == true)
        {
            Jump();
        }
    }

    protected override void OnDie()
    {
        base.OnDie();
        StartCoroutine(DestroyCoroutine());
    }

    public void Clear()
    {
        _animator.SetBool("Clear", true);
        _state = StateEnum.Die;
    }

    public void Jump()
    {
        passTime += Time.deltaTime;
        if (!base.IsSittable) return;
        if (SitEnable == false) return;
        once = true;
        base.GoToSitStateIfPossible();
        if ((transform.position - _linkEndPos).magnitude < 0.1f || passTime > span)       // 終わりを検出し、再び歩く
        {
            Debug.Log("Jump終わり");
            onceJ = false;
            //_animator.SetBool("Jump", false);
            base.GoToNormalStateIfPossible();
            passTime = 0f;
        }
    }

    public void DropDown()
    {
        if (!base.IsSittable) return;
        if (SitEnable == false) return;
        _animator.SetBool("DropDown", true);
        once = true;
        base.GoToSitStateIfPossible();
        if ((transform.position - _linkEndPos).magnitude < 0.1f || passTime > span)       // 終わりを検出し、再び歩く
        {
            _animator.SetBool("DropDown", false);
            base.GoToNormalStateIfPossible();
            passTime = 0f;
        }
    }

    public void Sit()
    {
        if (!base.IsSittable) return;
        if (SitEnable == false) return;
        Debug.Log("座ります");
        _animator.SetBool("Sit", true);
        once = true;
        base.GoToSitStateIfPossible();
    }

    public void StandUp()
    {
        Debug.Log("立ち上がります");
        SitEnable = false;
        if (once == true)
        {
            once = false;
            base.GoToNormalStateIfPossible();
        }
        _animator.SetBool("Sit", false);
        StartCoroutine(SitEnableCoroutine());
    }

    /// <summary>
    /// 倒された時の消滅コルーチンです。
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private IEnumerator SitEnableCoroutine()
    {
        yield return new WaitForSeconds(2);
        SitEnable = true;
        yield break;
    }
}