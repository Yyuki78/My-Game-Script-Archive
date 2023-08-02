using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffectManager : MonoBehaviour
{
    private int BoardSize;
    [SerializeField] BuffEffect BuffEffects;

    // �A�N�e�B�u�ȃe�L�X�g�̃��X�g
    private List<BuffEffect> activeList = new List<BuffEffect>();
    // ��A�N�e�B�u�ȃe�L�X�g�̃I�u�W�F�N�g�v�[��
    private Stack<BuffEffect> inactivePool = new Stack<BuffEffect>();

    void Start()
    {
        BoardSize = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>().BoardSize;
    }
    
    void Update()
    {
        // �t���Ƀ��[�v���񂵂āAactiveList�̗v�f���r���ō폜����Ă����������[�v�����悤�ɂ���
        for (int i = activeList.Count - 1; i >= 0; i--)
        {
            var buffEffect = activeList[i];
            if (!buffEffect.IsActive)
            {
                Remove(buffEffect);
            }
        }
    }

    // �e�L�X�g���A�N�e�B�u�����郁�\�b�h
    public void Remove(BuffEffect buffEffect)
    {
        activeList.Remove(buffEffect);
        inactivePool.Push(buffEffect);
    }

    // �e�L�X�g���A�N�e�B�u�����郁�\�b�h�@�����̂�
    public void Active(Vector2 pos, int num)
    {
        // ��A�N�e�B�u�̃e�L�X�g������Ύg���񂷁A�Ȃ���ΐ�������
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
