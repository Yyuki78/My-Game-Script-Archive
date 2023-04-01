using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GameQuit : MonoBehaviour
{
    private void Start()
    {
        var button = GetComponent<Button>();
        // ボタンを押下した時のリスナーを設定
        button.onClick.AddListener(() =>
        {
            //ゲームを終了させる
            Application.Quit();
        });
    }
}
