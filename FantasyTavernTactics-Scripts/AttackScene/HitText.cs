using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HitText : MonoBehaviour
{
    public bool IsActive => gameObject.activeSelf;
    [SerializeField] Transform Camera;
    private TextMeshProUGUI _text;
    private RectTransform rectTransform;
    private Transform _transform;

    private void Awake()
    {
        if (Camera == null)
            Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        _transform = GetComponent<Transform>();
    }

    public void Init(Vector3 pos, string comboText, Color textColor, int type = 0)
    {
        rectTransform.position = pos;
        Vector3 p = Camera.position;
        p.y = rectTransform.position.y;
        rectTransform.LookAt(p);

        // DOTween�̃V�[�P���X���쐬
        var sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOScale(1f, 0.01f));
        if (type == 0)
            _text.color = textColor;
        else
            _text.color = new Color(0, 110f / 255f, 1, 1);
        _text.text = comboText;

        // �ŏ��Ɋg��\������
        sequence.Append(rectTransform.DOScale(1.2f, 0.5f));

        // ���ɏ�ֈړ�������
        sequence.Append(rectTransform.DOMoveY(0.25f, 1f).SetRelative());

        // ���݂̐F���擾
        var color = _text.color;

        // �A���t�@�l��0�Ɏw�肵�ĕ����𓧖��ɂ���
        color.a = 0.0f;

        // ��Ɉړ��Ɠ����ɔ������ɂ��ď�����悤�ɂ���
        sequence.Join(DOTween.To(() => _text.color, c => _text.color = c, color, 1f).SetEase(Ease.InOutQuart));

        // ���ׂẴA�j���[�V�������I�������A�������g���폜����
        sequence.OnComplete(() => gameObject.SetActive(false));
    }
}
