using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ScoreTextObjectPool : MonoBehaviour
{
    [SerializeField] HitText hitText;
    private string popText;
    private int randomNum;
    private Color textColor;

    [SerializeField] [TextArea]
    string[] ComboTexts;

    [SerializeField]
    Color[] colorR;

    [SerializeField]
    Color[] colorB;

    // アクティブなテキストのリスト
    private List<HitText> activeList = new List<HitText>();
    // 非アクティブなテキストのオブジェクトプール
    private Stack<HitText> inactivePool = new Stack<HitText>();

    void Start()
    {
        randomNum = Random.Range(0, 3);
        for (int i = 0; i < ComboTexts.Length; i++)
            ComboTexts[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "ComboText" + (i + 1).ToString());
    }

    private void Update()
    {
        // 逆順にループを回して、activeListの要素が途中で削除されても正しくループが回るようにする
        for (int i = activeList.Count - 1; i >= 0; i--)
        {
            var hitText = activeList[i];
            if (!hitText.IsActive)
            {
                Remove(hitText);
            }
        }
    }

    // テキストを非アクティブ化するメソッド
    public void Remove(HitText hitText)
    {
        activeList.Remove(hitText);
        inactivePool.Push(hitText);
    }

    // テキストをアクティブ化するメソッド　自分のみ
    public void Active(Vector3 pos, int combo = 0, int type = 0)
    {
        if (type == 0)//剣のヒット時テキスト
        {
            // 非アクティブのテキストがあれば使い回す、なければ生成する
            var HitText = (inactivePool.Count > 0) ? inactivePool.Pop() : Instantiate(hitText, transform);

            HitText.gameObject.SetActive(true);
            popText = ComboTexts[randomNum + (combo / 2) * 3];
            if (combo > 2)
                combo -= 2;
            HitText.Init(pos, popText, colorR[combo / 2]);
            activeList.Add(HitText);
        }else if (type == 1)//突撃兵の青い剣のヒット時テキスト
        {
            // 非アクティブのテキストがあれば使い回す、なければ生成する
            var HitText = (inactivePool.Count > 0) ? inactivePool.Pop() : Instantiate(hitText, transform);

            HitText.gameObject.SetActive(true);
            popText = ComboTexts[randomNum + (combo / 2) * 3];
            if (combo > 2)
                combo -= 2;
            HitText.Init(pos, popText, colorB[combo / 2]);
            activeList.Add(HitText);
        }
    }
}
