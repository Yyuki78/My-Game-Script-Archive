using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTask : MonoBehaviour
{
    private int hitNum1 = 0;
    private int hitNum2 = 0;

    [SerializeField] ParticleSystem particle;
    [SerializeField] GameObject[] Plants;

    private bool once = true;

    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponentInChildren<AudioSource>();
    }

    public void hitWater(int plantType)
    {
        if (plantType == 0)
        {
            hitNum1++;
        }
        if (plantType == 1)
        {
            hitNum2++;
        }

        if (hitNum1 > 100)
        {
            Destroy(Plants[0].GetComponent<Outline>());
            Destroy(Plants[1].GetComponent<Outline>());
        }

        if (hitNum2 > 100)
        {
            Destroy(Plants[2].GetComponent<Outline>());
            Destroy(Plants[3].GetComponent<Outline>());
        }

        if (hitNum1 > 100 && hitNum2 > 100 && once)
        {
            once = false;
            var manager = GetComponentInParent<TaskManager>();
            manager.FinishTask(1);

            Destroy(Plants[4].GetComponent<Outline>());

            this.gameObject.SetActive(false);
        }
    }

    public void ReStartParticle()
    {
        _audio.Play();
        particle.Play(true);
        if (Plants[4].GetComponent<Outline>() != null)
            Destroy(Plants[4].GetComponent<Outline>());
    }

    public void StopParticle()
    {
        _audio.Pause();
        particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
