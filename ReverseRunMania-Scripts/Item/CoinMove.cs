using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMove : MonoBehaviour
{
    public bool isMagnet = false;

    public bool IsActive => gameObject.activeSelf;

    private GameObject Player;
    private GameObject MagnetCenter;
    private Coroutine coroutine;
    private Coroutine playCoroutine;
    private float x_Abs, y_Abs, z_Abs;
    private float speedParameter = 20;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!isMagnet) return;
        MagnetCenter = GameObject.FindGameObjectWithTag("Magnet");
        if (MagnetCenter == null) return;
        x_Abs = Mathf.Abs(this.gameObject.transform.position.x - MagnetCenter.transform.position.x);
        y_Abs = Mathf.Abs(this.gameObject.transform.position.y - MagnetCenter.transform.position.y);
        z_Abs = Mathf.Abs(this.gameObject.transform.position.z - MagnetCenter.transform.position.z);

        if (coroutine == null)
        {
            coroutine = StartCoroutine(MoveCoroutine());
        }
    }

    IEnumerator MoveCoroutine()
    {
        float speed = speedParameter * Time.deltaTime;
        var wait = new WaitForEndOfFrame();

        while (x_Abs > 0 || y_Abs > 0 || z_Abs > 0)
        {
            yield return wait;
            if (MagnetCenter == null) yield break;
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, MagnetCenter.transform.position, speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            if (playCoroutine == null)
            {
                playCoroutine = StartCoroutine(getEffect());
            }
        }
        if (other.CompareTag("BackMostWall"))
        {
            StartCoroutine(destroy());
        }
    }

    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private IEnumerator getEffect()
    {
        Vector3 plusPos = new Vector3(0, 0.02f, 0);
        if (transform.position.y > 5f)
            plusPos *= -1;
        for (int i = 0; i < 10; i++)
        {
            transform.localPosition += plusPos;
            yield return null;
        }
        Player.GetComponent<ItemManager>().GetCoin(1);
        for (int i = 0; i < 5; i++)
        {
            transform.localPosition -= plusPos;
            yield return null;
        }

        isMagnet = false;

        gameObject.SetActive(false);
        yield break;
    }

    public void Init(Vector3 origin, Quaternion rotate)
    {
        transform.position = origin;
        transform.rotation = rotate;

        gameObject.SetActive(true);
        coroutine = null;
        playCoroutine = null;
        isMagnet = false;
    }
}
