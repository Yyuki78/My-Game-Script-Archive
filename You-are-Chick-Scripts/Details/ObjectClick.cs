using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectClick : MonoBehaviour
{
    [SerializeField] Transform objectName;
    [SerializeField] int[] releaseNum;

    private Stage4Controller _controller;
    private bool once = true;

    void Start()
    {
        _controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Stage4Controller>();
        objectName.gameObject.SetActive(false);
    }

    // クリックされたときに呼び出されるメソッド
    public void OnMouseDown()
    {
        if (once)
        {
            once = false;
            StartCoroutine(releaseCharacter());
        }
    }

    private IEnumerator releaseCharacter()
    {
        objectName.localScale = new Vector3(1, 0, 1);
        objectName.gameObject.SetActive(true);
        yield return null;
        objectName.DOScale(new Vector3(1, 1, 1), 0.5f);

        yield return new WaitForSeconds(0.75f);

        _controller.ActiveCharacter(releaseNum);

        yield break;
    }
}
