using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiyokoPull : MonoBehaviour
{
    [SerializeField] float boundsPower = 0.1f;
    [SerializeField] GameObject DirectionImage;
    private Transform _transform;

    private HiyokoInfomation _info;
    private Rigidbody2D _rigid;

    void Start()
    {
        _info = GetComponent<HiyokoInfomation>();
        DirectionImage.SetActive(false);
        _transform = DirectionImage.GetComponent<Transform>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_rigid.velocity.magnitude < 1f && _info._currentState == HiyokoInfomation.State.Bound)
        {
            _info.SetState(HiyokoInfomation.State.Normal);
        }
    }

    // ドラッグされたときに呼び出されるメソッド
    public void OnMouseDrag()
    {
        if (!_info.IsPick) return;
        _info.SetState(HiyokoInfomation.State.Pick);

        DirectionImage.SetActive(true);

        Vector3 position = Input.mousePosition;
        position.z = 0.1f;
        Vector3 screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);

        Vector3 boundVec = this.transform.position - screenToWorldPointPosition;
        float distance = boundVec.sqrMagnitude - 81f;
        if (distance > 50)
            distance = 50f;

        screenToWorldPointPosition.z = 0;
        _transform.up = (screenToWorldPointPosition - _transform.position) * -1;
        _transform.localScale = new Vector3(1, distance / 30f + 0.35f, 1);
    }

    // ドロップされたときに呼び出されるメソッド
    public void OnMouseUp()
    {
        if (!_info.IsPick) return;
        _info.SetState(HiyokoInfomation.State.Bound);

        DirectionImage.SetActive(false);

        //計算して発射
        Vector3 position = Input.mousePosition;
        position.z = 1f;
        Vector3 screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);

        Vector3 boundVec = this.transform.position - screenToWorldPointPosition;
        float distance = boundVec.sqrMagnitude - 81f;
        if (distance > 50)
            distance = 50f;

        Vector3 forceDir = boundsPower * boundVec.normalized * distance;
        _rigid.AddForce(forceDir, ForceMode2D.Impulse);
    }
}
