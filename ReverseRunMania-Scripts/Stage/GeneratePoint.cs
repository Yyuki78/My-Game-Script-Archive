using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePoint : MonoBehaviour
{
    private GameObject Player;
    private Transform playerPos;
    private Transform myPos;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerPos = Player.GetComponent<Transform>();
        myPos = GetComponent<Transform>();
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pos.x = playerPos.localPosition.x + 250;
        myPos.position = pos;
    }
}
