using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMostWall : MonoBehaviour
{
    private GameObject Player;
    private Vector3 pos;
    private float MaxPos;
    
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        pos = transform.position;
    }
    
    void Update()
    {
        if (MaxPos > Player.transform.localPosition.x) return;
        MaxPos = Player.transform.localPosition.x;
        pos.x = MaxPos - 25;
        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerBody")) return;
        GameObject.FindGameObjectWithTag("Tank").GetComponent<PostEffect>().changeScene(2);
    }
}
