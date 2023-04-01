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
    private Slider MasterSlider;
    [SerializeField]
    private Slider BGMSlider;
    [SerializeField]
    private Slider SESlider;

    private float _MasterVol;

    private float _BGMVol;

    private float _SEVol;

    void Start()
    {

        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            _MasterVol = PlayerPrefs.GetFloat("MasterVolume");
        }
        else
        {
            _MasterVol = 0;
        }
        MasterSlider.value = _MasterVol;

        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            _BGMVol = PlayerPrefs.GetFloat("BGMVolume");
        }
        else
        {
            _BGMVol = 0;
        }
        BGMSlider.value = _BGMVol;

        if (PlayerPrefs.HasKey("SEVolume"))
        {
            _SEVol = PlayerPrefs.GetFloat("SEVolume");
        }
        else
        {
            _SEVol = 0;
        }
        SESlider.value = _SEVol;

        audioMixer.SetFloat("MasterVol", _MasterVol);
        audioMixer.SetFloat("BGMVol", _BGMVol);
        audioMixer.SetFloat("SEVol", _SEVol);
    }


    public void SetMaster(float volume)
    {
        audioMixer.SetFloat("MasterVol", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetBGM(float volume)
    {
        audioMixer.SetFloat("BGMVol", volume);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSE(float volume)
    {
        audioMixer.SetFloat("SEVol", volume);
        PlayerPrefs.SetFloat("SEVolume", volume);
    }
}
