using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCCollider : MonoBehaviour
{
    private NavMeshAgent _agent;
    private NPCAnimator _ani;
    private GameObject Player;

    [SerializeField] GameObject RotateCube;
    public bool alreadyHit = false;

    private bool isHit = false;

    private AudioManager _audio;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _ani = GetComponent<NPCAnimator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        _audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (isHit)
        {
            Vector3 relativePoint = Player.transform.position;
            relativePoint.y = transform.position.y;
            transform.LookAt(relativePoint);
        }

        if (alreadyHit) return;
        if (!_ani.isCanHit)
        {
            RotateCube.SetActive(false);
        }
        else
        {
            RotateCube.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 12) return;
        if (!_ani.isCanHit) return;
        if (alreadyHit) return;
        var m = other.gameObject.GetComponent<PlayerMove>();
        if (!m.isCanHit) return;
        alreadyHit = true;
        
        int ran = Random.Range(0, 2);
        if (ran == 0)
            _audio.SE4();
        else
            _audio.SE5();

        RotateCube.SetActive(false);
        m.StartApologize(gameObject);
        StartCoroutine(stopLookAt());
        StartCoroutine(_ani.Apoloziging());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 12) return;
        if (!_ani.isCanHit) return;
        if (alreadyHit) return;
        var m = other.gameObject.GetComponent<PlayerMove>();
        if (!m.isCanHit) return;
        alreadyHit = true;

        int ran = Random.Range(0, 2);
        if (ran == 0)
            _audio.SE4();
        else
            _audio.SE5();

        RotateCube.SetActive(false);
        m.StartApologize(gameObject);
        StartCoroutine(stopLookAt());
        StartCoroutine(_ani.Apoloziging());
    }

    private IEnumerator stopLookAt()
    {
        var info = GetComponent<NPCInformation>();
        if (info.role == 5)
        {
            var audio = GetComponents<AudioSource>();
            for(int i = 0; i < audio.Length; i++)
                audio[i].Stop();
        }

        isHit = true;
        yield return new WaitForSeconds(3f);
        isHit = false;
        yield break;
    }
}
