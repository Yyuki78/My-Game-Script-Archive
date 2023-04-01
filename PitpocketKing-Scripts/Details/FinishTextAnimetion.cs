using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTextAnimetion : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(finish());
    }

    private IEnumerator finish()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        for(int i = 0; i < 50; i++)
        {
            transform.localPosition -= new Vector3(0, 9f, 0);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield return new WaitForSecondsRealtime(2f);
        for (int i = 0; i < 50; i++)
        {
            transform.localPosition -= new Vector3(0, 18f, 0);
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}
