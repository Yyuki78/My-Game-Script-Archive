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

    // �A�N�e�B�u�ȃe�L�X�g�̃��X�g
    private List<HitText> activeList = new List<HitText>();
    // ��A�N�e�B�u�ȃe�L�X�g�̃I�u�W�F�N�g�v�[��
    private Stack<HitText> inactivePool = new Stack<HitText>();

    void Start()
    {
        randomNum = Random.Range(0, 3);
        for (int i = 0; i < ComboTexts.Length; i++)
            ComboTexts[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "ComboText" + (i + 1).ToString());
    }

    private void Update()
    {
        // �t���Ƀ��[�v���񂵂āAactiveList�̗v�f���r���ō폜����Ă����������[�v�����悤�ɂ���
        for (int i = activeList.Count - 1; i >= 0; i--)
        {
            var hitText = activeList[i];
            if (!hitText.IsActive)
            {
                Remove(hitText);
            }
        }
    }

    // �e�L�X�g���A�N�e�B�u�����郁�\�b�h
    public void Remove(HitText hitText)
    {
        activeList.Remove(hitText);
        inactivePool.Push(hitText);
    }

    // �e�L�X�g���A�N�e�B�u�����郁�\�b�h�@�����̂�
    public void Active(Vector3 pos, int combo = 0, int type = 0)
    {
        if (type == 0)//���̃q�b�g���e�L�X�g
        {
            // ��A�N�e�B�u�̃e�L�X�g������Ύg���񂷁A�Ȃ���ΐ�������
            var HitText = (inactivePool.Count > 0) ? inactivePool.Pop() : Instantiate(hitText, transform);

            HitText.gameObject.SetActive(true);
            popText = ComboTexts[randomNum + (combo / 2) * 3];
            if (combo > 2)
                combo -= 2;
            HitText.Init(pos, popText, colorR[combo / 2]);
            activeList.Add(HitText);
        }else if (type == 1)//�ˌ����̐����̃q�b�g���e�L�X�g
        {
            // ��A�N�e�B�u�̃e�L�X�g������Ύg���񂷁A�Ȃ���ΐ�������
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
