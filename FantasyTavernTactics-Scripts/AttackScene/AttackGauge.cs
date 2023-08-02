using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AttackGauge : MonoBehaviour
{
    public bool isFinishGame = false;//AttackManager
    public float gaugeValue = 0f;//MagicianAttackのみ書き換え
    public bool isGaugeMax { private set; get; } = false;
    private bool reachGaugeMax = false;

    [SerializeField] Transform Camera;
    private Transform _transform;

    [SerializeField] GameObject Gauges;
    [SerializeField] Image Gauge;

    [SerializeField] RectTransform ParentScoreText;
    [SerializeField] ScoreText scoreText;
    private float score;

    private Tween _gaugeTween;
    private Coroutine _playCoroutine;

    // アクティブなテキストのリスト
    private List<ScoreText> activeList = new List<ScoreText>();
    // 非アクティブなテキストのオブジェクトプール
    private Stack<ScoreText> inactivePool = new Stack<ScoreText>();

    void Awake()
    {
        if (Camera == null)
            Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        _transform = GetComponent<Transform>();
        Gauges.SetActive(false);
    }
    
    void Update()
    {
        Vector3 p = Camera.position;
        p.y = _transform.position.y;
        _transform.LookAt(p);

        for (int i = activeList.Count - 1; i >= 0; i--)
        {
            var scoreText = activeList[i];
            if (!scoreText.IsActive)
            {
                Remove(scoreText);
            }
        }
    }

    public void ChangeGauge(float value, float time = 1f)
    {
        if (isFinishGame) return;
        if (_playCoroutine != null)
        {
            StopCoroutine(_playCoroutine);
            _playCoroutine = null;
        }
        Active(value);
        Gauges.SetActive(true);
        float valueFrom = gaugeValue / 150f;
        Gauge.fillAmount = valueFrom;
        float valueTo;
        if (gaugeValue + value <= 0)
            valueTo = 0;
        else
            valueTo = (gaugeValue + value) / 150f;

        gaugeValue = gaugeValue + value;
        if (gaugeValue > 150f)
            gaugeValue = 150f;

        if (gaugeValue >= 100)
            isGaugeMax = true;
        else if (gaugeValue >= 0)
        {
            isGaugeMax = false;
            reachGaugeMax = false;
        }
        else
            gaugeValue = 0;

        if (isGaugeMax && !reachGaugeMax)
        {
            reachGaugeMax = true;
            AudioManager.Instance.SE(4);
        }

        if (_gaugeTween != null)
            _gaugeTween.Kill();

        _gaugeTween = DOTween.To(
            () => valueFrom,
            x => {
                Gauge.fillAmount = x;
            },
            valueTo,
            time
        ).OnComplete(() => {
            if(gameObject.activeSelf)
                _playCoroutine = StartCoroutine(HideCoroutine());
        });
    }
    private IEnumerator HideCoroutine()
    {
        yield return new WaitForSeconds(3f);
        Gauges.SetActive(false);
        yield break;
    }

    // テキストを非アクティブ化するメソッド
    private void Remove(ScoreText scoreText)
    {
        activeList.Remove(scoreText);
        inactivePool.Push(scoreText);
    }

    // テキストをアクティブ化するメソッド　自分のみ
    private void Active(float score)
    {
        // 非アクティブのテキストがあれば使い回す、なければ生成する
        var ScoreText = (inactivePool.Count > 0) ? inactivePool.Pop() : Instantiate(scoreText, transform);

        Vector3 pos = ParentScoreText.localPosition + new Vector3(1.05f - (2.1f * gaugeValue / 150f), 0, 0);
        ScoreText.gameObject.SetActive(true);
        ScoreText.Init(pos, score);
        activeList.Add(ScoreText);
    }
}
