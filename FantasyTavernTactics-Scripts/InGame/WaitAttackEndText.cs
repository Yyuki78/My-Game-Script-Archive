using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class WaitAttackEndText : MonoBehaviour
{
    [SerializeField] GameObject waitAttackEndText;

    private int BoardSize = 0;
    private Tween _moveTween;

    private Transform Camera;

    private void Awake()
    {
        BoardSize = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>().BoardSize;
        waitAttackEndText.SetActive(false);
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    public void ShowText(PieceInfomation info, float time = 2f)
    {
        Vector3 pos = new Vector3(0, 0, 0);
        switch (BoardSize)
        {
            case 5:
                pos = new Vector3(4.579f + (info.CurrentPosition.y - 1) * 0.2f, 1.437f, 4.097f + (info.CurrentPosition.x - 1) * 0.2f);
                break;
            case 6:
                pos = new Vector3(4.559f + (info.CurrentPosition.y - 1) * 0.168f, 1.437f, 4.077f + (info.CurrentPosition.x - 1) * 0.168f);
                break;
            case 7:
                pos = new Vector3(4.55f + (info.CurrentPosition.y - 1) * 0.143f, 1.437f, 4.067f + (info.CurrentPosition.x - 1) * 0.143f);
                break;
        }
        waitAttackEndText.transform.position = pos;

        Vector3 p = Camera.position;
        p.y = waitAttackEndText.transform.position.y;
        waitAttackEndText.transform.LookAt(p);

        waitAttackEndText.SetActive(true);

        if (_moveTween != null)
        {
            _moveTween.Kill();
        }

        _moveTween = waitAttackEndText.transform.DOLocalMoveY(0.0025f, time).SetDelay(0.5f).OnComplete(() => { waitAttackEndText.SetActive(false); });
    }
}
