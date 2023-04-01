using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveVolume : MonoBehaviour
{
    private float _MasterVol;
    public float MasterVolume
    {
        get { return _MasterVol; }
        set { _MasterVol = value; }
    }
    private float _BGMVol;
    public float BGMVolume
    {
        get { return _BGMVol; }
        set { _BGMVol = value; }
    }
    private float _SEVol;
    public float SEVolume
    {
        get { return _SEVol; }
        set { _SEVol = value; }
    }
    // Start is called before the first frame update
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

        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            _BGMVol = PlayerPrefs.GetFloat("BGMVolume");
        }
        else
        {
            _BGMVol = 0;
        }

        if (PlayerPrefs.HasKey("SEVolume"))
        {
            _SEVol = PlayerPrefs.GetFloat("SEVolume");
        }
        else
        {
            _SEVol = 0;
        }
    }
}
