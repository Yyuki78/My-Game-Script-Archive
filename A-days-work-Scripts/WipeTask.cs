using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WipeTask : MonoBehaviour
{
    private int wipeNum = 0;

    public void wipe()
    {
        wipeNum++;
        if (wipeNum == 9)
        {
            var manager = GetComponentInParent<TaskManager>();
            manager.FinishTask(0);
            manager.FinishTaskNumber(9);
            this.gameObject.SetActive(false);
        }
    }
}
