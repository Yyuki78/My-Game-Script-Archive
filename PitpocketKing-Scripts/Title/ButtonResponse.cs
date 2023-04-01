using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonResponse : MonoBehaviour
{
    [SerializeField] int buttonMode = 0;

    [SerializeField] GameObject setActiveObj;

    public void OnClick()
    {
        switch (buttonMode)
        {
            case 0:
                setActiveObj.SetActive(true);
                break;
            case 1:
                setActiveObj.SetActive(false);
                break;
            case 2:
                SceneManager.LoadScene("SampleScene");
                break;
            case 3:
                SceneManager.LoadScene("Title");
                break;
            case 4:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
                Application.Quit();//ゲームプレイ終了
#endif
                break;
            default:
                Debug.Log("何もなし");
                break;
        }
    }
}
