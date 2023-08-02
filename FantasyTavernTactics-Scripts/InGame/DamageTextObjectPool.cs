using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextObjectPool : MonoBehaviour
{
    private int BoardSize;
    [SerializeField] DamageText damageText;

    [SerializeField]
    Sprite[] redFlame;
    [SerializeField]
    Sprite[] yellowFlame;
    [SerializeField]
    Sprite[] blueFlame;

    // アクティブなテキストのリスト
    private List<DamageText> activeList = new List<DamageText>();
    // 非アクティブなテキストのオブジェクトプール
    private Stack<DamageText> inactivePool = new Stack<DamageText>();

    void Start()
    {
        GetComponentInParent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        BoardSize = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>().BoardSize;
    }

    private void Update()
    {
        // 逆順にループを回して、activeListの要素が途中で削除されても正しくループが回るようにする
        for (int i = activeList.Count - 1; i >= 0; i--)
        {
            var damageText = activeList[i];
            if (!damageText.IsActive)
            {
                Remove(damageText);
            }
        }
    }

    // テキストを非アクティブ化するメソッド
    public void Remove(DamageText damageText)
    {
        activeList.Remove(damageText);
        inactivePool.Push(damageText);
    }

    // テキストをアクティブ化するメソッド　自分のみ
    public void Active(Vector2 pos, int damage)
    {
        // 非アクティブのテキストがあれば使い回す、なければ生成する
        var DamageText = (inactivePool.Count > 0) ? inactivePool.Pop() : Instantiate(damageText, transform);

        DamageText.gameObject.SetActive(true);

        Vector3 setPos = new Vector3(0, 0, 0);
        float ranX, ranZ; 
        int ranNum;
        switch (BoardSize)
        {
            case 5:
                ranX = Random.Range(-0.04f, 0.04f);
                ranZ = Random.Range(-0.02f, 0.02f);
                setPos = new Vector3(-1.55f + ranX + (pos.x - 1) * 0.2f, -0.25f, 0.47f + ranZ - (pos.y - 1) * 0.2f);
                break;
            case 6:
                ranX = Random.Range(-0.03f, 0.03f);
                ranZ = Random.Range(-0.015f, 0.015f);
                setPos = new Vector3(-1.57f + ranX + (pos.x - 1) * 0.168f, -0.25f, 0.48f + ranZ - (pos.y - 1) * 0.168f);
                break;
            case 7:
                ranX = Random.Range(-0.02f, 0.02f);
                ranZ = Random.Range(-0.01f, 0.01f);
                setPos = new Vector3(-1.578f + ranX + (pos.x - 1) * 0.143f, -0.25f, 0.505f + ranZ - (pos.y - 1) * 0.143f);
                break;
        }
        if (damage >= 70)
        {
            ranNum = Random.Range(0, redFlame.Length);
            DamageText.Init(setPos, damage, redFlame[ranNum]);
        }
        else if (damage >= 30)
        {
            ranNum = Random.Range(0, yellowFlame.Length);
            DamageText.Init(setPos, damage, yellowFlame[ranNum]);
        }
        else
        {
            ranNum = Random.Range(0, blueFlame.Length);
            DamageText.Init(setPos, damage, blueFlame[ranNum]);
        }
        activeList.Add(DamageText);
    }
}
