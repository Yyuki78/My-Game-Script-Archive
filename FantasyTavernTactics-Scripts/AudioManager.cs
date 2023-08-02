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
            case 0://タイトル(ステージセレクト)
                _audio[0].volume = 0.8f;
                break;
            case 1://チュートリアル
                _audio[0].volume = 0.5f;
                break;
            case 2://ゲーム準備
                _audio[0].volume = 0.01f;
                break;
            case 3://戦闘シーン待機
                _audio[0].volume = 0.2f;
                break;
            case 4://戦闘シーンリザルト
                _audio[0].volume = 0.065f;
                break;
            case 5://ゲームリザルト(勝利)
                _audio[0].volume = 0.15f;
                break;
            case 6://ゲームリザルト(敗北)
                _audio[0].volume = 0.075f;
                _audio[0].loop = false;
                break;
            case 7://ゲーム中(6種類)
                _audio[0].loop = true;
                _audio[0].volume = 0.25f;
                _audio[0].clip = _bgm[7 + _gameInfomation.isGameBGM];
                _audio[0].Play();
                break;
            case 8://戦闘シーン中(3種類)
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
            case 0://止める
                _audio[0].Stop();
                break;
            case 1://フェードアウト
                StartCoroutine(fadeOutBGM());
                break;
        }
    }

    //現在のvolumeを取得してフェードアウトさせる
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
            case 0://ボタン選択音(デフォルト)
                break;
            case 1://ボタン選択音(キャンセル)
                break;
            case 2://ボタン選択音(立体木目ボタン)
                break;
            case 3://ボタン選択音(ゲームスタートボタン)
                break;
            case 4://ゲージ100％を超えた時の音
                break;
            case 5://相手の攻撃を受けた時の音
                break;
            case 6://コンボ最大時のジングル
                break;
            case 7://魔法陣が完成した時の音
                break;
            case 8://最後の魔法攻撃の音(発生→着弾まで→着弾+爆発)
                break;
            case 9://軌跡のヒット音
                break;
            case 10://セットアップの終了音
                break;
            case 11://ターン開始音
                break;
            case 12://ターン終了音
                break;
            case 13://ゲーム終了時の効果音(ライトアップ)
                break;
            case 14://強風イベント発生音
                break;
            case 15://HalfWayBoost発生音
                break;
            case 16://特殊タイル発生音
                break;
            case 17://WaitAttackEnd出現音
                break;
            case 18://戦闘シーンへの移行音
                break;
            case 19://戦闘シーンの開始音
                break;
            case 20://鳴り続ける魔法陣の溜め音()
                break;
            case 21://時が止まっている時の音
                break;
            case 22://時が動き出す音
                break;
            case 23://攻撃を弾いた
                break;
            case 24://チュートリアルの達成音
                break;
            case 25://タイトルのタイプ音
                break;
            case 26://ゲーム終了時の攻撃音
                break;
            case 27://ファイター選択or攻撃フェイズで確認メッセージを出す時の音
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
