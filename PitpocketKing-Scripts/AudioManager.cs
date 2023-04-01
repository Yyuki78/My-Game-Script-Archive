using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource walkSE;
    [SerializeField] private AudioSource se;
    [SerializeField] private AudioSource hitse;
    [SerializeField] private AudioSource longse;
    [SerializeField] private AudioSource combose;

    [SerializeField] private AudioClip bgm1;//タイトル
    [SerializeField] private AudioClip bgm2;//ゲーム中
    [SerializeField] private AudioClip bgm3;//リザルト

    [SerializeField] private AudioClip se1;//歩いている音
    [SerializeField] private AudioClip se2;//早歩きしている音
    [SerializeField] private AudioClip se3;//走っている音
    [SerializeField] private AudioClip se4;//何かとぶつかる音1
    [SerializeField] private AudioClip se5;//何かとぶつかる音2
    [SerializeField] private AudioClip se6;//すりをする音1
    [SerializeField] private AudioClip se7;//すりをする音2
    [SerializeField] private AudioClip se8;//お金のゲット音
    [SerializeField] private AudioClip se9;//コンボゲージが伸びる時の音
    [SerializeField] private AudioClip se10;//連続ヒット時の音
    [SerializeField] private AudioClip se11;//コンボアップ時の音
    [SerializeField] private AudioClip se12;//アイテム取得時の音
    [SerializeField] private AudioClip se13;//カウントダウン
    [SerializeField] private AudioClip se14;//お金を取られた時の音
    [SerializeField] private AudioClip se15;//捕まった時の音1
    [SerializeField] private AudioClip se16;//獲得金額上昇中の音
    [SerializeField] private AudioClip se17;//お金が増えていく音
    [SerializeField] private AudioClip se18;//リザルト出現音
    [SerializeField] private AudioClip se19;//クリック音
    [SerializeField] private AudioClip se20;//お金が減っていく音

    private void Start()
    {

    }

    //DebugGameMangerで使用
    public void BGM1()
    {
        bgm.clip = bgm1;
        bgm.volume = 0.075f;
        bgm.Play();
    }

    //CountdownText
    public void BGM2()
    {
        bgm.clip = bgm2;
        bgm.volume = 0.075f;
        bgm.Play();
    }

    //GameManger
    public void BGM3()
    {
        bgm.clip = bgm3;
        bgm.volume = 0.2f;
        bgm.Play();
    }

    public void ChangeBGMpitch(float pitch) //ComboGauge
    {
        bgm.pitch = pitch;
    }

    //全てのBGMを止める GameManger
    public void StopBGM()
    {
        bgm.pitch = 1.0f;
        bgm.Stop();
    }

    //歩いている音 PlayerMove
    public void SE1()
    {
        walkSE.clip = se1;
        walkSE.volume = 0.1f;
        if (!walkSE.isPlaying) walkSE.Play();
    }

    //早歩きしている音　PlayerMove
    public void SE2()
    {
        walkSE.clip = se2;
        walkSE.volume = 0.1f;
        if (!walkSE.isPlaying) walkSE.Play();
    }

    //走っている音　PlayerMove
    public void SE3()
    {
        walkSE.clip = se3;
        walkSE.volume = 0.1f;
        if (!walkSE.isPlaying) walkSE.Play();
    }

    //WalkSEを止める PlayerMove
    public void StopWalkSE()
    {
        walkSE.volume = 0f;
        walkSE.Stop();
    }

    //何かとぶつかる音1 NPCCollider
    public void SE4()
    {
        se.volume = 0.15f;
        se.PlayOneShot(se4);
    }

    //何かとぶつかる音2 NPCCollider
    public void SE5()
    {
        se.volume = 0.15f;
        se.PlayOneShot(se5);
    }

    //すりをする音1 PlayerMove
    public void SE6()
    {
        hitse.volume = 0.25f;
        hitse.PlayOneShot(se6);
    }

    //すりをする音2 PlayerMove
    public void SE7()
    {
        hitse.volume = 0.25f;
        hitse.PlayOneShot(se7);
    }

    //お金のゲット音 PlayerMove
    public void SE8()
    {
        se.volume = 0.2f;
        se.PlayOneShot(se8);
    }

    //コンボゲージが伸びる時の音 ComboGauge
    public void SE9()
    {
        combose.volume = 0.1f;
        combose.PlayOneShot(se9);
    }

    //コンボゲージが伸びきった時に止める用 ComboGauge
    public void StopSE9()
    {
        combose.Stop();
    }

    //連続ヒット時の音 PlayerMove
    public void SE10()
    {
        se.volume = 0.2f;
        se.PlayOneShot(se10);
    }

    //コンボアップ時の音 PlayerMove
    public void SE11()
    {
        se.volume = 0.1f;
        se.PlayOneShot(se11);
    }

    //アイテム取得時の音 GameManager
    public void SE12()
    {
        se.volume = 0.03f;
        se.PlayOneShot(se12);
    }

    //ゲーム開始時のカウントダウン音 CountdownText
    public void SE13()
    {
        se.volume = 0.075f;
        se.PlayOneShot(se13);
    }

    //お金を取られた時の音 PlayerMove
    public void SE14()
    {
        se.volume = 0.15f;
        se.PlayOneShot(se14);
    }

    //捕まった時の音1 CameraController
    public void SE15()
    {
        se.volume = 0.2f;
        se.PlayOneShot(se15);
    }

    //獲得金額上昇中の音 CameraController
    public void SE16()
    {
        se.volume = 0.25f;
        se.PlayOneShot(se16);
    }

    //お金が増えていく音 GetMoneyText
    public void SE17()
    {
        longse.volume = 0.01f;
        longse.PlayOneShot(se17);
    }

    //リザルト出現音 ResultManager
    public void SE18()
    {
        se.volume = 0.5f;
        se.PlayOneShot(se18);
    }

    //クリック音 ButtonAnimation
    public void SE19()
    {
        se.volume = 0.2f;
        se.PlayOneShot(se19);
    }

    //お金が減っていく音 GetMoneyText
    public void SE20()
    {
        se.volume = 0.2f;
        se.PlayOneShot(se20);
    }

    //SEのリセット
    public void ResetSE()
    {
        se.Stop();
        hitse.Stop();
        longse.Stop();
        combose.Stop();
        se.pitch = 1f;
        se.volume = 0.1f;
        StopAllCoroutines();
    }
}
