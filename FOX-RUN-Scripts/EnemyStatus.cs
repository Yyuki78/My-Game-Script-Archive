using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// �G�̏�ԊǗ��X�N���v�g
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
        // NavMeshAgent��velocity�ňړ����x�̃x�N�g�����擾�ł���
        _animator.SetFloat("MoveSpeed", _agent.velocity.magnitude);

        if (_agent.isOnOffMeshLink)                // �u��э~��v�܂��́u�W�����v�Ŕ�щz���v�֑J�ڂ���
        {
            OffMeshLinkData linkData = _agent.currentOffMeshLinkData;
            _linkEndPos = linkData.endPos;

            if (linkData.linkType == OffMeshLinkType.LinkTypeDropDown)       // �u��э~��v�ւ̑J��
            {
                DropT = true;
                passTime += Time.deltaTime;
                //_agent.speed = 1;

                Debug.Log("Drop Down���܂�");
                return;
            }

            if (linkData.linkType == OffMeshLinkType.LinkTypeJumpAcross && onceJ == false)    // �u�W�����v�Ŕ�щz���v�ւ̑J��
            {
                JumpT = true;
                onceJ = true;
                //_agent.speed = 2;
                _animator.SetTrigger("Jump");

                Debug.Log("Jump���܂�");
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
        if ((transform.position - _linkEndPos).magnitude < 0.1f || passTime > span)       // �I�������o���A�Ăѕ���
        {
            Debug.Log("Jump�I���");
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
        if ((transform.position - _linkEndPos).magnitude < 0.1f || passTime > span)       // �I�������o���A�Ăѕ���
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
        Debug.Log("����܂�");
        _animator.SetBool("Sit", true);
        once = true;
        base.GoToSitStateIfPossible();
    }

    public void StandUp()
    {
        Debug.Log("�����オ��܂�");
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
    /// �|���ꂽ���̏��ŃR���[�`���ł��B
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