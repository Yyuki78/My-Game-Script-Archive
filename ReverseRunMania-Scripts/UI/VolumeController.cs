using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Slider BGMSlider;
    [SerializeField]
    private Slider SESlider;

    private float _BGMVol;
    private float _SEVol;

    void Start()
    {
        if (PlayerPrefs.HasKey("BGMVolume"))
            _BGMVol = PlayerPrefs.GetFloat("BGMVolume");
        else
            _BGMVol = 0;
        switch (_BGMVol)
        {
            case -80:
                BGMSlider.value = 0;
                break;
            case -50:
                BGMSlider.value = 1;
                break;
            case -25:
                BGMSlider.value = 3;
                break;
            case 0:
                BGMSlider.value = 3;
                break;
            case 10:
                BGMSlider.value = 4;
                break;
            case 20:
                BGMSlider.value = 5;
                break;
        }

        if (PlayerPrefs.HasKey("SEVolume"))
            _SEVol = PlayerPrefs.GetFloat("SEVolume");
        else
            _SEVol = 0;
        switch (_SEVol)
        {
            case -80:
                SESlider.value = 0;
                break;
            case -50:
                SESlider.value = 1;
                break;
            case -25:
                SESlider.value = 3;
                break;
            case 0:
                SESlider.value = 3;
                break;
            case 10:
                SESlider.value = 4;
                break;
            case 20:
                SESlider.value = 5;
                break;
        }

        audioMixer.SetFloat("BGMVol", _BGMVol);
        audioMixer.SetFloat("SEVol", _SEVol);
    }

    public void SetBGM(float volume)
    {
        switch (volume)
        {
            case 0:
                volume = -80f;
                break;
            case 1:
                volume = -50f;
                break;
            case 2:
                volume = -25f;
                break;
            case 3:
                volume = 0f;
                break;
            case 4:
                volume = 10f;
                break;
            case 5:
                volume = 20f;
                break;
        }
        audioMixer.SetFloat("BGMVol", volume);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSE(float volume)
    {
        switch (volume)
        {
            case 0:
                volume = -80f;
                break;
            case 1:
                volume = -50f;
                break;
            case 2:
                volume = -25f;
                break;
            case 3:
                volume = 0f;
                break;
            case 4:
                volume = 10f;
                break;
            case 5:
                volume = 20f;
                break;
        }
        audioMixer.SetFloat("SEVol", volume);
        PlayerPrefs.SetFloat("SEVolume", volume);
    }

    public void PlaySE()
    {
        AudioManager.instance.SE(20);
    }
}
