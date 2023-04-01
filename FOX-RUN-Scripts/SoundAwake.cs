using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAwake : MonoBehaviour
{
    //����
    public AudioClip sound1;
    public AudioClip sound2;
    public AudioClip sound3;
    public AudioClip sound4;
    public AudioClip sound5;
    AudioSource _audioSource;

    //�����o�͂���x�����s��Ȃ����߂�bool
    private bool once = false;
    private bool once2 = false;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(CooldownCoroutine());
            once = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (once == true)
            {
                once = false;
                StartCoroutine(CooldownCoroutine());
            }
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(1f);
        if (once == true) yield break;
        int rnd = Random.Range(1, 6);
        switch (rnd)
        {
            case 1:
                Debug.Log("����1");
                _audioSource.PlayOneShot(sound1);
                break;
            case 2:
                Debug.Log("����2");
                _audioSource.volume = 0.6f;
                _audioSource.PlayOneShot(sound2);
                _audioSource.volume = 0.8f;
                break;
            case 3:
                Debug.Log("����3");
                _audioSource.PlayOneShot(sound3);
                break;
            case 4:
                Debug.Log("����4");
                _audioSource.PlayOneShot(sound4);
                break;
            case 5:
                Debug.Log("����5");
                _audioSource.PlayOneShot(sound5);
                break;
        }
        
    }
} 
