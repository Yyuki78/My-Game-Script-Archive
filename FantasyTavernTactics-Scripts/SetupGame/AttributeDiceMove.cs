using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeDiceMove : MonoBehaviour
{
    public bool isDiceStopping = false;
    public int AttributeResult = -1;//0は赤,1は青,2は緑
    public bool isFinish { private set; get; } = false;

    private bool isStart = false;
    private float moveSpeed = 5f;
    private bool isMove = false;
    private Vector3 DestinationPos;
    private Quaternion saveRotation;
    private bool once = true;
    private bool canSound = true;

    //各面の場所
    [SerializeField]
    private Transform[] _diceSpots;

    [SerializeField] AttributeDiceMug _attributeDiceMug;
    private Rigidbody _rigid;
    private Transform _transform;
    private AudioSource _audio;

    [SerializeField] AudioClip[] clip = new AudioClip[4];

    void Start()
    {
        _transform = GetComponent<Transform>();
        _rigid = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
        _rigid.maxDepenetrationVelocity = 2f;
        _rigid.isKinematic = false;
        Invoke(nameof(KinematicInMug), 0.1f);
    }
    
    void Update()
    {
        if (isMove)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, DestinationPos, moveSpeed * Time.deltaTime);
            _transform.rotation = saveRotation;
            if (Vector3.Distance(_transform.position, DestinationPos) < 0.01f)
            {
                _transform.position = DestinationPos;
                gameObject.layer = 8;
                isMove = false;
                //_rigid.isKinematic = false;
                isFinish = true;
            }
        }
        else
        {
            if (isFinish) return;
            if (isStart && _rigid.IsSleeping() && !_rigid.isKinematic && once)
            {
                once = false;
                CheckSpot();
            }
            //マグで動かしたりした時に属性をもう一度決め直せる用
            if (isStart && !_rigid.IsSleeping() && !_rigid.isKinematic && !once)
                once = true;
        }

        if (!_attributeDiceMug.isAllDiceOut) return;
        if (_rigid.IsSleeping() && !isDiceStopping)
        {
            isDiceStopping = true;
        }
    }

    private void KinematicInMug()
    {
        _rigid.isKinematic = true;
        isStart = true;
    }

    public void StartMove()
    {
        StartCoroutine(UnlockKinematic());
    }

    WaitForSeconds wait = new WaitForSeconds(0.1f);
    private IEnumerator UnlockKinematic()
    {
        yield return wait;
        _rigid.isKinematic = false;
        yield return null;
        transform.parent = null;
        yield break;
    }

    public void goTable(Vector3 pos)
    {
        DestinationPos = pos;
        saveRotation = _transform.rotation;
        StartCoroutine(HeadtoTable());
    }

    private IEnumerator HeadtoTable()
    {
        _rigid.isKinematic = true;
        _transform.parent = null;
        yield return new WaitForSeconds(1.75f);
        isMove = true;
        yield break;
    }

    private void CheckSpot()
    {
        var topIndex = 0;
        var topValue = _diceSpots[0].transform.position.y;
        for (var i = 1; i < _diceSpots.Length; ++i)
        {
            if (_diceSpots[i].transform.position.y < topValue)
                continue;
            topValue = _diceSpots[i].transform.position.y;
            topIndex = i;
        }
        //Debug.Log(topIndex + 1);
        if (topIndex == 0 || topIndex == 5)
        {
            AttributeResult = 0;
        }
        else if (topIndex == 1 || topIndex == 4)
        {
            AttributeResult = 1;
        }
        else
        {
            AttributeResult = 2;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isStart || !canSound || _rigid.isKinematic) return;
        canSound = false;
        StartCoroutine(CoolTime());
        _audio.pitch = 1.0f + Random.Range(-0.1f, 0.1f);
        if (collision.gameObject.tag == "AttributeDice")
        {
            _audio.volume = Mathf.Min(_rigid.velocity.magnitude / 5f, 0.15f);
            int ran = Random.Range(0, 2);
            _audio.PlayOneShot(clip[ran]);
        }
        else if (collision.gameObject.tag == "DiceMug")
        {
            if (_rigid.velocity.magnitude > 0.1f) return;
            _audio.volume = 0.2f;
            _audio.PlayOneShot(clip[2]);
        }
        else
        {
            _audio.volume = Mathf.Min(_rigid.velocity.magnitude / 5f, 0.25f);
            _audio.PlayOneShot(clip[3]);
        }
    }

    WaitForSeconds wait2 = new WaitForSeconds(0.2f);
    private IEnumerator CoolTime()
    {
        yield return wait2;
        canSound = true;
    }
}
