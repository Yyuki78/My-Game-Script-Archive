using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffectManager : MonoBehaviour
{
    private int BoardSize;
    [SerializeField] BuffEffect BuffEffects;

    // アクティブなテキストのリスト
    private List<BuffEffect> activeList = new List<BuffEffect>();
    // 非アクティブなテキストのオブジェクトプール
    private Stack<BuffEffect> inactivePool = new Stack<BuffEffect>();

    void Start()
    {
        BoardSize = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>().BoardSize;
    }
    
    void Update()
    {
        // 逆順にループを回して、activeListの要素が途中で削除されても正しくループが回るようにする
        for (int i = activeList.Count - 1; i >= 0; i--)
        {
            var buffEffect = activeList[i];
            if (!buffEffect.IsActive)
            {
                Remove(buffEffect);
            }
        }
    }

    // テキストを非アクティブ化するメソッド
    public void Remove(BuffEffect buffEffect)
    {
        activeList.Remove(buffEffect);
        inactivePool.Push(buffEffect);
    }

    // テキストをアクティブ化するメソッド　自分のみ
    public void Active(Vector2 pos, int num)
    {
        // 非アクティブのテキストがあれば使い回す、なければ生成する
        var BuffEffect = (inactivePool.Count > 0) ? inactivePool.Pop() : Instantiate(BuffEffects, transform);

        BuffEffect.gameObject.SetActive(true);

        Vector3 generatePos = new Vector3(0, 0, 0);
        switch (BoardSize)
        {
            case 5:
                generatePos = new Vector3(4.579f + (pos.y - 1) * 0.2f, 1.4f, 4.097f + (pos.x - 1) * 0.2f);
                break;
            case 6:
                generatePos = new Vector3(4.559f + (pos.y - 1) * 0.168f, 1.4f, 4.077f + (pos.x - 1) * 0.168f);
                break;
            case 7:
                generatePos = new Vector3(4.55f + (pos.y - 1) * 0.143f, 1.4f, 4.067f + (pos.x - 1) * 0.143f);
                break;
        }
        BuffEffect.Init(generatePos, num);
        activeList.Add(BuffEffect);
    }
}
