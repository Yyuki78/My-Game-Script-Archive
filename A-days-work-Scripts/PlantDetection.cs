using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDetection : MonoBehaviour
{
    public int plantNum = 0;//0�͑傫����,1�͏�������

    private WaterTask _task;
    [SerializeField] AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        _task = GetComponentInParent<WaterTask>();
    }

    private void OnParticleCollision(GameObject hitObject)
    {
        if (hitObject.CompareTag("mTask1"))
        {
            int num = hitObject.GetComponent<PlantDetection>().plantNum;
            _task.hitWater(num);
            _audio.volume = 0.15f;
        }
    }
}
