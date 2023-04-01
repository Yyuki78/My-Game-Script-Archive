using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearTextAnimator : MonoBehaviour
{
    [SerializeField] private GameObject result;
    // Start is called before the first frame update
    void Start()
    {
        var transformCache = transform;
        var defaultPosition = transformCache.localPosition;
        transform.DOScale(
                new Vector3(1f, 1f, 1f), // �X�P�[���l
                2f                    // ���o����
            );
        Invoke("Result", 2);
    }

    void Result()
    {
        result.SetActive(true);
    }
}
