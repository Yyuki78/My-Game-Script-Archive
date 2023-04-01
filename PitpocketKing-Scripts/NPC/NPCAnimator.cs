using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAnimator : MonoBehaviour
{
    public enum NPCState
    {
        idle,
        walk,
        run,
        apologizing,
        drinking,
        stopping,
        sitting
    }
    private NPCState currentState;

    public bool isCanHit => currentState == NPCState.walk || currentState == NPCState.run;
    public bool isApologize => currentState == NPCState.apologizing;

    private Animator _ani;
    private NavMeshAgent _agent;
    private AudioSource _audio;
    [SerializeField] AudioClip _clip1;
    [SerializeField] AudioClip _clip2;
    [SerializeField] AudioClip _clip3;

    // Start is called before the first frame update
    void Start()
    {
        _ani = GetComponentInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //NavMeshAgentが停止中ならreturn
        if (!_agent.isActiveAndEnabled)
        {
            _agent.speed = 0;
            _ani.SetFloat("MoveSpeed", 0);
            return;
        }

        _ani.SetFloat("MoveSpeed", _agent.velocity.magnitude);
        if (_agent.velocity.magnitude > 3f)
        {
            ChangeState(NPCState.run);
        }
        else if (_agent.velocity.magnitude > 0.01f)
        {
            ChangeState(NPCState.walk);
        }
        else
        {
            ChangeState(NPCState.idle);
        }
    }

    public void ChangeState(NPCState _state)
    {
        if (_state == currentState) return;
        currentState = _state;
        switch (_state)
        {
            case NPCState.idle:
                break;
            case NPCState.walk:
                break;
            case NPCState.run:
                break;
            case NPCState.sitting:
                break;
            case NPCState.drinking:
                break;
            case NPCState.stopping:
                break;
            case NPCState.apologizing:
                break;
            default:
                Debug.Log("State変更ミス");
                break;
        }
    }

    public IEnumerator Sitting(Vector3 pos, Vector3 rotate)
    {
        ChangeState(NPCState.sitting);
        _ani.SetBool("Sitting", true);
        //停止
        _agent.enabled = false;

        for (int i = 0; i < 10; i++)
        {
            transform.localPosition = pos;
            transform.localEulerAngles = rotate;

            yield return new WaitForSeconds(0.01f);
        }

        _audio.PlayOneShot(_clip1);

        yield return new WaitForSeconds(4f);
        _ani.SetBool("Sitting", false);
        _agent.enabled = true;

        yield break;
    }

    public IEnumerator Drinking(Vector3 pos, Vector3 rotate)
    {
        ChangeState(NPCState.drinking);
        _ani.SetBool("Drinking", true);
        //停止
        _agent.enabled = false;

        for (int i = 0; i < 10; i++)
        {
            transform.localPosition = pos;
            transform.localEulerAngles = rotate;

            yield return new WaitForSeconds(0.01f);
        }

        _audio.PlayOneShot(_clip2);
        yield return new WaitForSeconds(6f);

        _ani.SetBool("Drinking", false);
        _agent.enabled = true;

        yield break;
    }

    public IEnumerator Stopping(Vector3 pos, Vector3 rotate)
    {
        ChangeState(NPCState.stopping);
        _ani.SetBool("Stopping", true);
        //停止
        _agent.enabled = false;

        for (int i = 0; i < 10; i++)
        {
            transform.localPosition = pos;
            transform.localEulerAngles = rotate;

            yield return new WaitForSeconds(0.01f);
        }
        _audio.PlayOneShot(_clip3);
        yield return new WaitForSeconds(6f);

        _ani.SetBool("Stopping", false);
        _agent.enabled = true;

        yield break;
    }

    public IEnumerator Apoloziging()
    {
        _agent.ResetPath();
        //停止
        _agent.enabled = false;

        _ani.Play("change state", 0, 0);
        yield return new WaitForSeconds(0.25f);

        ChangeState(NPCState.apologizing);
        _ani.SetBool("Apologizing", true);

        yield return new WaitForSeconds(3.5f);

        _ani.SetBool("Apologizing", false);
        _agent.enabled = true;

        yield return new WaitForSeconds(0.75f);
        var c = GetComponent<NPCController>();
        c.SetDes();

        yield break;
    }
}
