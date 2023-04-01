using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadsController : MonoBehaviour
{
    private GameObject[] Loads = new GameObject[22];
    private GameObject Player;

    private int lastLoadPos = 0;
    private float playerPos = 0;
    private int updateCount = 0;
    private bool once = true;
    

    // Start is called before the first frame update
    void Start()
    {
        GameObject parentObject = this.gameObject;
        int count = 0;
        foreach (Transform child in parentObject.transform)
        {
            Loads[count] = child.gameObject;
            count++;
        }
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = Player.transform.localPosition.x;
        if (lastLoadPos < playerPos - 50 && once)
        {
            once = false;
            StartCoroutine(UpdateLoads());
        }
    }

    private IEnumerator UpdateLoads()
    {
        updateCount++;
        lastLoadPos = updateCount * 50;

        Loads[(updateCount - 1) % Loads.Length].transform.localPosition += new Vector3(Loads.Length * 50, 0, 0);

        yield return new WaitForSeconds(0.1f);
        once = true;
        yield break;
    }
}
