using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponSpeed : MonoBehaviour
{
    [SerializeField] bool mode = false;//falseÇÕtranform,trueÇÕrigidbody
    [SerializeField] bool isAudio = false;//êÊí[ÇÃÇ›âπÇèoÇ∑
    private bool isStart = false;

    private Transform _transform;
    private Vector3 currentPosition;
    private Vector3 currentVelocity;
    private float currentSpeed = 0f;
    private Vector3 _prevPosition;

    private Rigidbody _rigidbody;

    [SerializeField] float particleSpeed = 3f;

    private int flame = 0;
    private float timeThreshold;
    private float timeWindow = 0.5f;//ï€ë∂Ç∑ÇÈéûä‘ÇÃí∑Ç≥
    private Dictionary<float, float> speedHistory = new Dictionary<float, float>();//âﬂãéÇÃóöó

    private ParticleSystem _particle;
    private AudioSource _audio;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _particle = GetComponentInChildren<ParticleSystem>();
        _prevPosition = _transform.position;
        _particle.Stop();
        if (mode)
            _rigidbody = GetComponent<Rigidbody>();
        if (isAudio)
            _audio = GetComponent<AudioSource>();
    }

    public void GameStart()
    {
        _prevPosition = _transform.position;
        isStart = true;
    }

    private void Update()
    {
        if (!isStart) return;
        
        if (GetHighestSpeed() > particleSpeed)
        {
            _particle.Play();
            if (isAudio)
                _audio.PlayOneShot(_audio.clip);
        }

        flame++;
        if (flame % 3 != 0) return;

        if (mode)
        {
            currentSpeed = _rigidbody.velocity.magnitude;
        }
        else
        {
            currentVelocity = (_transform.position - _prevPosition) / Time.deltaTime;
            currentSpeed = currentVelocity.magnitude;
            _prevPosition = _transform.position;
        }

        timeThreshold = Time.time - timeWindow;
        speedHistory.Add(Time.time, currentSpeed);
        for (int i = 0; i < speedHistory.Count; i++)
        {
            KeyValuePair<float, float> pair = speedHistory.ElementAt(i);
            if (pair.Key < timeThreshold)
                speedHistory.Remove(pair.Key);
        }
    }

    public float GetHighestSpeed()
    {
        float highestSpeed = float.MinValue;

        for (int i = 0; i < speedHistory.Count; i++)
        {
            KeyValuePair<float, float> pair = speedHistory.ElementAt(i);
            if (pair.Value > highestSpeed)
                highestSpeed = pair.Value;
        }

        return highestSpeed;
    }
}
