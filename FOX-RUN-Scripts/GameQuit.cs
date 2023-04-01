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
        // �{�^���������������̃��X�i�[��ݒ�
        button.onClick.AddListener(() =>
        {
            //�Q�[�����I��������
            Application.Quit();
        });
    }
}
