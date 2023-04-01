using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EventPopUpText : MonoBehaviour
{
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = this.gameObject.GetComponent<Text>();
        // DOTween�̃V�[�P���X���쐬
        var sequence = DOTween.Sequence();

        // �ŏ��Ɋg��\������
        sequence.Append(transform.DOScale(1.2f, 0.5f));

        // ���ɏ�ֈړ�������
        sequence.Append(transform.DOMoveY(5.0f, 1.25f).SetRelative());

        // ���݂̐F���擾
        var color = text.color;

        // �A���t�@�l��0�Ɏw�肵�ĕ����𓧖��ɂ���
        color.a = 0.0f;

        // ��Ɉړ��Ɠ����ɔ������ɂ��ď�����悤�ɂ���
        sequence.Join(DOTween.To(() => text.color, c => text.color = c, color, 2.5f).SetEase(Ease.InOutQuart));

        // ���ׂẴA�j���[�V�������I�������A�������g���폜����
        sequence.OnComplete(() => Destroy(gameObject));
    }
}
