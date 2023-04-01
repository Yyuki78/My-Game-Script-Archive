using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTask : MonoBehaviour
{
    [SerializeField] GameObject[] Barrel;
    [SerializeField] GameObject[] BreakParticle;

    private int setNum = 0;

    private AudioSource[] _audio = new AudioSource[2];
    [SerializeField] AudioClip _clip;

    // Start is called before the first frame update
    void Start()
    {
        _audio[0] = Barrel[2].GetComponent<AudioSource>();
        _audio[1] = Barrel[3].GetComponent<AudioSource>();
    }

    public void SetBarrel()
    {
        setNum++;
        if (setNum == 2)
        {
            StartCoroutine(CrushingEffect());
        }
    }

    private IEnumerator CrushingEffect()
    {
        var wait = new WaitForSeconds(0.5f);
        var wait2 = new WaitForSeconds(0.08f);
        yield return wait;
        BreakParticle[0].SetActive(true);
        BreakParticle[1].SetActive(true);

        _audio[0].PlayOneShot(_clip);
        _audio[1].PlayOneShot(_clip);

        Vector3 scale= new Vector3(0, 0.02f, 0);
        Vector3 pos = new Vector3(0, 0.01f, 0);
        for (int i = 0; i < 40; i++)
        {
            Barrel[0].transform.localScale -= scale;
            Barrel[1].transform.localScale -= scale;
            Barrel[0].transform.localPosition += pos;
            Barrel[1].transform.localPosition += pos;
            yield return wait2;
        }
        Barrel[0].transform.localScale = new Vector3(1, 0.2f, 1);
        Barrel[1].transform.localScale = new Vector3(1, 0.2f, 1);

        yield return wait;

        Barrel[0].SetActive(false);
        Barrel[1].SetActive(false);
        Barrel[2].SetActive(false);
        Barrel[3].SetActive(false);
        BreakParticle[2].SetActive(true);

        var _manager = GetComponentInParent<TaskManager>();
        _manager.FinishTask(1);

        yield break;
    }
}
