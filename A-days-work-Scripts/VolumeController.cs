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

    [SerializeField] AudioSource _audio;

    void Start()
    {
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
        audioMixer.SetFloat("BGMVol", _BGMVol);
        audioMixer.SetFloat("SEVol", _SEVol);
    }

    public void SetBGM(float volume)
    {
        if (volume == -40f)
            volume = -80f;
        audioMixer.SetFloat("BGMVol", volume);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSE(float volume)
    {
        if (volume == -40f)
            volume = -80f;
        audioMixer.SetFloat("SEVol", volume);
        PlayerPrefs.SetFloat("SEVolume", volume);
    }

    public void PlaySE()
    {
        _audio.PlayOneShot(_audio.clip);
    }
}