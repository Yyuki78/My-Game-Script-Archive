using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakGate : MonoBehaviour
{
    [SerializeField] GameObject Gate;
    [SerializeField] Image[] hibi;
    private Stage8Controller _controller;
    
    private List<GameObject> myParts = new List<GameObject>();
    private Rigidbody2D[] _rigid = new Rigidbody2D[24];

    void Start()
    {
        _controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Stage8Controller>();

        int i = 0;
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.AddComponent<Rigidbody2D>();
            _rigid[i] = child.gameObject.GetComponent<Rigidbody2D>();
            _rigid[i].isKinematic = true;
            i++;
            
            myParts.Add(child.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        if (hibi[0].fillAmount >= 1f)
        {
            AudioManager.instance.SE(9);
            _controller.GateOpen();
            
            for (int i = 0; i < hibi.Length; i++)
                hibi[i].gameObject.SetActive(false);

            int j = 0;
            foreach (GameObject obj in myParts)
            {
                // 飛ばすパワーと回転をランダムに設定
                Vector2 forcePower = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                float torquePower = Random.Range(-5f, 5f);

                // パーツをふっとばす！
                _rigid[j].isKinematic = false;
                _rigid[j].AddForce(forcePower, ForceMode2D.Impulse);
                _rigid[j].AddTorque(torquePower, ForceMode2D.Impulse);
                j++;
            }

            GetComponent<BoxCollider2D>().isTrigger = true;
            StartCoroutine(destroy());

            return;
        }

        for (int i = 0; i < hibi.Length; i++)
            hibi[i].fillAmount += 0.2f;
        AudioManager.instance.SE(8);
    }

    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(2f);
        foreach (GameObject obj in myParts)
        {
            obj.SetActive(false);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
