using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatMove : MonoBehaviour
{
    [SerializeField] bool isStick = false;
    private Vector3 firstPos;
    private Transform _transform;
    [SerializeField] Stage2Controller _controller;

    void Start()
    {
        firstPos = transform.position;
        _transform = GetComponent<Transform>();
    }

    // ドラッグされたときに呼び出されるメソッド
    public void OnMouseDrag()
    {
        if (isStick) return;
        gameObject.layer = 6;
        Vector3 position = Input.mousePosition;
        position.z = 1f;
        Vector3 screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
        _transform.position = screenToWorldPointPosition;
    }

    // ドロップされたときに呼び出されるメソッド
    public void OnMouseUp()
    {
        if (isStick) return;
        _transform.position = firstPos;
        gameObject.layer = 0;
        AudioManager.instance.SE(3);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && gameObject.layer == 0)
        {
            isStick = true;

            transform.parent = collision.gameObject.transform;
            GetComponent<BoxCollider2D>().enabled = false;
            _transform.localPosition = new Vector3(0.02f, 0.6f, -0.1f);

            _controller.GateOpen();
        }
    }
}
