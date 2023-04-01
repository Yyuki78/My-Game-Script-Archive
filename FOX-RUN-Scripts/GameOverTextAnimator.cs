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
        // 終点として使用するため、初期座標を保持
        var defaultPosition = transformCache.localPosition;
        // いったん上の方に移動させる
        transformCache.localPosition = new Vector3(0, 300f);
        // 移動アニメーション開始
        transformCache.DOLocalMove(defaultPosition, 1f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Debug.Log("ゲームオーバー");
                // シェイクアニメーション
                transformCache.DOShakePosition(1.5f, 100);
            });
        Invoke("Result", 2);
    }

    void Result()
    {
        result.SetActive(true);
    }
}