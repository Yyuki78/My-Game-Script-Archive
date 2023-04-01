using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class ButtonAnimation : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] int ButtonMode = 0; //inspectorで変更する

    [SerializeField] bool isAppearanceEffect = true;
    [SerializeField] float waitTime = 4f;

    private CanvasGroup _canvasGroup;

    private TextMeshProUGUI _text;

    private AudioManager _audio;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        transform.localScale = new Vector3(0, 0, 0);
        if(isAppearanceEffect)
            StartCoroutine(Appearance());
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    private IEnumerator Appearance()
    {
        yield return new WaitForSeconds(waitTime);
        for(int i = 0; i < 55; i++)
        {
            transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 5; i++)
        {
            transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localScale = new Vector3(1f, 1f, 1f);
        yield break;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        // ボタンが押される
        switch (ButtonMode)
        {
            case 0:
                transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(0.8f, 0.24f).SetEase(Ease.OutCubic);
                _text.fontStyle = FontStyles.Underline;
                _text.color = new Color(0, 0, 0, 1);

                _audio.SE19();
                break;
            case 1: //再生ボタン
                transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(0.8f, 0.24f).SetEase(Ease.OutCubic);

                _audio.SE19();
                break;
            case 2: //一時停止・再生ボタン
                transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(0.8f, 0.24f).SetEase(Ease.OutCubic);

                _audio.SE19();
                break;
            default:
                Debug.Log("ミス");
                _audio.SE19();
                break;
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        // ボタンが離される
        switch (ButtonMode)
        {
            case 0:
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);
                _text.fontStyle = FontStyles.Normal;
                _text.color = new Color(0.2f, 0.2f, 0.2f, 1);
                break;
            case 1: //再生ボタン
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);

                _canvasGroup.DOFade(0f, 1.5f).SetEase(Ease.OutCubic);

                StartCoroutine(Wait());
                break;
            case 2: //一時停止・再生ボタン
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            default:
                Debug.Log("ミス");
                break;
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        // ボタンが押され、その後ドラッグ操作が入ることなくボタンが離される
        switch (ButtonMode)
        {
            case 0:
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);
                _text.fontStyle = FontStyles.Normal;
                _text.color = new Color(0.2f, 0.2f, 0.2f, 1);
                break;
            case 1: //再生ボタン
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);

                _canvasGroup.DOFade(0f, 1.5f).SetEase(Ease.OutCubic);

                StartCoroutine(Wait());
                break;
            case 2: //一時停止・再生ボタン
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            default:
                Debug.Log("ミス");
                break;
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        //ボタンの範囲にマウスカーソルが入る
        switch (ButtonMode)
        {
            case 0:
                transform.DOScale(1.1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1.05f, 0.24f).SetEase(Ease.OutCubic);
                _text.fontStyle = FontStyles.Underline;
                _text.color = new Color(0, 0, 0, 1);
                break;
            case 1: //再生ボタン
                transform.DOScale(1.2f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1.1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            case 2: //一時停止・再生ボタン
                transform.DOScale(1.3f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1.15f, 0.24f).SetEase(Ease.OutCubic);
                break;
            default:
                Debug.Log("ミス");
                break;
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        //ボタンの範囲からマウスカーソルが出る
        switch (ButtonMode)
        {
            case 0:
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);
                _text.fontStyle = FontStyles.Normal;
                _text.color = new Color(0.2f, 0.2f, 0.2f, 1);
                break;
            case 1: //再生ボタン
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            case 2: //一時停止・再生ボタン
                transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);
                _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);
                break;
            default:
                Debug.Log("ミス");
                break;
        }
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("1.0秒遅延しました");
        this.gameObject.SetActive(false);
        yield break;
    }
}
