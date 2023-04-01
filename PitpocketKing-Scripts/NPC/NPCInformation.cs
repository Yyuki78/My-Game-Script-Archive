using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInformation : MonoBehaviour
{
    //0で一般人,1で警察,2で職業持ち,3で泥棒,4でお金持ち
    public int role;

    private SetClothing _set;

    // Start is called before the first frame update
    void Start()
    {
        if (role == 5) return;

        _set = GetComponent<SetClothing>();

        Invoke("Init", 0.01f);

        //StartCoroutine(test());
    }

    private void Init()
    {
        _set.Init(role);
    }

    private IEnumerator test()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            _set.Init(role);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
