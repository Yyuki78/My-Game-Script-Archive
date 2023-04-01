using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStatus))]
public class EnemyMove : MonoBehaviour
{
    [SerializeField] private LayerMask raycastLayerMask; // レイヤーマスク

    private NavMeshAgent _agent;
    private RaycastHit[] _raycastHits = new RaycastHit[10];
    private EnemyStatus _status;
    [SerializeField] GameObject Ball;
    Rigidbody _rigidbody;

    //鳴き声
    public AudioClip sound1;
    AudioSource _audioSource;

    //満足ゲージ
    public bool StartGauge = false;
    public bool reduce = false;
    public Slider slider;
    public float fill;
    //満足ゲージが増えているかどうかのbool
    public bool plus = false;
    [SerializeField] GameObject _foxGauge;

    [SerializeField] GameObject gameClear;//ゲームクリア画面
    [SerializeField] GameObject GameClearParticle;//ゲームクリア時のエフェクト
    public GameObject ball;//FoxのCollider

    public bool hukan = false;//カメラの俯瞰視点変換用

    public bool clear = false;//ゲームクリア時に他のスクリプトを止めるようのbool

    private FootCollisionDetector _footcol;//足跡判定用のスクリプト

    //土管イベント用の変数
    [SerializeField] GameObject _DokanDetect;
    private DokanEvent _DokanEve;
    public bool DokanTrue = false;//ポップアップテキストを表示するための関数
    private bool once = false;

    //ネズミイベント用の変数
    [SerializeField] GameObject Rat;
    private RatEvent RatEve;
    public bool RatTrue = false;//ポップアップテキストを表示するための関数
    private bool once2 = false;

    //農具イベント用の変数
    [SerializeField] ObstacleEvent ObsEve;
    public bool ObsTrue = false;
    private bool once3 = false;

    //攻撃成功時にゲージを上昇させるための変数
    //BGMControlにて実装(fillもいじられてる)

    private bool Sitonce = false;//座るためのbool

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>(); // NavMeshAgentを保持しておく
        _status = GetComponent<EnemyStatus>();
        _rigidbody = Ball.GetComponent<Rigidbody>();

        _audioSource = GetComponent<AudioSource>();
        fill = 0;
        slider.value = 0;

        _footcol = Ball.GetComponent<FootCollisionDetector>();

        _DokanEve = _DokanDetect.GetComponent<DokanEvent>();

        RatEve = Rat.GetComponent<RatEvent>();
    }

    private void Update()
    {
        if (fill < 0)
        {
            fill = 0;
        }
        
        //範囲外の時に止まる
        if (_agent.remainingDistance >= 5f)
        {
            _agent.isStopped = true;
        }

        if (reduce == true && fill > 0 && plus == false)
        {
            //範囲外なのでゲージを減らす
            fill -= 0.1f;
        }

        //条件を満たしたら俯瞰視点になる
        if (StartGauge == true && _agent.velocity.magnitude == 0 && Input.anyKey == false && _rigidbody.velocity.magnitude <= 2 && _status.IsSittable)
        {
            //CameraControll
            hukan = true;
        }
        else
        {
            hukan = false;
        }

        //3秒以上キツネが動いていないなら座る
        if (_agent.velocity.magnitude == 0)
        {
            if (Sitonce == false)
            {
                StartCoroutine(SitCoroutine());
                Sitonce = true;
            }
        }

        /*
        if (_foxGauge.GetComponent<FoxGauge>().reduse == true)
        {
           fill -= 0.1f;
        }*/

        //土管イベントが成功したときにゲージを上昇させる
        if (_DokanEve.DokanEventBool == true && once == false)
        {
            //DokanTrue = true;
            once = true;
            fill += 50;
        }
        /*
        if (once == true)
        {
            DokanTrue = false;
        }*/

        //ネズミイベントが成功したときにゲージを上昇させる
        if (once2 == true)
        {
            RatTrue = false;
            once2 = false;
        }
        if (RatEve.Rat1Eve == true || RatEve.Rat2Eve == true || RatEve.Rat3Eve == true || RatEve.Rat4Eve == true || RatEve.Rat5Eve == true || RatEve.Rat6Eve == true || RatEve.Rat7Eve == true || RatEve.Rat8Eve == true || RatEve.Rat9Eve == true || RatEve.Rat10Eve == true)
        {
            if (once2 == false)
            {
                RatTrue = true;
                once2 = true;
                fill += 10;
                Debug.Log("10点が加算されます");
            }
        }

        //農具イベントが成功したときにゲージを上昇させる
        if (ObsEve.ObstacleEve == true && once3 == false)
        {
            ObsTrue = true;
            once = true;
            fill += 100;
        }

        //ゲージの表示
        slider.value = fill * 0.1f;
    }

    //SoundDetectorのonTriggerEnterにセットし、衝突判定を受け取るメソッド
    public void OnReduceOffObject(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            //プレイヤーが検知範囲内に入ったのでゲージ減少を止める
            reduce = false;
        }
    }

    //SoundDetectorのonTriggerStayにセットし、衝突判定を受け取るメソッド
    public void OnSoundObject(Collider collider)
    {
        // 検知したオブジェクトに「Player」のタグがついている+キツネの検知範囲内に一度でも入っている＋キツネが動いている+足跡の上でないならゲージを増やす
        if (collider.CompareTag("Player") && StartGauge == true && _agent.velocity.magnitude != 0 && _footcol.FootGaugeCol == false)
        {
            //範囲内なのでゲージを増やす
            fill += 0.1f;
            plus = true;
        }
        else
        {
           plus = false;
        }
    }

    //SoundDetectorのonTriggerExitにセットし、衝突判定を受け取るメソッド
    public void OnReduceObject(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            //プレイヤーが検知範囲内に検知範囲外に出たのでゲージを減らす
            reduce = true;
            plus = false;
        }
    }

    // CollisionDetectorのonTriggerStayにセットし、衝突判定を受け取るメソッド
    public void OnDetectObject(Collider collider)
    {
        StartGauge = true;
        if (slider.value >= 50)
        {
            if (Ball.GetComponent<PlayerStatus>().Life <= 0) return;
            //満足ゲージが最大になった際の動作
            // ゲームクリアを表示
            gameClear.SetActive(true);
            GameClearParticle.SetActive(true);
            GetComponent<MobAttack>().enabled = false;
            GetComponent<EnemyStatus>().enabled = false;
            ball.SetActive(false);
            _status.Clear();
            clear = true;
            GetComponent<EnemyMove>().enabled = false;
        }
        if (collider.CompareTag("Player"))
        {
            if (_agent.isStopped == true)
            {
                Sitonce = false;
                _status.StandUp();
            }
        }

        if (!_status.IsMovable)
        {
            _agent.isStopped = true;
            return;
        }

        // 検知したオブジェクトに「Player」のタグがついていれば、そのオブジェクトを追いかける
        if (collider.CompareTag("Player"))
        {
            var positionDiff = collider.transform.position - transform.position; // 自身とプレイヤーの座標差分を計算
            var distance = positionDiff.magnitude; // プレイヤーとの距離を計算
            var direction = positionDiff.normalized; // プレイヤーへの方向

            // _raycastHitsに、ヒットしたColliderや座標情報などが格納される
            // RaycastAllとRaycastNonAllocは同等の機能だが、RaycastNonAllocだとメモリにゴミが残らないのでこちらを推奨
            var hitCount = Physics.RaycastNonAlloc(transform.position, direction, _raycastHits, distance, raycastLayerMask);
        //    Debug.Log("hitCount: " + hitCount);
            if (hitCount == 0)
            {
                // 本作のプレイヤーはCharacterControllerを使っていて、Colliderは使っていないのでRaycastはヒットしない
                // つまり、ヒット数が0であればプレイヤーとの間に障害物が無いということになる
                _agent.isStopped = false;
                _agent.destination = collider.transform.position;
            }
            else
            {
                // 見失ったら停止する
                _agent.isStopped = true;
            }
        }
    }

    private IEnumerator SitCoroutine()
    {
        yield return new WaitForSeconds(3);
        _status.Sit();
        Sitonce = false;
    }
}