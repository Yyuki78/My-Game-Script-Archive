using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LeverMove : MonoBehaviour
{
    private bool canClick = true;
    [SerializeField] Transform lever;
    [SerializeField] Transform hand;

    public void OnClickLever()
    {
        if (canClick)
        {
            canClick = false;
            StartCoroutine(leverEffect());
        }
    }

    private IEnumerator leverEffect()
    {
        AudioManager.instance.SE(6);
        lever.DORotate(new Vector3(0, 0, 50), 0.5f).SetEase(Ease.OutQuad);
        float wait = Random.Range(-1f, 2.7f);
        wait = Mathf.Max(0.6f, wait);
        yield return null;
        lever.DORotate(new Vector3(0, 0, -50), 0.5f).SetEase(Ease.OutQuad).SetDelay(wait);
        hand.DOLocalMoveX(-460f, 0.3f).SetDelay(wait - 0.05f);
        hand.DOLocalMoveX(-505f, 0.8f).SetDelay(wait + 0.3f);

        yield return new WaitForSeconds(wait);
        AudioManager.instance.SE(6);
        yield return new WaitForSeconds(0.5f);

        canClick = true;
        yield break;
    }
}
