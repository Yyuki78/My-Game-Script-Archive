using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    public void ReloadScene1()
    {
        // ���݂̃V�[�����擾
        var scene = SceneManager.GetActiveScene();

        // ���݂̃V�[�����ă��[�h����
        SceneManager.LoadScene(scene.name);
    }
}