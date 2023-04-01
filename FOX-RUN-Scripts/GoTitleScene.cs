using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GoTitleScene : MonoBehaviour
{
    private void Start()
    {
        var button = GetComponent<Button>();
        // ボタンを押下した時のリスナーを設定
        button.onClick.AddListener(() =>
        {
            // シーン遷移の際にはSceneManagerを使用する
            SceneManager.LoadScene("TitleScene");
        });
    }
}