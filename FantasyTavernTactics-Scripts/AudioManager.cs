using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource[] _audio;
    [SerializeField] AudioClip[] _bgm;
    [SerializeField] AudioClip[] _se;

    private GameInfomation _gameInfomation;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        _audio = GetComponentsInChildren<AudioSource>();
    }

    void Start()
    {
        _gameInfomation = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>();
    }

    public void SetBGM(int num, bool type = false)
    {
        if (num != 7 && num != 8)
        {
            _audio[0].clip = _bgm[num];
            _audio[0].Play();
        }
        switch (num)
        {
            case 0://�^�C�g��(�X�e�[�W�Z���N�g)
                _audio[0].volume = 0.8f;
                break;
            case 1://�`���[�g���A��
                _audio[0].volume = 0.5f;
                break;
            case 2://�Q�[������
                _audio[0].volume = 0.01f;
                break;
            case 3://�퓬�V�[���ҋ@
                _audio[0].volume = 0.2f;
                break;
            case 4://�퓬�V�[�����U���g
                _audio[0].volume = 0.065f;
                break;
            case 5://�Q�[�����U���g(����)
                _audio[0].volume = 0.15f;
                break;
            case 6://�Q�[�����U���g(�s�k)
                _audio[0].volume = 0.075f;
                _audio[0].loop = false;
                break;
            case 7://�Q�[����(6���)
                _audio[0].loop = true;
                _audio[0].volume = 0.25f;
                _audio[0].clip = _bgm[7 + _gameInfomation.isGameBGM];
                _audio[0].Play();
                break;
            case 8://�퓬�V�[����(3���)
                _audio[0].loop = false;
                switch (_gameInfomation.isBattleBGM)
                {
                    case 0:
                        _audio[0].volume = 0.15f;
                        break;
                    case 1:
                        _audio[0].volume = 0.225f;
                        break;
                    case 2:
                        _audio[0].volume = 0.25f;
                        break;
                }
                if (type)
                    _audio[0].volume /= 2f;
                _audio[0].clip = _bgm[13 + _gameInfomation.isBattleBGM];
                _audio[0].Play();
                break;
        }
    }

    public void StopBGM(int type)
    {
        switch (type)
        {
            case 0://�~�߂�
                _audio[0].Stop();
                break;
            case 1://�t�F�[�h�A�E�g
                StartCoroutine(fadeOutBGM());
                break;
        }
    }

    //���݂�volume���擾���ăt�F�[�h�A�E�g������
    WaitForSeconds wait = new WaitForSeconds(0.1f);
    private IEnumerator fadeOutBGM()
    {
        float volume = _audio[0].volume;
        for(int i = 0; i < 10; i++)
        {
            _audio[0].volume = volume - (volume * i / 10f);
            yield return wait;
        }
        _audio[0].volume = 0f;
        yield break;
    }

    public void TogglePlaybackBGM(bool type)
    {
        if(type)
            _audio[0].Play();
        else
            _audio[0].Pause();
    }

    public void StopAllSounds()
    {
        _audio[0].pitch = 1f;
        for (int i = 0; i < _audio.Length; i++)
        {
            _audio[i].Stop();
        }
    }

    public void SE(int num)
    {
        switch (num)
        {
            case 0://�{�^���I����(�f�t�H���g)
                break;
            case 1://�{�^���I����(�L�����Z��)
                break;
            case 2://�{�^���I����(���̖ؖڃ{�^��)
                break;
            case 3://�{�^���I����(�Q�[���X�^�[�g�{�^��)
                break;
            case 4://�Q�[�W100���𒴂������̉�
                break;
            case 5://����̍U�����󂯂����̉�
                break;
            case 6://�R���{�ő厞�̃W���O��
                break;
            case 7://���@�w�������������̉�
                break;
            case 8://�Ō�̖��@�U���̉�(���������e�܂Ł����e+����)
                break;
            case 9://�O�Ղ̃q�b�g��
                break;
            case 10://�Z�b�g�A�b�v�̏I����
                break;
            case 11://�^�[���J�n��
                break;
            case 12://�^�[���I����
                break;
            case 13://�Q�[���I�����̌��ʉ�(���C�g�A�b�v)
                break;
            case 14://�����C�x���g������
                break;
            case 15://HalfWayBoost������
                break;
            case 16://����^�C��������
                break;
            case 17://WaitAttackEnd�o����
                break;
            case 18://�퓬�V�[���ւ̈ڍs��
                break;
            case 19://�퓬�V�[���̊J�n��
                break;
            case 20://�葱���閂�@�w�̗��߉�()
                break;
            case 21://�����~�܂��Ă��鎞�̉�
                break;
            case 22://���������o����
                break;
            case 23://�U����e����
                break;
            case 24://�`���[�g���A���̒B����
                break;
            case 25://�^�C�g���̃^�C�v��
                break;
            case 26://�Q�[���I�����̍U����
                break;
            case 27://�t�@�C�^�[�I��or�U���t�F�C�Y�Ŋm�F���b�Z�[�W���o�����̉�
                break;
        }
        _audio[num + 1].PlayOneShot(_se[num]);
    }

    public void StopSE(int num)
    {
        _audio[num + 1].Stop();
    }

    public void MagicCircleSound(int num)
    {
        _audio[21].volume = 0.075f * Mathf.Min(num, 9);
        if (num == 1)
            _audio[21].Play();
    }
}
