using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMControl : MonoBehaviour
{
    //ゲームクリア・ゲームオーバー時に音楽を止める
    //ゲージが9割を超えたらテンポを上げる(ピッチは未定)
    //
    //BGM用のクリップとソース
    [SerializeField] AudioClip BGM;
    [SerializeField] AudioSource _audioSource;

    //それ以外のクリップとソース
    [SerializeField] AudioClip Attack;
    [SerializeField] AudioClip Explode;
    [SerializeField] AudioClip GameOver;
    [SerializeField] AudioClip Clear;
    [SerializeField] AudioClip a;
    [SerializeField] AudioSource _audioSource2;

    //ダメージ効果音を一度だけ再生するためのbool
    private bool Damege1 = true;
    private bool Damege2 = true;

    [SerializeField] GameObject Fox;
    EnemyMove _move;
    [SerializeField] GameObject Ball;
    PlayerStatus _status;
    public float fill;

    public bool AttackTrue = false;//PopupText用の変数
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _move = Fox.GetComponent<EnemyMove>();
        _status = Ball.GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_move.slider.value >= 45)
        {
            _audioSource.pitch = 1.2f;
        }
        else
        {
            _audioSource.pitch = 1.0f;
        }

        if (_move.slider.value >= 50)
        {
            if (Ball.GetComponent<PlayerStatus>().Life <= 0) return;
            _audioSource.Stop();
            _audioSource2.volume = 0.3f;
            if (Damege1 == true || Damege2 == true)
            {
                _audioSource2.PlayOneShot(Clear);
                Damege1 = false;
                Damege2 = false;
            }
        }

        AttackTrue = false;//攻撃時のポップアップテキストが一度だけ表示されるため

        switch (_status.Life)
        {
            //自キャラのHPに応じて変える
            /*
            case 10:
                if (Damege1 == true)
                {
                    _audioSource2.PlayOneShot(Attack);
                    Damege1 = false;
                    Damege2 = true;
                }
                break;
                */
            case 9:
                if (Damege2 == true)
                {
                    _audioSource2.PlayOneShot(Attack);
                    Damege1 = true;
                    Damege2 = false;
                    //攻撃によるゲージ上昇
                    _move.fill += 3;
                    AttackTrue = true;
                }
                break;
            case 8:
                if (Damege1 == true)
                {
                    _audioSource2.PlayOneShot(Attack);
                    Damege1 = false;
                    Damege2 = true;
                    //攻撃によるゲージ上昇
                    _move.fill += 3;
                    AttackTrue = true;
                }
                break;
            case 7:
                if (Damege2 == true)
                {
                    _audioSource2.PlayOneShot(Attack);
                    Damege1 = true;
                    Damege2 = false;
                    //攻撃によるゲージ上昇
                    _move.fill += 3;
                    AttackTrue = true;
                }
                break;
            case 6:
                if (Damege1 == true)
                {
                    _audioSource2.PlayOneShot(Attack);
                    Damege1 = false;
                    Damege2 = true;
                    //攻撃によるゲージ上昇
                    _move.fill += 3;
                    AttackTrue = true;
                }
                break;
            case 5:
                if (Damege2 == true)
                {
                    _audioSource2.PlayOneShot(Attack);
                    Damege1 = true;
                    Damege2 = false;
                    //攻撃によるゲージ上昇
                    _move.fill += 3;
                    AttackTrue = true;
                }
                break;
            case 4:
                if (Damege1 == true)
                {
                    _audioSource2.PlayOneShot(Attack);
                    Damege1 = false;
                    Damege2 = true;
                    //攻撃によるゲージ上昇
                    _move.fill += 3;
                    AttackTrue = true;
                }
                break;
            case 3:
                if (Damege2 == true)
                {
                    _audioSource2.PlayOneShot(Attack);
                    Damege1 = true;
                    Damege2 = false;
                    //攻撃によるゲージ上昇
                    _move.fill += 3;
                    AttackTrue = true;
                }
                break;
            case 2:
                if (Damege1 == true)
                {
                    _audioSource2.PlayOneShot(Attack);
                    Damege1 = false;
                    Damege2 = true;
                    //攻撃によるゲージ上昇
                    _move.fill += 3;
                    AttackTrue = true;
                }
                break;
            case 1:
                if (Damege2 == true)
                {
                    _audioSource2.PlayOneShot(Attack);
                    Damege1 = true;
                    Damege2 = false;
                    //攻撃によるゲージ上昇
                    _move.fill += 3;
                    AttackTrue = true;
                }
                break;
        }

        if (_status.Life <= 0)
        {
            _audioSource.Stop();
            _audioSource2.volume = 0.4f;
            if (Damege2 == false)
            {
                _audioSource2.PlayOneShot(Explode);
                Damege2 = true;
            }
            StartCoroutine("GameOverCoroutine");
        }
    }
    IEnumerator GameOverCoroutine()
    {
        if (Damege1 == false) yield break;
        Damege1 = false;
        yield return new WaitForSeconds(0.5f);
        _audioSource2.PlayOneShot(GameOver);
    }
}
