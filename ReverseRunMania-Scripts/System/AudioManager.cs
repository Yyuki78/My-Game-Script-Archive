using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    //EnemyMove,Rocket(Prefab),ShieldDetectionでもAudioは使っています
    private AudioSource[] _audio;
    [SerializeField] AudioClip[] _bgm;
    [SerializeField] AudioClip[] _se;

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
        StopAllSounds();
        if (SceneManager.GetActiveScene().name == "Title")
        {
            BGM(0);
        }
        if (SceneManager.GetActiveScene().name == "Main")
        {
            SE(1);
        }
    }

    public void BGM(int num)
    {
        _audio[0].clip = _bgm[num];
        _audio[0].Play();
    }

    public void drunkBGM(int num)
    {
        if (num == 0)
            _audio[0].pitch = 0.8f;
        else
            _audio[0].pitch = 1f;
    }

    public void StopAllSounds()
    {
        _audio[0].pitch = 1f;
        for (int i = 0; i < _audio.Length; i++)
        {
            _audio[i].Stop();
        }
    }

    //カウントダウン又はクリック
    public void SE1()
    {
        _audio[1].PlayOneShot(_se[0]);
    }

    public void SE(int num)
    {
        switch (num)
        {
            case 0://コイン獲得
                break;
            case 1://車の走行音
                break;
            case 2://車が曲がる音
                if(!_audio[num + 1].isPlaying)
                {
                    _audio[num + 1].Play();
                }
                return;
            case 3://アイテムガチャ音
                break;
            case 4://アイテム使用音
                break;
            case 5://アースシャッター発動音
                break;
            case 6://お化けになる音
                break;
            case 7://壁時間回復音
                break;
            case 8://鎧装着音
                break;
            case 9://コイン連続獲得音
                break;
            case 10://いかずち音
                break;
            case 11://波動音
                break;
            case 12://全アイテム使用時の特別音
                break;
            case 13://墨汁ヒット音
                break;
            case 14://壁時間減少音
                break;
            case 15://滑りやすい時の音
                if (!_audio[num + 1].isPlaying)
                {
                    _audio[num + 1].Play();
                }
                return;
            case 16://沼踏んだ時の音
                break;
            case 17://メーターがない時の警告音
                break;
            case 18://車との接触音
                break;
            case 19://カウントダウン
                break;
            case 20://クリック
                break;
            case 21://レーザー
                break;
            case 22://鎧にぶつかった
                break;
        }
        _audio[num + 1].PlayOneShot(_se[num]);
    }

    public void StopSE(int num)
    {
        _audio[num + 1].Stop();
    }
}
