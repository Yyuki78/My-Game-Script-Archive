using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampMove : MonoBehaviour
{
    private int role = 0;
    [SerializeField] Material[] swampMat;

    public bool IsActive => gameObject.activeSelf;

    private GameObject Player;
    private GameObject generateEnemy;
    private Coroutine coroutine = null;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        generateEnemy = GameObject.FindGameObjectWithTag("enemy");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(hitEffect(other.gameObject));
            }
        }
        if (other.CompareTag("BackMostWall"))
        {
            StartCoroutine(destroy());
        }
    }

    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private IEnumerator hitEffect(GameObject player)
    {
        AudioManager.instance.SE(16);
        yield return null;
        var wait = new WaitForSeconds(1f);
        GetComponentInParent<SwampGenerate>().CheckSwampHit(role);
        switch (role)
        {
            case 0://画面を汚す
                GameObject.FindGameObjectWithTag("Castle").GetComponent<InkEffect>().InkSplash();
                break;
            case 1://壁走り時間を減らす
                AudioManager.instance.SE(14);
                PlayerWallSpecification pw = player.GetComponentInParent<PlayerWallSpecification>();
                pw.HealStayWallTime(-5f);
                break;
            case 2://車を滑るようにする
                player.GetComponentInParent<PlayerMove>().FreezeWheel();
                break;
            case 3://敵車の上下をランダムに入れ替える
                //敵の車をすべて取得
                GameObject randomObj;
                int choiceNum;
                GameObject[] enemyCar = GameObject.FindGameObjectsWithTag("EnemyCar");
                yield return null;
                List<GameObject> myList = new List<GameObject>(enemyCar.Length);
                List<GameObject> useList = new List<GameObject>(enemyCar.Length / 2);
                for (int i = 0; i < enemyCar.Length; i++)
                    myList.Add(enemyCar[i]);

                yield return null;
                //その内の1/4を選出
                for (int i = 0; i < enemyCar.Length / 4; i++)
                {
                    //Listの中からランダムで1つを選ぶ
                    randomObj = myList[Random.Range(0, myList.Count)];
                    //選んだオブジェクトをuseListに追加
                    useList.Add(randomObj);
                    //選んだオブジェクトのリスト番号を取得
                    choiceNum = myList.IndexOf(randomObj);
                    //同じリスト番号をmyListから削除
                    myList.RemoveAt(choiceNum);
                }
                yield return null;
                //最大対象数は12台
                while (useList.Count > 12)
                {
                    useList.RemoveAt(useList.Count - 1);
                }

                for (int i = 0; i < useList.Count; i++)
                {
                    if (useList[i] != null)
                    {
                        if (useList[i].transform.position.x > Player.transform.position.x + 15f)
                        {
                            useList[i].GetComponentInParent<EnemyMove>().ReversalUpDown();
                        }
                    }
                }
                AchievementDetection.TeleportNum++;
                break;
            case 4://敵車を増やす
                GenerateEnemy generate = generateEnemy.GetComponent<GenerateEnemy>();
                int ranPos, ranSize;
                for (int i = 0; i < 6; i++)
                {
                    ranPos = Random.Range(0, 8);

                    ranSize = Random.Range(0, 10);
                    if (ranSize < 5)
                        ranSize = 0;
                    else if (ranSize < 8)
                        ranSize = 1;
                    else
                        ranSize = 2;
                    generate.GenerateEnemyCar(ranPos, ranSize);
                    yield return wait;
                }
                break;
            case 5://画面にブラーをかける
                GameObject.FindGameObjectWithTag("Tank").GetComponent<PostEffect>().DrunkEffect();
                break;
            default:
                break;
        }
        yield return wait;
        gameObject.SetActive(false);
        yield break;
    }

    public void Init(Vector3 origin, Quaternion rotate, int size)
    {
        transform.position = origin;
        transform.rotation = rotate;
        switch (size)
        {
            case 0:
                transform.localScale = new Vector3(0.5f, 1, 1);
                break;
            case 1:
            case 2:
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case 3:
                transform.localScale = new Vector3(2, 1, 1);
                break;
            case 4:
                transform.localScale = new Vector3(2, 2, 2);
                break;
        }

        role = Random.Range(0, 6);

        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        Material[] materials = renderer.sharedMaterials;
        materials[0] = swampMat[role];
        renderer.sharedMaterials = materials;

        gameObject.SetActive(true);
        coroutine = null;
    }
}
