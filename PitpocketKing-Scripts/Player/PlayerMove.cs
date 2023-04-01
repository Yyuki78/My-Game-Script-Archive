using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public int getMoney { private set; get; } = 0;

    [SerializeField] float speed = 10F;
    [SerializeField] float rotateSpeed = 1.0F;

    private CharacterController controller;
    private Animator _ani;

    public bool EnableMove = false;

    //ItemBehaviorで変更
    public bool isUpGetMoney = false;
    public bool already = false;
    private Coroutine playCoroutine;

    private Coroutine _currentCoroutine;
    private GameObject hitNPC;
    private bool isHit = false;
    public bool isCanHit { private set; get; } = true;//NPCとぶつかってから0.25秒はぶつかれない

    //お金を表示する者たち
    [SerializeField] GameObject popUpScoreObj;
    [SerializeField] GameObject getMoneyText;
    private GetMoneyText _text;

    //コンボ
    [SerializeField] GameObject popUpComboObj;
    private ComboGauge _combo;

    //連続奪取
    [SerializeField] GameObject popUpinaLowObj;
    private int isInaLow = 0;//連続奪取回数

    private Light _light;//自身のライト

    private AudioManager _audio;//音

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        _ani = GetComponentInChildren<Animator>();
        _text = getMoneyText.GetComponent<GetMoneyText>();
        _combo = GetComponentInChildren<ComboGauge>();
        _audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        _light = GetComponentInChildren<Light>();
        _light.intensity = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        updateMove();
        updateAnimation();
        updateWalkSound();
        updateLookAt();
        updateItemEvent();
    }

    //Playerの動き
    private void updateMove()
    {
        if (EnableMove)
        {
            //回転させる
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);

            //Transform.TransformDirectionはローカル空間からワールド空間へ方向Vectorを変換する
            Vector3 forward = transform.TransformDirection(Vector3.forward);

            //前進後退する
            float currentSpeed = speed * Input.GetAxis("Vertical");
            controller.SimpleMove(forward * currentSpeed);
        }
        else
        {

        }
    }

    //Playerの歩行アニメーション
    private void updateAnimation()
    {
        if (EnableMove)
        {
            _ani.SetFloat("MoveSpeed", controller.velocity.magnitude);
        }
    }

    //Playerの歩行サウンド
    private void updateWalkSound()
    {
        if (!EnableMove)
        {
            _audio.StopWalkSE();
        }
        else
        {
            float speed = controller.velocity.magnitude;
            if (speed == 0)
            {
                _audio.StopWalkSE();
            }
            else if (speed < 1.5f)
            {
                _audio.SE1();
            }
            else if (speed < 3f)
            {
                _audio.SE2();
            }
            else
            {
                _audio.SE3();
            }
        }
    }

    //NPCとの接触時にNPCの方向を向く
    private void updateLookAt()
    {
        if (isHit)
        {
            Vector3 relativePoint = hitNPC.transform.position;
            relativePoint.y = transform.position.y;
            transform.LookAt(relativePoint);
        }
    }

    //ぶつかったNPCに呼ばれる
    public void StartApologize(GameObject hitObj)
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
        _currentCoroutine = StartCoroutine(HitNPC(hitObj));
    }

    private IEnumerator HitNPC(GameObject hitObj)
    {
        //アニメーションのリセット＋動けなくする
        isCanHit = false;
        _ani.Play("change state", 0, 0);

        Debug.Log("NPCとぶつかった！");
        EnableMove = false;

        controller.SimpleMove(new Vector3(0, 0, 0));
        _ani.SetFloat("MoveSpeed", 0f);

        //ぶつかったNPCの方向を向き続ける
        hitNPC = hitObj;
        Vector3 relativePoint = hitNPC.transform.position;
        relativePoint.y = transform.position.y;
        transform.LookAt(relativePoint);
        //ぶつかったNPCの情報を取得
        var info = hitNPC.GetComponent<NPCInformation>();

        //既に当たっている状態＋警察・泥棒でないなら獲得賞金が増える
        if (isHit)
        {
            if (info.role != 1 && info.role != 3)
            {
                isInaLow++;
                Instantiate(popUpinaLowObj, new Vector3(0, 0, 0), Quaternion.identity, _text.gameObject.transform);
                _audio.SE10();
            }
        }
         isHit = true;

        _ani.SetBool("Apologizing", true);
        
        yield return new WaitForSeconds(0.01f);

        //ぶつかったNPCに応じた金の変動
        float getNum = 0;
        switch (info.role)
        {
            case 0://一般人
                getNum = Random.Range(5000, 20000);
                _combo.ComboDecide();
                _audio.SE6();
                //_audio.SE11();
                break;
            case 1://警察
                yield return new WaitForSeconds(0.25f);
                GameOver();
                yield break;
            case 2://職業持ち
                getNum = Random.Range(15000, 40000);
                _combo.ComboDecide();
                _audio.SE6();
                //_audio.SE11();
                break;
            case 3://泥棒
                getNum = Random.Range(-50000, -20000);
                _audio.SE14();
                break;
            case 4://お金持ち
                getNum = Random.Range(75000, 100000);
                _combo.ComboDecide();
                _audio.SE7();
                //_audio.SE11();
                break;
            case 5://超お金持ち
                getNum = Random.Range(150000, 200000);
                _combo.ComboDecide();
                //_audio.SE11();
                break;
            default:
                Debug.Log("NPCの役職が変です");
                break;
        }
        yield return new WaitForSeconds(0.25f);
        //獲得金額アップ中かどうか
        if (isUpGetMoney && getNum > 0)
            getNum *= 1.5f;

        //増える分のお金を表示
        var ins = Instantiate(popUpScoreObj, new Vector3(0, 0, 0), Quaternion.identity, _text.gameObject.transform);
        ins.transform.localPosition = new Vector3(40, 25, 0);
        ins.GetComponent<PopUpScoreText>().score = (int)getNum;
        ins.GetComponent<PopUpScoreText>().comboNum = _combo.comboNum;
        ins.GetComponent<PopUpScoreText>().isInaLow = isInaLow;

        //コンボ倍率
        if (getNum > 0)
            getNum *= (1 + Mathf.Min(_combo.comboNum - 1, 10) * 0.3f);
        //連続ヒット倍率
        if (getNum > 0)
            getNum *= (Mathf.Pow(2, isInaLow));
        //音を鳴らす
        if (getNum > 120000)
            _audio.SE8();
        //全体のお金を増やす
        getMoney += (int)getNum;
        //増える分のお金を表示UIの獲得金額を増やす
        _text.SlideToNumber(getMoney, 1.75f);

        //コンボが2以上かどうか
        if (_combo.comboNum > 1 && info.role != 1 && info.role != 3)
        {
            var insC = Instantiate(popUpComboObj, new Vector3(0, 0, 0), Quaternion.identity, _text.gameObject.transform);
            insC.GetComponent<ComboUpText>().comboNum = _combo.comboNum;
        }

        isCanHit = true;

        //後ろに下がる
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        for (int i = 0; i < 50; i++)
        {
            controller.SimpleMove(forward * -1.0f);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1.5f);
        _ani.SetBool("Apologizing", false);
        _ani.Play("change state", 0, 0);

        yield return new WaitForSeconds(0.5f);
        EnableMove = true;
        isInaLow = 0;
        isHit = false;
    }

    //獲得金額増加イベントのリセット用
    private void updateItemEvent()
    {
        if (isUpGetMoney)
        {
            _light.intensity = 1.25f;
            if (already)
            {
                already = false;
                if (playCoroutine != null)
                {
                    StopCoroutine(playCoroutine);
                }
                playCoroutine = StartCoroutine("upMoneyTimer");
            }
        }
    }

    private IEnumerator upMoneyTimer()
    {
        yield return new WaitForSeconds(16f);
        already = false;
        isUpGetMoney = false;
        _light.intensity = 0.25f;
    }

    private void GameOver()
    {
        Debug.Log("ゲームオーバー");
        Time.timeScale = 0f;

        _combo.Reset();

        _audio.StopBGM();

        StartCoroutine(GameObject.FindGameObjectWithTag("vCamera").GetComponent<CameraController>().GameOver());
    }
}
