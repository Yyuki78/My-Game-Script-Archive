using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    /// <summary>
    /// ライフ最大値を返します
    /// </summary>
    public float LifeMax => lifeMax;

    /// <summary>
    /// ライフの値を返します
    /// </summary>
    public float Life => _life;

    //　LifeGaugeスクリプト
    [SerializeField]
    private LifeGauge lifeGauge;

    public float lifeMax = 5; // ライフ最大値
    private float _life; // 現在のライフ値（ヒットポイント）

    [SerializeField] GameObject gameOver;//ゲームオーバー画面
    [SerializeField] GameObject GameOverParticle;//ゲームオーバー時のエフェクト

    GameObject Fox; //Foxそのものが入る変数
    MobAttack _attack; //MobAttackが入る変数

    protected void Start()
    {
        _life = lifeMax; // 初期状態はライフ満タン
        //　体力ゲージに反映
        lifeGauge.SetLifeGauge(_life);

        Fox = GameObject.Find("Fox"); //Foxをオブジェクトの名前から取得して変数に格納する
        _attack = Fox.GetComponent<MobAttack>(); //Foxの中にあるMobAttackを取得して変数に格納する
    }

    /// <summary>
    /// 指定値のダメージを受けます。
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        var source = GetComponent<Cinemachine.CinemachineImpulseSource>();
        if (_attack.attckone == false) return;
        _attack.attckone = false;
        _life -= damage;
        if (_life >= 0)
        {
            //カメラを揺らす
            source.GenerateImpulse();
            lifeGauge.SetLifeGauge(_life);
        }
        if (_life > 0) return;

        //HPが0になった際の動作
        // ゲームオーバーを表示
        GameOverParticle.SetActive(true);
        gameOver.SetActive(true);
        GetComponent<UnityStandardAssets.Vehicles.Ball.BallUserControl>().enabled = false;
        _attack.enabled = false;
    }
}
