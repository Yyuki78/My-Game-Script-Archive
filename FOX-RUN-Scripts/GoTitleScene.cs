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
        // �{�^���������������̃��X�i�[��ݒ�
        button.onClick.AddListener(() =>
        {
            // �V�[���J�ڂ̍ۂɂ�SceneManager���g�p����
            SceneManager.LoadScene("TitleScene");
        });
    }
}