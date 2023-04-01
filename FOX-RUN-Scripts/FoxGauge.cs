using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxGauge : MonoBehaviour
{
    //範囲外に出たときに満足ゲージを減らす
    private EnemyMove _move;
    public bool reduse = false;

    public void OnTriggerEnter(Collider other)
    {
        reduse = false;
    }

    public void OnTriggerExit(Collider other)
    {
        reduse = true;
    }
}
