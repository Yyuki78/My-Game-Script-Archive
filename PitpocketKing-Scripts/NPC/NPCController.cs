using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    //位置の基準になるオブジェクトのTransformを収める
    [SerializeField] Transform[] wayPoints;

    //座る位置の基準になるオブジェクトのTransformを収める
    [SerializeField] Transform[] sittingPoints;

    //立ち止まる位置の基準になるオブジェクトのTransformを収める
    [SerializeField] Transform[] stopPoints;

    private NavMeshAgent agent;
    private NPCAnimator _ani;

    //待機時間
    private float waitTime = 2;

    //どこを目指すのか　0は柵の奥,1はベンチ,2は自販機や店の入り口
    private int destinationNum = 0;

    //自分が消えるまでのカウント　0で消える
    private int finishCount = 5;

    //目的地の座標と回転量
    private Vector3 DestinationPosition;
    private Vector3 DestinationRotation;

    //自販機かドアの前か
    private bool isDrink = false;

    private bool once = true;
    private float stopTime = 0;

    public bool isInfinite = false;//これがtrueだとNPCは消えなくなる

    [SerializeField] AudioClip finishSound;

    void Start()
    {
        GameObject[] waypointsFind = GameObject.FindGameObjectsWithTag("waypoint");
        for(int i=0; i< waypointsFind.Length; i++)
        {
            wayPoints[i] = waypointsFind[i].transform;
        }

        GameObject[] sittingpointsFind = GameObject.FindGameObjectsWithTag("sittingpoint");
        for (int i = 0; i < sittingpointsFind.Length; i++)
        {
            sittingPoints[i] = sittingpointsFind[i].transform;
        }

        GameObject[] stoppointsFind = GameObject.FindGameObjectsWithTag("stoppoint");
        for (int i = 0; i < stoppointsFind.Length; i++)
        {
            stopPoints[i] = stoppointsFind[i].transform;
        }

        agent = GetComponent<NavMeshAgent>();
        _ani = GetComponent<NPCAnimator>();

        //目標地点に近づいても速度を落とさなくなる
        agent.autoBraking = false;

        //歩く速度をランダムに変更する
        agent.speed = Random.Range(1.5f, 4.5f);

        // 最初の目的地を入れる
        SetDestination();

        //目標地点を決める
        //GotoNextPoint();
    }

    private void SetDestination()
    {
        //Debug.Log("目的地を設定します");

        //稼働
        agent.enabled = true;
        agent.isStopped = false;
        agent.angularSpeed = 120;

        //目指す場所を決める
        destinationNum = Random.Range(0, 6);
        if (finishCount <= 1)//次で消える時
            destinationNum = 0;

        if (destinationNum == 5)
        {
            destinationNum = 1;
        }else if(destinationNum ==4)
        {
            destinationNum = 2;
        }
        else
        {
            destinationNum = 0;
        }

        switch (destinationNum)
        {
            case 0://柵の奥
                int parcent = Random.Range(0, 100);
                Vector3 newPos;
                float ran;
                if (parcent < 30)
                {
                    newPos = wayPoints[0].position;
                    ran = Random.Range(-6f, 6f);
                }
                else if(parcent < 60)
                {
                    newPos = wayPoints[1].position;
                    ran = Random.Range(-6f, 6f);
                }
                else if(parcent < 80)
                {
                    newPos = wayPoints[2].position;
                    ran = Random.Range(-6f, 6f);
                }
                else if(parcent < 90)
                {
                    newPos = wayPoints[3].position;
                    ran = Random.Range(-2f, 2f);
                }
                else
                {
                    newPos = wayPoints[4].position;
                    ran = Random.Range(-1.5f, 1.5f);
                }

                if (parcent < 60)
                    newPos.x += ran;
                else
                    newPos.z += ran;

                // 目的地を次の場所に設定
                agent.SetDestination(newPos);

                //経路があるかの確認
                RandomWander();

                //終わった後の待機時間の設定
                waitTime = 2;

                break;
            case 1://ベンチ
                int ranD = Random.Range(0, sittingPoints.Length);

                // 目的地を次の場所に設定
                agent.SetDestination(sittingPoints[ranD].position);

                //経路があるかの確認
                RandomWander();

                //終わった後の待機時間の設定
                waitTime = 5;

                //目的地の座標と回転量を設定
                DestinationPosition = sittingPoints[ranD].position;
                DestinationRotation = sittingPoints[ranD].eulerAngles;
                break;
            case 2://自販機、ドアの前等
                int ranS = Random.Range(0, stopPoints.Length);

                //目的地が自販機かドアの前か
                if (ranS < 3)
                    isDrink = true;
                else
                    isDrink = false;

                // 目的地を次の場所に設定
                agent.SetDestination(stopPoints[ranS].position);

                //経路があるかの確認
                RandomWander();

                //終わった後の待機時間の設定
                waitTime = 8;

                //目的地の座標と回転量を設定
                DestinationPosition = stopPoints[ranS].position;
                DestinationRotation = stopPoints[ranS].eulerAngles;
                break;
            default:
                Debug.Log("目的地の設定ミス");
                break;
        }

        StartCoroutine(waitMove());
    }

    private IEnumerator waitMove()
    {
        agent.speed = 0.1f;
        yield return new WaitForSeconds(1f);
        agent.speed = Random.Range(1.5f, 4.5f);

        yield return new WaitForSeconds(1f);
        once = true;
        yield break;
    }

    private void RandomWander()
    {
        //指定した目的地に障害物があるかどうか、そもそも到達可能なのかを確認して問題なければセットする。
        //pathPending 経路探索の準備できているかどうか
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                //hasPath エージェントが経路を持っているかどうか
                //navMeshAgent.velocity.sqrMagnitudeはスピード
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("辿り着けないです");
                    SetDestination();
                }
            }
        }
    }

    private IEnumerator StopHere()
    {
        //辿り着いていないなら別の場所に向かう
        if(agent.remainingDistance > agent.stoppingDistance)
        {
            SetDestination();
            yield break;
        }

        if (once)
        {
            once = false;
            //場所に応じて行う
            switch (destinationNum)
            {
                case 0://柵の奥
                    agent.isStopped = true;
                    agent.enabled = false;
                    break;
                case 1://ベンチ
                    StartCoroutine(_ani.Sitting(DestinationPosition, DestinationRotation));
                    break;
                case 2://自販機、ドアの前等
                    if(isDrink)
                        StartCoroutine(_ani.Drinking(DestinationPosition, DestinationRotation));
                    else
                        StartCoroutine(_ani.Stopping(DestinationPosition, DestinationRotation));
                    break;
                default:
                    Debug.Log("目的地の設定ミス");
                    break;
            }
            if(!isInfinite)
                finishCount--;
        }

        //待ち時間を数える
        yield return new WaitForSeconds(waitTime);

        //待ち時間が設定された数値を超えると発動
        if (finishCount <= 0)
            Finish();

        //目標地点を設定し直す
        SetDestination();

        yield break;
    }

    //NPCAnimatorで使用
    public void SetDes()
    {
        SetDestination();
    }

    void Update()
    {
        //NavMeshAgentが停止中ならreturn
        if (!agent.isActiveAndEnabled) return;

        //NPCStateがwalkかrunで、経路探索の準備ができておらず
        //目標地点までの距離が0.5m未満ならNavMeshAgentを止める
        if (_ani.isCanHit && !agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
            StartCoroutine(StopHere());

        //ずっと止まっている時に再起動する
        if (agent.speed == 0)
        {
            stopTime += Time.deltaTime;

            if (stopTime > 7.5f)
            {
                //目標地点を設定し直す
                SetDestination();
                stopTime = 0;
            }
        }
        else
        {
            stopTime = 0;
        }
    }

    private void Finish()
    {
        finishCount = 10;//1回しか呼ばない用
        Debug.Log("NPCが消えます");
        var g = GetComponentInParent<GameManager>();
        g.Repop(transform.position);

        var a = GetComponent<AudioSource>();
        a.PlayOneShot(finishSound);

        StartCoroutine(wait());
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}
