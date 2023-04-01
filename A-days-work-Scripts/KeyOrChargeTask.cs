using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyOrChargeTask : MonoBehaviour
{
    [SerializeField] int TaskType = 0;//0はキー,1はチャージ,2は円柱

    [SerializeField] GameObject taskObj;
    [SerializeField] GameObject transparentObj;
    [SerializeField] GameObject effectObj;

    [SerializeField] Material[] changeMat;

    private int setNum = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetSomething()
    {
        setNum++;
        if (setNum == 1)
        {
            if (TaskType == 0)
            {
                StartCoroutine(effect1());
            }
            else if (TaskType == 1)
            {
                StartCoroutine(effect2());
            }
        }
        else if (setNum == 2 && TaskType != 2)
        {
            var manager = GetComponentInParent<TaskManager>();
            manager.FinishTask(2);
        }
        else if (setNum == 3 && TaskType == 2)
        {
            var manager = GetComponentInParent<TaskManager>();
            manager.FinishTask(2);
        }
    }

    private IEnumerator effect1()
    {
        yield return new WaitForSeconds(0.25f);
        effectObj.SetActive(true);

        yield return new WaitForSeconds(5f);

        taskObj.SetActive(true);
        transparentObj.SetActive(true);
    }

    private IEnumerator effect2()
    {
        Material[] mat = taskObj.GetComponentInChildren<Renderer>().materials;

        yield return new WaitForSeconds(0.25f);

        effectObj.SetActive(true);
        var wait = new WaitForSeconds(0.5f);
        for (int i = 0; i < 10; i++)
        {
            yield return wait;
            mat[0] = changeMat[i];
            taskObj.GetComponentInChildren<Renderer>().materials = mat;
        }

        yield return new WaitForSeconds(0.5f);

        taskObj.AddComponent<Outline>();
        taskObj.GetComponent<XRGrabInteractable>().enabled = true;
        taskObj.GetComponent<Rigidbody>().isKinematic = false;
        transparentObj.SetActive(true);
    }
}
