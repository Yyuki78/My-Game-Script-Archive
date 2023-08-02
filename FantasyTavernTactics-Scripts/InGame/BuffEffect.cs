using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffect : MonoBehaviour
{
    public bool IsActive => gameObject.activeSelf;
    [SerializeField] GameObject[] Effects;

    void Start()
    {
        for (int i = 0; i < Effects.Length; i++)
        {
            Effects[i].SetActive(false);
        }
    }

    public void Init(Vector3 pos, int num)
    {
        transform.localPosition = pos;
        StartCoroutine(waitParticle(num));
    }

    WaitForSeconds wait = new WaitForSeconds(0.5f);
    WaitForSeconds wait2 = new WaitForSeconds(4f);
    private IEnumerator waitParticle(int num)
    {
        yield return wait;
        Effects[num].SetActive(true);
        yield return wait2;
        Effects[num].SetActive(false);
        gameObject.SetActive(false);
    }
}
