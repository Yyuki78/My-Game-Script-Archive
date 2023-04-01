using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGround : MonoBehaviour
{
    [SerializeField] private GameObject GroundPrefab;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 40; i++)
        {
            for(int l =0; l < 60; l++)
            {
                Vector3 pos = new Vector3(i * 2.5f - 50, 0.0003f, l * 2.5f - 75);
                GameObject obj = Instantiate(GroundPrefab, pos, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
