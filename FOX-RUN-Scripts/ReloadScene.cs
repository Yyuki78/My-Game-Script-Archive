using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    public void ReloadScene1()
    {
        // 現在のシーンを取得
        var scene = SceneManager.GetActiveScene();

        // 現在のシーンを再ロードする
        SceneManager.LoadScene(scene.name);
    }
}