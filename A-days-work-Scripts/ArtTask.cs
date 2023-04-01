using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtTask : MonoBehaviour
{
    [SerializeField] GameObject[] Quarters;
    [SerializeField] GameObject[] vanishObjs;
    [SerializeField] GameObject clearObj;
    [SerializeField] GameObject CoverSphere;

    private int setNum = 0;

    public void SetArt()
    {
        CoverSphere.SetActive(true);
        setNum++;
        if (setNum == 4)
        {
            StartCoroutine(clearEffect());
        }
    }

    private IEnumerator clearEffect()
    {
        for (int i = 0; i < vanishObjs.Length; i++)
        {
            vanishObjs[i].SetActive(false);
        }
        yield return new WaitForSeconds(0.5f);

        var wait= new WaitForSeconds(0.01f);
        Vector3 pos = new Vector3(0, 0.001f, 0);
        for (int i = 0; i < 50; i++)
        {
            Quarters[0].transform.localPosition += pos;
            Quarters[1].transform.localPosition += pos;
            Quarters[2].transform.localPosition += pos;
            Quarters[3].transform.localPosition += pos;
            yield return wait;
        }
        yield return new WaitForSeconds(0.5f);
        pos = new Vector3(-0.004f, 0, 0.004f);
        Vector3 pos2 = new Vector3(0.004f, 0, 0.004f);
        for (int i = 0; i < 12; i++)
        {
            Quarters[0].transform.localPosition += pos;
            Quarters[1].transform.localPosition += pos2;
            Quarters[2].transform.localPosition -= pos;
            Quarters[3].transform.localPosition -= pos2;
            yield return wait;
        }
        yield return new WaitForSeconds(0.1f);

        clearObj.SetActive(true);

        yield return new WaitForSeconds(6.5f);

        var manager = GetComponentInParent<TaskManager>();
        manager.FinishTask(2);

        Quarters[0].SetActive(false);
        Quarters[1].SetActive(false);
        Quarters[2].SetActive(false);
        Quarters[3].SetActive(false);
        CoverSphere.SetActive(false);

        yield break;
    }
}
