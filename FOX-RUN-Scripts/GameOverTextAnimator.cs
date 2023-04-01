using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverTextAnimator : MonoBehaviour
{
    [SerializeField] private GameObject result;
    private void Start()
    {
        var transformCache = transform;
        // �I�_�Ƃ��Ďg�p���邽�߁A�������W��ێ�
        var defaultPosition = transformCache.localPosition;
        // ���������̕��Ɉړ�������
        transformCache.localPosition = new Vector3(0, 300f);
        // �ړ��A�j���[�V�����J�n
        transformCache.DOLocalMove(defaultPosition, 1f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Debug.Log("�Q�[���I�[�o�[");
                // �V�F�C�N�A�j���[�V����
                transformCache.DOShakePosition(1.5f, 100);
            });
        Invoke("Result", 2);
    }

    void Result()
    {
        result.SetActive(true);
    }
}