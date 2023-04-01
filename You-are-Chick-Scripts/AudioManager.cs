using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private AudioSource[] _audio;
    [SerializeField] AudioClip[] _bgm;
    [SerializeField] AudioClip[] _se;
    [SerializeField] AudioClip[] _guitarSE;

    private float hibiNum = 0;

    static public AudioManager instance;
    void Awake()
    {
        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {

            Destroy(gameObject);
        }
        _audio = GetComponents<AudioSource>();
    }

    private void Start()
    {
        hibiNum = 0;
        StopAllSounds();
        if (SceneManager.GetActiveScene().name == "Title")
        {
            BGM(0);
        }else if (SceneManager.GetActiveScene().name == "Home")
        {
            BGM(1);
        }
        else
        {
            BGM(2);
        }
    }

    public void Init()
    {
        hibiNum = 0;
        StopAllSounds();
        if (SceneManager.GetActiveScene().name == "Title")
        {
            BGM(0);
        }
        else if (SceneManager.GetActiveScene().name == "Home")
        {
            BGM(1);
        }
        else
        {
            BGM(2);
        }
    }

    public void BGM(int num)
    {
        _audio[0].clip = _bgm[num];
        _audio[0].Play();
        switch (num)
        {
            case 0:
                _audio[0].volume = 0.2f;
                break;
            case 1:
                _audio[0].volume = 0.05f;
                break;
            case 2:
                _audio[0].volume = 0.1f;
                break;
            case 3:
                _audio[0].volume = 0.075f;
                break;
            case 4:
                break;
        }
    }

    public void StopAllSounds()
    {
        _audio[0].pitch = 1f;
        for (int i = 0; i < _audio.Length; i++)
        {
            _audio[i].Stop();
        }
    }
    
    public void guitarSE()
    {
        int ran = Random.Range(0,6);
        _audio[17].PlayOneShot(_guitarSE[ran]);
    }

    public void SE(int num)
    {
        switch (num)
        {
            case 0://�q���g�o����
                break;
            case 1://�����������鉹
                break;
            case 2://�Q�[�g���J����
                break;
            case 3://�ԈႦ����
                break;
            case 4://�D���𓥂񂾉�
                break;
            case 5://�S�[����
                break;
            case 6://���o�[��������
                break;
            case 7://���̐U�����鉹
                break;
            case 8://�Ђт����鉹
                hibiNum++;
                _audio[num + 1].volume = 0.1f + hibiNum / 20f;
                break;
            case 9://�Q�[�g�����鉹
                break;
            case 10://���Ԃ̉�鉹
                break;
            case 11://�ق����̑|����
                break;
            case 12://�W�����v��
                break;
            case 13://�N���b�N��
                break;
            case 14://������
                break;
            case 15://�{�[���̃L�b�N��
                break;
        }
        _audio[num + 1].PlayOneShot(_se[num]);
    }

    public void StopSE(int num)
    {
        _audio[num + 1].Stop();
    }
}