using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceInsideCheck : MonoBehaviour
{
    public int DiceNum = 0;
    private bool isStart = false;

    private AttributeDiceMug attributeDiceMug;

    private void Start()
    {
        attributeDiceMug = GetComponentInParent<AttributeDiceMug>();
        Invoke("Set", 0.1f);
    }

    private void Set()
    {
        isStart = true;
    }

    private void Update()
    {
        if (!isStart) return;
        if (DiceNum > 0) return;
        if (attributeDiceMug.isAllDiceOut) return;
        attributeDiceMug.isAllDiceOut = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AttributeDice")
        {
            DiceNum++;
        }
    }

    //マグの中にダイスがあるか判定する
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "AttributeDice")
        {
            DiceNum--;
        }
    }
}
