using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HitEffect : MonoBehaviour
{
    [SerializeField] GameObject HPGauges;
    [SerializeField] Image GreenGauge;
    [SerializeField] Image RedGauge;

    private PieceInfomation _info;
    private Tween _redGaugeTween;
    private Coroutine _playCoroutine;

    private void Awake()
    {
        _info = GetComponentInParent<PieceInfomation>();
        HPGauges.SetActive(false);
        GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void GaugeReduction(int reducationValue, float time = 1.5f)
    {
        if (_playCoroutine != null)
        {
            StopCoroutine(_playCoroutine);
            _playCoroutine = null;
        }
        HPGauges.SetActive(true);
        float valueFrom = (_info.HP + reducationValue) / (float)_info.MaxHP;
        float valueTo;
        if (_info.HP <= 0)
            valueTo = 0;
        else
            valueTo = _info.HP / (float)_info.MaxHP;

        // —ÎƒQ[ƒWŒ¸­
        GreenGauge.fillAmount = valueTo;

        if (_redGaugeTween != null)
        {
            _redGaugeTween.Kill();
        }

        // ÔƒQ[ƒWŒ¸­
        _redGaugeTween = DOTween.To(
            () => valueFrom,
            x => {
                RedGauge.fillAmount = x;
            },
            valueTo,
            time
        );
    }
    private IEnumerator HideCoroutine()
    {
        yield return new WaitForSeconds(2f);
        HPGauges.SetActive(false);
        yield break;
    }
}
