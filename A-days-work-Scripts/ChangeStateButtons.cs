using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStateButtons : MonoBehaviour
{
    [SerializeField] Fade _fade;

    private AudioSource _audio;
    [SerializeField] AudioClip _clip;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void OnClick(int type)
    {
        _audio.PlayOneShot(_clip);
        switch (type)
        {
            case 0://ゲームスタート
                _fade.changeScene(2);
                break;
            case 1://ゲームリセット
                _fade.changeScene(0);
                break;
            case 2://ゲームをやめる
                Application.Quit();
                break;
            default:
                break;
        }
    }
}
