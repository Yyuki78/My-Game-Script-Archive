using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerate : MonoBehaviour
{
    [SerializeField] GameObject Coin;
    [SerializeField] CoinMove coinMove;
    [SerializeField] Transform[] generatePos;

    private float previousPos;

    // アクティブなコインのリスト
    private List<CoinMove> activeList = new List<CoinMove>();
    // 非アクティブなコインのオブジェクトプール
    private Stack<CoinMove> inactivePool = new Stack<CoinMove>();

    void Start()
    {
        StartCoroutine(generate());
        StartCoroutine(FirstGenerate());
    }

    private void Update()
    {
        // 逆順にループを回して、activeListの要素が途中で削除されても正しくループが回るようにする
        for (int i = activeList.Count - 1; i >= 0; i--)
        {
            var coinMove = activeList[i];
            if (!coinMove.IsActive)
            {
                Remove(coinMove);
            }
        }
    }

    // コインを非アクティブ化するメソッド
    public void Remove(CoinMove coinMove)
    {
        activeList.Remove(coinMove);
        inactivePool.Push(coinMove);
    }

    // コインをアクティブ化するメソッド　自分のみ
    private void Active(Vector3 pos, Quaternion rotate)
    {
        // 非アクティブのコインがあれば使い回す、なければ生成する
        var CoinMove = (inactivePool.Count > 0) ? inactivePool.Pop() : Instantiate(coinMove, transform);

        CoinMove.gameObject.SetActive(true);
        CoinMove.Init(pos, rotate);
        activeList.Add(CoinMove);
    }

    private IEnumerator generate()
    {
        var wait = new WaitForSeconds(1f);
        int ranPos;
        Vector3 rotation = new Vector3(1, 0, 0);
        float angle = 0;

        while (true)
        {
            if (generatePos[0].position.x - previousPos > 5f)
            {
                int ran = Random.Range(0, 25);
                switch (ran)
                {
                    case 0:
                    case 1:
                        StartCoroutine(ContinuousCoin());
                        yield return wait;
                        break;
                    case 2:
                    case 3:
                        StartCoroutine(DiagonalWallCoin());
                        yield return wait;
                        break;
                    case 4:
                        StartCoroutine(DiagonalLoadCoin());
                        yield return wait;
                        break;
                    default:
                        ranPos = Random.Range(0, 14);

                        if (ranPos < 8)
                            angle = 0;
                        else if (ranPos == 8 || ranPos == 13)
                            angle = 45;
                        else if (ranPos == 10 || ranPos == 11)
                            angle = 135;
                        else
                            angle = 90;

                        Active(generatePos[ranPos].localPosition, Quaternion.AngleAxis(angle, rotation));
                        break;
                }
            }

            previousPos = generatePos[0].position.x;

            float time = Random.Range(0.75f, 2.75f);
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator ContinuousCoin()
    {
        var wait = new WaitForSeconds(0.5f);
        int ranPos;
        Vector3 rotation = new Vector3(1, 0, 0);
        float angle = 0;

        ranPos = Random.Range(0, 14);
        if (ranPos < 8)
            angle = 0;
        else if (ranPos == 8 || ranPos == 13)
            angle = 45;
        else if (ranPos == 10 || ranPos == 11)
            angle = 135;
        else
            angle = 90;

        for (int i = 0; i < 3; i++)
        {
            Active(generatePos[ranPos].localPosition, Quaternion.AngleAxis(angle, rotation));

            yield return wait;
        }
        yield break;
    }

    private IEnumerator DiagonalWallCoin()
    {
        var wait = new WaitForSeconds(0.5f);
        int ranPos, generateNum;
        Vector3 rotation = new Vector3(1, 0, 0);
        float angle = 0;

        ranPos = Random.Range(0, 4);
        for (int i = 0; i < 3; i++)
        {
            switch (ranPos)
            {
                case 0:
                    generateNum = 8 + i;
                    break;
                case 1:
                    generateNum = 10 - i;
                    break;
                case 2:
                    generateNum = 11 + i;
                    break;
                case 3:
                    generateNum = 13 - i;
                    break;
                default:
                    generateNum = 0;
                    break;
            }
            if (generateNum == 8 || ranPos == 13)
                angle = 45;
            else if (generateNum == 10 || ranPos == 11)
                angle = 135;
            else
                angle = 90;

            Active(generatePos[generateNum].localPosition, Quaternion.AngleAxis(angle, rotation));

            yield return wait;
        }
        yield break;
    }

    private IEnumerator DiagonalLoadCoin()
    {
        var wait = new WaitForSeconds(0.5f);
        int ranPos, generateNum;

        int ran = Random.Range(0, 4);
        ranPos = Random.Range(0, 2);
        for (int i = 0; i < 3; i++)
        {
            switch (ran)
            {
                case 0:
                    generateNum = ranPos + i;
                    break;
                case 1:
                    generateNum = 3 - ranPos - i;
                    break;
                case 2:
                    generateNum = 4 + ranPos + i;
                    break;
                case 3:
                    generateNum = 6 - ranPos - i;
                    break;
                default:
                    generateNum = 0;
                    break;
            }

            Active(generatePos[generateNum].localPosition, Quaternion.identity);

            yield return wait;
        }
        yield break;
    }

    private IEnumerator FirstGenerate()
    {
        int ranPos;
        float minusPosX = 125f;
        for (int i = 0; i < 12; i++)
        {
            Vector3 pos;
            Vector3 rotation = new Vector3(1, 0, 0);
            float angle = 0;

            ranPos = Random.Range(0, 14);

            if (ranPos < 8)
                angle = 0;
            else if (ranPos == 8 || ranPos == 13)
                angle = 45;
            else if (ranPos == 10 || ranPos == 11)
                angle = 135;
            else
                angle = 90;

            pos = generatePos[ranPos].localPosition - new Vector3(minusPosX, 0, 0);
            minusPosX -= 20f;

            Active(pos, Quaternion.AngleAxis(angle, rotation));

            yield return null;
        }
        yield return null;
        minusPosX = 0;

        yield break;
    }
}
